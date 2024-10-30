using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharginGun : GunBase
{

    [Space]
    [Header("Charging Gun proprties")]
    [SerializeField] private int maxCharge;
    [SerializeField] private float chargingRate;

    private int currentCharge;
    public override bool TryStartShooting()
    {
        if (Ammo <= 0)
            return false;

        if (!IsInvoking(nameof(PreformCharging)))
        {
            StartCharging();
            return true;
        }
        else
        {
            Debug.LogError("This Should not happen");
            return false;
        }


    }

    public override void StopShooting()
    {
        CancelInvoke(nameof(PreformCharging));
        Fire(needAmmo: false, damageMultiplier: currentCharge);
    }

    private void PreformCharging()
    {
        if (currentCharge == maxCharge)
            return;

        if (TryDecreaseAmmo())
            currentCharge++;

    }

    private void StartCharging()
    {
        Debug.Log("Charging started");

        InvokeRepeating(nameof(PreformCharging), chargingRate, chargingRate);
    }
}
