using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private List<HealthSystem> objectsInRange = new List<HealthSystem>();


    private const string Open_string = "open";
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " Entered");
        if (!other.TryGetComponent(out HealthSystem healthSystem))
            return;

        if (objectsInRange.Count == 0)
            animator.SetBool(Open_string, true);

        objectsInRange.Add(healthSystem);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + " Exited");
        if (!other.TryGetComponent(out HealthSystem healthSystem))
            return;

        objectsInRange.Remove(healthSystem);

        if (objectsInRange.Count == 0)
            animator.SetBool(Open_string, false);
    }
}
