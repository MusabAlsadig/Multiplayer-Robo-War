using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    public override bool TryStartShooting()
    {

        if (Ammo <= 0)
            return false;

        else
        {
            Fire();
            return true;
        }

    }

    public override void StopShooting() { }
}
