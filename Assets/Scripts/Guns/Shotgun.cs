using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    [Space]
    [Header("Shotgun proprties")]
    public float timeBetweenShots;

    bool ready;

    public override bool TryStartShooting()
    {

        if (Ammo <= 0)
            return false;

        else
        {

            if (ready)
            {
                ready = false;
                Invoke(nameof(Charge), timeBetweenShots);

                Fire();
            }
                return true;

        }

    }

    public override void StopShooting()
    {

    }

    void Charge()
    {
        ready = true;
    }
}