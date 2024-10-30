using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;
using UI;
using UnityEditor.Experimental;
using System.Collections.Generic;

[RequireComponent(typeof(HealthSystem))]
public class Player : NetworkBehaviour
{
    public Transform flagHolder;

    public static List<Player> AllPlayers = new List<Player>();

    [Header("Components")]
    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    #region Unity
    private void Awake()
    {
        healthSystem.OnDeath += OnDeath;
        healthSystem.OnRevive += OnRevive;

        MatchManagerBase.OnMatchStarted += MatchManagerBase_OnMatchStarted;
        MatchManagerBase.OnMatchEnded += MatchManagerBase_OnMatchEnded;

        AllPlayers.Add(this);
    }


    public override void OnDestroy()
    {
        base.OnDestroy();

        healthSystem.OnDeath -= OnDeath;
        healthSystem.OnRevive -= OnRevive;

        MatchManagerBase.OnMatchStarted -= MatchManagerBase_OnMatchStarted;

        AllPlayers.Remove(this);
    }

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            Owner_Setup();

            GameManager.Instance.LocalPlayer = this;
            
        }

        if (IsServer)
            PlayersHolder.Server_AddScoreInfo(OwnerClientId);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
            PlayersHolder.Server_RemoveScoreInfo(OwnerClientId);
    }

    #endregion


    private void MatchManagerBase_OnMatchStarted()
    {
        if (IsOwner)
        {
            RespawnPoint.ChangePoseFor(this, GameManager.Instance.LocalPlayerTeam);
            playerMotor.Refresh();

            PlayerInput.EnableInput();
        }
    }
    private void MatchManagerBase_OnMatchEnded()
    {
        if (IsOwner)
        {
            PlayerInput.DisableInput();
            DisableComponents();
            PlayerUIUpdater.Instance.StopRespawningTimer();
        }
    }


    private void Owner_Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];

        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
    }

    #region Callbacks
    private void OnDeath()
    {
        GetComponent<Rigidbody>().useGravity = true;

        if (!IsOwner)
            return;

        PlayerUIUpdater.Instance.StartRespawningTimer();

        if (flagHolder != null)
            flagHolder.GetComponentInChildren<Flag>().Server_Detach();


        DisableComponents();
        

    }

    private void DisableComponents()
    {
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            if (disableOnDeath[i] == null)
            {
                Logger.LogWarning($" <color=yellow>{nameof(disableOnDeath)}[{i}]</color> on <color=yellow>{name}</color> shouldn't be null");
                continue;
            }
            disableOnDeath[i].enabled = false;
        }
    }

    private void OnRevive()
    {
        GetComponent<Rigidbody>().useGravity = false;
        if (!IsOwner)
            return;

        RespawnPoint.ChangePoseFor(this, GameManager.Instance.LocalPlayerTeam);
        Owner_EnableComponents();
        PlayerUIUpdater.Instance.StopRespawningTimer();
    }

    #endregion

    private void Owner_EnableComponents()
    {
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            if (disableOnDeath[i] == null)
            {
                Debug.LogWarning("there is a null component", this);
                continue;
            }


            disableOnDeath[i].enabled = wasEnabled[i];
        }
    }

    

}