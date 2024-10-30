using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEngine;

public class Damage_Heal_Shower : NetworkBehaviour
{
    [Space]
    [SerializeField] private ParticleSystem healingEffect;

    [Header("Refrences")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Transform[] hp_fills;

    
    private void Awake()
    {
        healthSystem.OnHpChanged += HealthChangedHandler;
        healthSystem.Server_OnDamageReceived += Server_OnDamageReceivedHandler;
    }

    private void Server_OnDamageReceivedHandler()
    {
        ShowHitReceived_ClientRpc();
    }

    [ClientRpc]
    private void ShowHitReceived_ClientRpc()
    {
        if (IsOwner)
            PlayerUIUpdater.Instance.ShowHitIndicator();
    }

    private void HealthChangedHandler(int previosValue, int currentValue)
    {
        int amount = currentValue - previosValue;

        if (amount > 0)
            ShowHealing(amount);
        else if (amount < 0)
            ShowDamage(amount);

        RefreshHp(currentValue);
    }

    private void ShowHealing(int healing)
    {
        if (healingEffect != null)
            healingEffect.Emit(1);
        else
            Debug.LogWarning("Forgot to make healing effect");
    }

    private void ShowDamage(int damage)
    {
        if (IsOwner)
            CameraShaker.Shake(damage);


        // TODO : play a little particles/effect
        // on both sides? (local and remote clients?)
    }

    private void RefreshHp(int currentHP)
    {
        float percentage = (float)currentHP / healthSystem.MaxHP;

        percentage = Mathf.Clamp01(percentage);

        foreach (var fill in hp_fills)
        {
            if (fill != null)
                fill.localScale = new Vector3(percentage, 1, 1);
        }

        if (IsOwner)
            PlayerUIUpdater.Instance.UpdateHp(percentage);

        
    }

}
