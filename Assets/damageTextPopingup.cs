using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTextPopingup : MonoBehaviour
{
    [SerializeField] private int maxRotation = 15;
    [SerializeField] private int speed = 5;
    private void Awake()
    {
        int zRotation =  Random.Range(-maxRotation, maxRotation + 1);

        Vector3 angels = transform.eulerAngles;
        angels.z = zRotation;

        transform.eulerAngles = angels;
    }

    void Update()
    {
        transform.position += transform.up * speed;
    }
}
