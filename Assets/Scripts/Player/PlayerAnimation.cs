using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    [SerializeField] private Animator animator;

    private HealthSystem healthSystem;

    private const string ExplodString = "exploded";
    private const string DeathString = "isDead";

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
    }

    public void Owner_Revive()
    {
        animator.SetBool(DeathString, false);
        animator.SetBool(ExplodString, false);
    }


}
