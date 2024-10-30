using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Smg : GunBase
{
    [Space]
    [Header("Smg proprties")]
    public float fireRate;

    private bool isFiring;

    float time;

    private void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;

        if (!isFiring)
            return;

        if (time <= 0)
        {
            time = 1 / fireRate;
            Fire();
        }


    }

    public override bool TryStartShooting()
    {

        if (Ammo <= 0)
            return false;

        else 
        {
            isFiring = true;
            return true;
        }


    }

    public override void StopShooting()
    {
        isFiring = false;
    }



}
