using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using Data;
using System.Linq;

/// <summary>
/// Creted by <see cref="PrematchPanel"/>
/// </summary>
public class MatchManager : MatchManagerBase
{
    [Header("Custom stuff")]
    [SerializeField] private MatchSettingsHolder settings;




    #region Unity
    private new void Awake()
    {
        base.Awake();

    }

    


    #endregion

    #region Callbacks

    private void Server_onTimesUp()
    {
        print("Time is up");
        timeEnded.Value = true;
        Server_EndMatch();
    }


    #endregion

    protected override void Server_StartTimer()
    {
        timer.Server_StartDownTimer(settings.Duration, Server_onTimesUp);
    }


    [ClientRpc]
    private void ShowKillInfo_ClientRpc(ulong killerId, ulong victimId)
    {
        killInfo_panel.ShowKillInfo(killerId, victimId);
    }

    #region Inherted Methods

    public override void Server_AddScore(ulong id, ushort points = 1)
    {
        PlayerScoreInfo _playerInfo = PlayersHolder.GetPlayerScoreInfo(id);

        _playerInfo.points += points;

        PlayersHolder.Server_ChangePlayerScoreInfo(id, _playerInfo);
        Server_CheckScore();
    }

    public override void Server_AddKill(ulong killerId, ulong victimId)
    {

        PlayerScoreInfo _playerInfo = PlayersHolder.GetPlayerScoreInfo(killerId);

        _playerInfo.kills++;

        PlayersHolder.Server_ChangePlayerScoreInfo(killerId, _playerInfo);
        Server_CheckScore();

        ShowKillInfo_ClientRpc(killerId, victimId);
    }

    #endregion

    #region Utilites
    
    private void Server_CheckScore()
    {
        Dictionary<TeamName, Score> teamScores = new Dictionary<TeamName, Score>();
        List<Score> soloScores = new List<Score>();
        foreach (var info in PlayersHolder.GetCurrentScoreInfos())
        {
            if (info.team == TeamName.Solo)
                soloScores.Add(info.Score);
            else
            {
                if (teamScores.ContainsKey(info.team))
                    teamScores[info.team] += info.Score;
                else
                    teamScores[info.team] = info.Score;
            }
        }

        bool haveWinnerTeam = teamScores.Values.Any(score => settings.HaveReachedMaxScore(score));
        bool haveSoloWinner = soloScores.Any(score => settings.HaveReachedMaxScore(score));

        if (haveWinnerTeam || haveSoloWinner)
            Server_EndMatch();
    }
    #endregion

}
