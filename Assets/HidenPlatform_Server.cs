using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HidenPlatform_Server : NetworkBehaviour
{
    private const int MaxLength = 8;
    private int length;

    [TextArea] private Vector3 targetPosition;
    Vector3 direction;

    private float countdown = Defaults.PlatformStayDuration;

    private bool isReturning;



    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        length = UnityEngine.Random.Range(1, MaxLength + 1);
        SetDirection();
        targetPosition = transform.position + direction * length;
        
    }

    private void Update()
    {
        if (Vector3.Distance(targetPosition, transform.position) > 0.1)
        {
            PreformMovement();
        }
        else
        {
            transform.position = targetPosition;
            countdown -= Time.deltaTime;
        }


        if (countdown <= 0)
        {
            Reverse();
        }
    }

    private void PreformMovement()
    {
        int speed = Defaults.PlatformSpeed;


        transform.position += direction * speed * Time.deltaTime;

    }

    private void Reverse()
    {
        if (isReturning)
        {
            NetworkObject.Despawn();
            return;
        }
        countdown = 1;

        direction *= -1;
        targetPosition = transform.position + direction * length;
        isReturning = true;
    }

    #region Utilities
    private void SetDirection()
    {
        Vector3 position = transform.position;

        float x = Math.Abs(position.x);
        float z = Math.Abs(position.z);

        if (x > z)
            direction = new Vector3(1, 0, 0) * Math.Sign(x) * -1;
        else
            direction = new Vector3(0, 0, 1) * Math.Sign(z) * -1;

        

    }
    #endregion

}
