using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    
    private void Update()
    {
        UpdateRotation();
    }
    private void UpdateRotation()
    {
        Vector3 euler = transform.eulerAngles;

        euler += new Vector3(0, 0, speed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(euler);
        transform.rotation = rotation;
    }
}
