using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [SerializeField] private TestRB testRB;

    [SerializeField] private int force;
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private int maxDistance = 80;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var copy = Instantiate(testRB, Vector3.zero, transform.rotation, transform);
            copy.Setup(force, forceMode, maxDistance);
            copy.gameObject.SetActive(true);
        }
    }
}
