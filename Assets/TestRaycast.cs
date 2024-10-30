using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour
{
    [SerializeField] float radius = 3;


    void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position,radius,transform.forward,out hit))
        {
            Debug.Log(hit.transform.name);
        }

        if (Physics.CheckSphere(transform.position, radius))
            Debug.Log("there is some thing");

    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(transform.position, radius);

    }
}
