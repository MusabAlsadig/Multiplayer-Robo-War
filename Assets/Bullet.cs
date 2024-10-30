using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace ServerSide
{
    public class Bullet : NetworkBehaviour
    {
        [Header("Proprties")]
        public int maxDamage;
        [SerializeField] private int force;
        [SerializeField] public int maxDistance;
        [SerializeField] private ForceMode forceMode;

        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private ParticleSystem onHitParticles;

        private Vector3 startPosition;
        private bool willGetDespawned = false;

        private ulong senderID;
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                Destroy(rb);
                return;
            }
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, startPosition) > maxDistance)
                Clear();
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out HealthSystem healthSystem))
            {
                if (PlayersHolder.AreAllies(healthSystem.OwnerClientId, senderID))
                {
                    // ally don't take damage
                }

                else
                {
                    healthSystem.Server_TakeDamage(maxDamage, senderID);
                    ShowDamage_ClientRpc(transform.position, maxDamage);
                }
            }

            Clear();

            if (onHitParticles != null)
            {
                Instantiate(onHitParticles, transform.position, transform.rotation);
            }
        }

        public void Fire(Vector3 direction, ulong senderID)
        {
            rb.AddForce(direction * force, forceMode);
            this.senderID = senderID;

            startPosition = transform.position;
        }


        [ClientRpc]
        private void ShowDamage_ClientRpc(Vector3 position, int damage)
        {
            if (!IsOwner)
                return;

            Popups.Instance.ShowDamageDelt(damage, position);
        }

        private void Clear()
        {
            if (willGetDespawned)
                return;

            if (!IsServer)
                return;

            willGetDespawned = true;
            NetworkObject.Despawn();
        }
    }
}