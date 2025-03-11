using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UI;

public class HealthSystem : NetworkBehaviour
{

    [SerializeField] private short max_hp = 100;
    [SerializeField] private int boomLimit = -60;
    [SerializeField] private bool canRevive = true;
    


    [Header("References"), Tooltip ("Optional")]
    public Transform textSpawningPlace;
    [SerializeField] private new Collider collider;




    private readonly NetworkVariable<short> current_hp = new NetworkVariable<short>();
    private readonly NetworkVariable<bool> _isDead = new NetworkVariable<bool>();


    /// <summary>
    /// to solve the little lag on response, so a player won't die 3 time instead of 1 with high fire rate
    /// </summary>
    private bool server_isDead = false;
    private bool server_exploded = false;
    private bool server_isInvincible = true;

    public Action Server_OnDamageReceived { get; set; }
    public Action OnDeath { get; set; }
    public Action OnExplod { get; set; }
    public Action OnRevive { get; set; }
    public Action<int, int> OnHpChanged { get; set; }

    public bool IsDead
    {
        get { return _isDead.Value; }
        private set 
        { 
            _isDead.Value = value;
            server_isDead = value;
        }
    }

    public int CurrentHP => current_hp.Value;
    public int MaxHP => max_hp;

    public static List<HealthSystem> allHealthSystems = new List<HealthSystem>();

    #region Unity
    private void Awake()
    {

        _isDead.OnValueChanged += IsDead_onValueChanged_Callback;

        MatchManagerBase.OnMatchStarted += MatchManagerBase_OnMatchStarted;
        MatchManagerBase.OnMatchEnded += MatchManagerBase_OnMatchEnded;

        allHealthSystems.Add(this);
    }

    public override void OnDestroy()
    {
        MatchManagerBase.OnMatchStarted -= MatchManagerBase_OnMatchStarted;
        MatchManagerBase.OnMatchEnded -= MatchManagerBase_OnMatchEnded;

        allHealthSystems.Remove(this);
    }

    


    private void Update()
    {
        if (!IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.R))
            Revive_ServerRpc();

    }

    #endregion

    public override void OnNetworkSpawn()
    {
        current_hp.OnValueChanged += HpChangedHandler;


    }

    private void MatchManagerBase_OnMatchStarted()
    {
        if (IsServer)
        {
            server_isInvincible = false;
            Server_ResetHP();
        }
    }
    private void MatchManagerBase_OnMatchEnded()
    {
        if (IsServer)
        {
            server_isInvincible = true;

            CancelInvoke(nameof(Server_Revive));
        }
    }
    

    #region Public Methods

    public void Server_TakeDamage(int damage, ulong? from = null)
    {
        if (server_isInvincible)
            return;

        current_hp.Value -= (short)damage;
        Server_OnDamageReceived?.Invoke();

        if (current_hp.Value <= boomLimit && !server_exploded)
        {
            server_exploded = true;
            Explod_ClientRpc();
        }
        else if (current_hp.Value <= 0 && !server_isDead)
        {
            if (from != null)
                MatchManagerBase.current.Server_AddKill(from.Value, OwnerClientId);
            else
                Debug.LogWarning("This player died from other source");

            Invoke(nameof(Server_Revive), Defaults.ReviveTime);
            IsDead = true;
        }

    }

    public void Server_Heal(short healPoints)
    {
        current_hp.Value = (short)Mathf.Clamp(current_hp.Value + healPoints, 0, max_hp);
    }
    #endregion


    private void IsDead_onValueChanged_Callback(bool oldValue, bool died)
    {

        if (died)
        {
            OnDeath?.Invoke();
            Debug.Log(name + " have died");
        }
        else
        {
            OnRevive?.Invoke();
            collider.enabled = true;
            Debug.Log(name + " Revived");
        }

    }



    [ServerRpc]
    private void Revive_ServerRpc()
    {
        Server_Revive();
    }

    private void Server_Revive()
    {
        CancelInvoke(nameof(Server_Revive));

        if (!canRevive)
        {
            NetworkObject.Despawn();
            return;
        }

        IsDead = false;
        server_exploded = false;


        Server_ResetHP();

    }

    #region Utilities


    private void Server_ResetHP()
    {
        current_hp.Value = max_hp;
    }

    protected void HpChangedHandler(short previosValue, short currentValue)
    {
        OnHpChanged?.Invoke(previosValue, currentValue);   
    }

    [ClientRpc]
    private void Explod_ClientRpc()
    {
        OnExplod?.Invoke();
        collider.enabled = false;
    }

    #endregion


}
