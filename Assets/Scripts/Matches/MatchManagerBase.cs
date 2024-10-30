using HelpingMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public abstract class MatchManagerBase : NetworkBehaviour
{
    public static MatchManagerBase current;


    [Header("UI")]
    [SerializeField] protected EndGamePanel endgame_panel;
    [SerializeField] protected KillInfoPanel killInfo_panel;
    [SerializeField] protected ScoreBoard scoreBoard;
    [SerializeField] protected Timer timer;

    private bool server_sceneLoaded;

    protected NetworkVariable<bool> finished = new NetworkVariable<bool>(false);
    protected NetworkVariable<bool> timeEnded = new NetworkVariable<bool>(false);


    private static Action _onMatchStarted;
    private static Action _onMatchEnded;

    public static event Action OnMatchEnded
    {
        add => _onMatchEnded += value;
        remove => _onMatchEnded -= value;
    }

    public static event Action OnMatchStarted 
    { 
        add => _onMatchStarted += value; 
        remove => _onMatchStarted -= value;
    }

    protected void Awake()
    {
        if (current != null)
        {
            Debug.LogWarning("MatchManager have been changed into \n" + GetType());
        }

        current = this;

        Debug.Log("Match Manager been created", this);

        ActionCaller.CallAfterframes( Server_ToggleSceneLoadedTest, 4); // 4 frames to insure all stuff have been setup
    }

    public override void OnNetworkSpawn()
    {
        if (finished.Value == true)
            Local_EndMatch(timeEnded.Value);
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (PlayersHolder.HaveAllPlayersJoined() && server_sceneLoaded)
            Server_StartMatch();
    }

    private void Server_ToggleSceneLoadedTest()
    {
        server_sceneLoaded = true;
    }

    private void Server_StartMatch()
    {
        if (!timer.Started)
            Server_StartTimer();

        MatchStartedHandler_ClientRpc();
    }

    [ClientRpc]
    private void MatchStartedHandler_ClientRpc()
    {
        _onMatchStarted?.Invoke();
        _onMatchStarted -= _onMatchStarted;
    }


    protected void Server_EndMatch()
    {
        finished.Value = true;
        timer.Server_Freeze();
        EndMatch_ClientRpc(timer.Ended);
    }

    [ClientRpc]
    private void EndMatch_ClientRpc(bool _timeEnded)
    {
        Local_EndMatch(_timeEnded);
        _onMatchEnded?.Invoke();
        _onMatchEnded -= _onMatchEnded;

        MouseHidder.ShowMouse();
    }

    private void Local_EndMatch(bool _timeEnded)
    {
        endgame_panel.Show(_timeEnded);
        print("Match finished");
    }

    protected abstract void Server_StartTimer();

    public abstract void Server_AddScore(ulong id, ushort score = 1);

    public abstract void Server_AddKill(ulong killerId, ulong victimId);

}
