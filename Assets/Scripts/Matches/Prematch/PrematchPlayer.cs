using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Linq;
using UnityEngine.Rendering;

public class PrematchPlayer : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;


    private NetworkObject player;

    public static PrematchPlayer LocalPlayer { get; private set; }

    public override void OnNetworkSpawn()
    {
        
        name += " " + OwnerClientId;

        

        

        if (!IsOwner)
            return;
        GameManager.Instance.LocalPlayerTeam = TeamName.Solo;

        


        NetworkManager.SceneManager.OnLoad += SceneManager_OnLoad;

        string playerName = Data.GameData.Current.playerInfo.name;

        // all none server will join ready
        AddPlayerInfo_ServerRpc(playerName, !IsServer);

        LocalPlayer = this;


        #region For players who join mid match
        string currentScene = SceneChanger.GetActiveSceneName();
        
        SceneManager_OnLoad(OwnerClientId, currentScene);
        #endregion
    }

    [ServerRpc]
    private void AddPlayerInfo_ServerRpc(string playerName, bool isReady)
    {
        PlayersHolder.Server_AddPlayer(OwnerClientId, playerName, isReady);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
            PlayersHolder.Server_RemovePlayer(OwnerClientId);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }


    #region Public Methods
    
    public void Owner_ChooseTeam(TeamName team)
    {
        GameManager.Instance.LocalPlayerTeam = team;

        SetTeam_ServerRpc(team);
    }

    [ServerRpc]
    private void SetTeam_ServerRpc(TeamName team)
    {
        ulong id = OwnerClientId;
        PlayerInfo info = PlayersHolder.GetPlayerInfo(id);
        info.team = team;
        PlayersHolder.Server_EditPlayer(id, info);
    }
    #endregion

    /// <summary>
    /// this method happen before loading
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="sceneName"></param>
    /// <param name="loadSceneMode"></param>
    /// <param name="asyncOperation"></param>
    private void SceneManager_OnLoad(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single, AsyncOperation asyncOperation = null)
    {
        if (!IsOwner)
            return;


        if (GameManager.IsMatchScene(sceneName))
        {
            CreatePlayer_ServerRpc();
        }
        else
            RemovePlayer_ServerRpc();
    }

    [ServerRpc]
    private void CreatePlayer_ServerRpc()
    {
        if (player != null)
        {
            Debug.LogError("Trying to re-create player");
            return;
        }

        

        player = Instantiate(playerPrefab);
        

        player.SpawnAsPlayerObject(OwnerClientId, true);
        player.TrySetParent(gameObject);

    }

    [ServerRpc]
    private void RemovePlayer_ServerRpc()
    {
        if (player == null)
            return;

        player.Despawn();
        print("Player despawned");
    }

}
