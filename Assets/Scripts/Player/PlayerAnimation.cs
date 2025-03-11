using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    [SerializeField] private Animator animator;

    private HealthSystem healthSystem;

    private const string ExplodString = "exploded";
    private const string DeathString = "isDead";

    [Header("Prefabs"), SerializeField]
    private GameObject explotionVFX;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;


        healthSystem.OnDeath += Owner_Death;
        healthSystem.OnExplod += Owner_Explode;
        healthSystem.OnRevive += Owner_Revive;
    }

    public void Owner_Death()
    {
        animator.SetBool(DeathString, true);
    }

    public void Owner_Explode()
    {
        animator.SetBool(ExplodString, true);
        SpawnExplotion_ServerRpc();
    }

    public void Owner_Revive()
    {
        animator.SetBool(DeathString, false);
        animator.SetBool(ExplodString, false);
    }


    [ServerRpc]
    private void SpawnExplotion_ServerRpc()
    {
        SpawnExplotion_ClientRpc();
    }

    [ClientRpc]
    private void SpawnExplotion_ClientRpc()
    {
        Instantiate(explotionVFX);
    }

}
