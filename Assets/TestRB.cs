using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRB : MonoBehaviour
{
    [SerializeField] private int force;
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private int maxDistance = 80;

    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
        GetComponent<Rigidbody>().AddForce(transform.forward * force, forceMode);
    }


    private void Update()
    {
        if (Vector3.Distance(transform.position, startPosition) > maxDistance)
            Destroy(gameObject);
    }

    public void Setup(int force, ForceMode forceMode, int maxDistance)
    {
        this.force = force;
        this.forceMode = forceMode;
        this.maxDistance = maxDistance;
        
    }
}
