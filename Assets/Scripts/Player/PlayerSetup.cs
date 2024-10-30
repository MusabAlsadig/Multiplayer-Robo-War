using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Player))]

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField] private Behaviour[] componotToDisable;

    [Header("Player Info")]
    [SerializeField] private PlayerInfoManager playerInfoManager;

    [Space]
    [SerializeField] private MeshRenderer eye;

    [Space]
    [SerializeField] private GameObject modelHead;
    [SerializeField] private GameObject modelBody;

    [Header("UI")]
    [SerializeField] private GameObject canvas;

    public string ally, enemy;

    [Space]
    [SerializeField] private string remoteLayerName;
    [SerializeField] private string hiddenLayerName;

    private void Awake()
    {
        MatchManagerBase.OnMatchStarted += MatchManagerBase_OnMatchStarted;

        
        
        eye.material = new Material(eye.material);
    }

    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            modelHead.layer = LayerMask.NameToLayer(hiddenLayerName);
            modelBody.layer = LayerMask.NameToLayer(hiddenLayerName);
            eye.gameObject.layer = LayerMask.NameToLayer(hiddenLayerName);
        }
        else
        {
            DisableComponents();
            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        }
    }

    private void MatchManagerBase_OnMatchStarted()
    {
        if (!IsOwner)
        {
            PlayerInfo info = PlayersHolder.GetPlayerInfo(OwnerClientId);
            FriendOrEnemy(info.name.ToString(), info.team);
        }

    }



    private void DisableComponents()
    {
        foreach (Behaviour b in componotToDisable)
        {
            b.enabled = false;
        }
        Destroy(canvas);
        
    }

    

    
    private void FriendOrEnemy(string playerName, TeamName myTeam)
    {
        AllyOrEnemy a;
        if (myTeam == TeamName.Solo || myTeam != GameManager.Instance.LocalPlayerTeam)
        {
            a = AllyOrEnemy.Enemy;
            tag = enemy;
        }
        else
        {
            a = AllyOrEnemy.Ally;
            tag = ally;
        }
        playerInfoManager.SetUp(playerName, a);
    }

}

