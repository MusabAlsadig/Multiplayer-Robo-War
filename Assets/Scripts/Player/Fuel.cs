using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UI;

public class Fuel : NetworkBehaviour
{
    
    [SerializeField] float maxFuel = 100;
    [SerializeField] float regenRate = 15f;
    [SerializeField] float LoseRate = 25f;
    [SerializeField] float useLimit = 1;

    [Space]
    [SerializeField] int flyingDistance = 1;

    [Space]
    [SerializeField] float thrusterForce = 2000;

    private float _currentFuel;
    private float CurrentFuel
    {
        get => _currentFuel;
        set
        {
            _currentFuel = value;
            PlayerUIUpdater.Instance.UpdateEnergy(value / maxFuel);
        }
    }

    bool isOn = false;

    Rigidbody rb;
    ConfigurableJoint cj;
    JointDrive y;
    float positionSpring;

    [Space]
    [SerializeField]
    private ParticleSystem[] burners;

    

    private void Start()
    {
        CurrentFuel = maxFuel;
        rb = GetComponent<Rigidbody>();
        cj = GetComponent<ConfigurableJoint>();
        y = cj.yDrive;
        positionSpring = y.positionSpring;

        SetThrosterServerRpc(false);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) 
            return;

        EventManager.AssignOnClick(EventManager.Controls.Thruster, StartThruster);
        EventManager.AssignOnCancle(EventManager.Controls.Thruster, StopThruster);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
            return;

        EventManager.RemoveOnClick(EventManager.Controls.Thruster, StartThruster);
        EventManager.RemoveOnCancle(EventManager.Controls.Thruster, StopThruster);
    }

    private void Update()
    {
        RefreshGrownd();

        if (isOn)
        {
            // consume fuel
            CurrentFuel -= LoseRate * Time.deltaTime;
        }
        else if (CurrentFuel < maxFuel && !isOn)
        {
            // regen fuel
            CurrentFuel += regenRate * Time.deltaTime;
            CurrentFuel = Mathf.Clamp(CurrentFuel, 0, maxFuel);
        }

        if(CurrentFuel <= useLimit && isOn)
        {
            StopThruster();
        }
    }

    private void FixedUpdate()
    {
        if (isOn)
        {
            rb.AddForce(new Vector3(0, thrusterForce * Time.fixedDeltaTime, 0),ForceMode.Acceleration);
        }

    }

    private void StartThruster()
    {
        // start moveing up
        y.positionSpring = 0;
        cj.yDrive = y;

        SetThrosterServerRpc(true);
        isOn = true;
    }

    private void StopThruster()
    {
        // start landing
        y.positionSpring = positionSpring;
        cj.yDrive = y;

        SetThrosterServerRpc(false);
        isOn = false;
    }


    void RefreshGrownd()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position,-transform.up,out hit))
        {
            cj.connectedAnchor = new Vector3(0, hit.point.y + flyingDistance, 0);
        }

    }

    [ServerRpc]
    public void SetThrosterServerRpc(bool value)
    {
        SetThrosterClientRpc(value);
    }

    [ClientRpc]
    public void SetThrosterClientRpc(bool value)
    {
        if (value)
        {
            foreach (var particleSystem in burners)
            {
                particleSystem.Play(true);
            }
        }
        else
        {
            foreach (var particleSystem in burners)
            {
                particleSystem.Stop(true);
            }
        }
    }
}
