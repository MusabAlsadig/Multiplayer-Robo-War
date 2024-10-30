using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageSource : NetworkBehaviour
{
    [SerializeField] private short damageOnContact = 1;
    [SerializeField, Range(0, 1)] private float delay = 0.1f;

    private readonly List<HealthSystem> healthSystems = new List<HealthSystem>();
    private float time;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        ResetTime();
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            ResetTime();
            DealDamageToAll();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystems.Add(healthSystem);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystems.Remove(healthSystem);
        }
    }

    private void ResetTime()
    {
        time = delay;
    }

    private void DealDamageToAll()
    {
        foreach (var healthSystem in healthSystems)
        {
            healthSystem.Server_TakeDamage(damageOnContact);
        }
    }
}
