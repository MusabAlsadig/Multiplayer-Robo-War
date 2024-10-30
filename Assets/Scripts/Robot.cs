using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(HealthSystem))]
public class Robot : MonoBehaviour
{
    public HealthSystem healthSystem;
    private void Start()
    {
        transform.name = "Robot " + GameManager.Instance.Robots.Count;
        GameManager.Instance.AddRobot(name, this);

        healthSystem.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        healthSystem.OnDeath -= OnDeath;
    }



    protected void OnDeath()
    {
        GameManager.Instance.RemoveRobot(name);
    }
}
