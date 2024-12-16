using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    private int degrees = 2;

    [SerializeField]
    private int speed = 100;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Rigidbody rb;

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        var rotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, degrees * Time.deltaTime));
    }
}
