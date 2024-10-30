using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// a class that hold (and sync) infos of all players in-match,
/// </summary>
public class PlayersHolder : NetworkBehaviour
{
    private static PlayersHolder instance;

    public static PlayerInfo LocalPlayerInfo => GetPlayerInfo(MyNetworkManager.singleton.LocalClientId);


    private NetworkList<PlayerInfo> _infos;
    private NetworkList<PlayerScoreInfo> _scoreInfos;

    private static NetworkList<PlayerInfo> Infos => instance._infos;
    private static NetworkList<PlayerScoreInfo> ScoreInfos => instance._scoreInfos;

    private static List<PlayerInfo> Server_TempInfos { get; } = new List<PlayerInfo>();

    public static Action<NetworkListEvent<PlayerScoreInfo>> OnPlayerScoreChanged { get; set; }

    public static event Action<NetworkListEvent<PlayerInfo>> OnPlayerInfoChanged;


    private void Awake()
    {
        if (instance == null)
            Setup();
    }

    private void Setup()
    {
        instance = this;

        _infos = new NetworkList<PlayerInfo>();
        _scoreInfos = new NetworkList<PlayerScoreInfo>();

        _infos.OnListChanged += PlayerInfoChange_Callback;
        _scoreInfos.OnListChanged += ScoreInfoChange_Callback;
        
    }

    public override void OnNetworkSpawn()
    {
        // sync all those players who spawned too early
        foreach (var info in Server_TempInfos)
        {
            _infos.Add(info);
        }

        Server_TempInfos.Clear();

    }

    #region CallBacks
    private void ScoreInfoChange_Callback(NetworkListEvent<PlayerScoreInfo> changeEvent)
    {
        Debug.Log("ScoreInfo changed "+ changeEvent.Type);
        OnPlayerScoreChanged?.Invoke(changeEvent);
    }
    
    private void PlayerInfoChange_Callback(NetworkListEvent<PlayerInfo> changeEvent)
    {
        OnPlayerInfoChanged?.Invoke(changeEvent);
    }

    #endregion

    public static bool AreAllies(ulong id1,  ulong id2)
    {
        PlayerInfo player1 = GetPlayerInfo(id1);
        PlayerInfo player2 = GetPlayerInfo(id2);

        return player1.team == player2.team && 
            player1.team != TeamName.Solo && player2.team != TeamName.Solo;
    }

    /// <summary>
    /// <b>Remember</b> call this only Localy
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public static bool IsMyAlly(ulong playerId)
    {
        TeamName PlayerTeam = GetPlayerInfo(playerId).team;
        TeamName myTeam = GameManager.Instance.LocalPlayerTeam;

        if (playerId == instance.NetworkManager.LocalClientId) // additional check
            Debug.LogError("can you be your own ally?, PlayersHolder");


        return PlayerTeam == myTeam && myTeam != TeamName.Solo;
    }


    public static bool HaveAllPlayersJoined()
    {
        return Infos.Count == ScoreInfos.Count;
    }

    #region Setters
    #region Player Infos
    public static void Server_AddPlayer(ulong id, FixedString32Bytes name, bool IsReady = true, TeamName team = TeamName.Solo)
    {
        PlayerInfo playerInfo = new PlayerInfo(id, name, team, IsReady);
        if (instance == null)
            Server_TempInfos.Add(playerInfo);
        else
            Infos.Add(playerInfo);
    }

    public static void Server_EditPlayer(ulong id, PlayerInfo newInfo)
    {
        var temp = PlayerInfo.IdChecker(id);
        int index = Helper.GetIndex(temp, Infos);
        Infos[index] = newInfo;

        Debug.LogWarning("Check for data traffic here, changing name may do a number to network");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state">isReady or not, will just toggle if null</param>
    public static void Server_SetPlayerState(ulong id, bool? state)
    {
        PlayerInfo info = GetPlayerInfo(id);

        if (state != null)
            info.isReady = state.Value;
        else
            info.isReady = !info.isReady;
             
        var temp = PlayerInfo.IdChecker(id);
        int index = Helper.GetIndex(temp, Infos);
        Infos[index] = info;
    }
    
    public static void Server_RemovePlayer(ulong id)
    {
        if (MyNetworkManager.singleton.ShutdownInProgress)
            return;

        int index = Infos.IndexOf(PlayerInfo.IdChecker(id));
        Infos.RemoveAt(index);
        
    }
    #endregion

    #region Score Infos
    public static void Server_AddScoreInfo(ulong id)
    {
        TeamName team = GetPlayerInfo(id).team;
        ScoreInfos.Add(new PlayerScoreInfo(id, team));
    }
    public static void Server_ChangePlayerScoreInfo(ulong id, PlayerScoreInfo scoreInfo)
    {
        var temp = PlayerScoreInfo.IdChecker(id);
        int index = Helper.GetIndex(temp, ScoreInfos);
        ScoreInfos[index] = scoreInfo;
    }
    
    public static void Server_RemoveScoreInfo(ulong id)
    {
        if (instance.NetworkManager.ShutdownInProgress)
            return;

        ScoreInfos.Remove(PlayerScoreInfo.IdChecker(id));
    }
    #endregion

    #endregion

    #region Getters
    public static PlayerInfo GetPlayerInfo(ulong id)
    {
        var temp = PlayerInfo.IdChecker(id);

        int index = Helper.GetIndex(temp, Infos);

        return Infos[index];
    }
    
    public static PlayerScoreInfo GetPlayerScoreInfo(ulong id)
    {
        var temp = PlayerScoreInfo.IdChecker(id);

        int index = Helper.GetIndex(temp, ScoreInfos);

        return ScoreInfos[index];
    }

    public static NetworkList<PlayerScoreInfo> GetCurrentScoreInfos()
    {
        return ScoreInfos;
    }
    
    public static NetworkList<PlayerInfo> GetCurrentPlayerInfos()
    {
        return Infos;
    }
    #endregion

}

public struct PlayerInfo : INetworkSerializable, IEquatable<PlayerInfo>
{
    public ulong id;
    public FixedString32Bytes name;
    public TeamName team;
    public bool isReady;

    public PlayerInfo(ulong id, FixedString32Bytes name, TeamName team, bool isReady)
    {
        this.id = id;
        this.name = name;
        this.team = team;
        this.isReady = isReady;
    }

    public static PlayerInfo IdChecker(ulong _id) 
    {
        return new PlayerInfo() { id = _id };
    }

    void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref name);
        serializer.SerializeValue(ref team);

        serializer.SerializeValue(ref isReady);
    }

    readonly bool IEquatable<PlayerInfo>.Equals(PlayerInfo other)
    {
        return id == other.id;
    }

}
