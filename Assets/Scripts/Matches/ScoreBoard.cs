using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private Transform playersList;
    [SerializeField] private ScrollRectSelector scrollRectSelector;
    [SerializeField] private PlayerScoreSubpanel playerInfoSubpanel_prefab;
    [SerializeField] private MatchSettingsHolder settings;

    private readonly Dictionary<ulong, PlayerScoreSubpanel> panels = new Dictionary<ulong, PlayerScoreSubpanel>();

    private void Awake()
    {
        panels.Clear();

        MatchManagerBase.OnMatchStarted += MatchManagerBase_OnMatchStarted;
    }

    private void MatchManagerBase_OnMatchStarted()
    {
        SetupOlderPlayers(PlayersHolder.GetCurrentScoreInfos());

        PlayersHolder.OnPlayerScoreChanged += OnScoreChanged;
    }

    private void OnDestroy()
    {
        PlayersHolder.OnPlayerScoreChanged -= OnScoreChanged;
    }

    private void AddToTheBoard(PlayerScoreInfo info)
    {
        PlayerScoreSubpanel playerScorePanel = Instantiate(playerInfoSubpanel_prefab, playersList);

        playerScorePanel.Setup(info);

        panels.Add(info.playerId, playerScorePanel);

    }
    
    private void RemoveFromTheBoard(PlayerScoreInfo info)
    {
        Destroy(panels[info.playerId].gameObject);
        panels.Remove(info.playerId);
    }

    private void RefreshInfo(PlayerScoreInfo info)
    {
        panels[info.playerId].ChangeScore(info);
    }

    private void OrganizeBoard()
    {
        // organize by score
        var orderdedPanels = panels.OrderByDescending(pair => pair.Value.ScoreInfo.Score.Current);
        int localPlayerIndex = 0;
        ulong localId = PrematchPlayer.LocalPlayer.OwnerClientId;

        int index = 0;
        foreach (var pair in orderdedPanels)
        {
            ulong playerId = pair.Key;
            panels[playerId].transform.SetSiblingIndex(index);
            if (playerId == localId)
                localPlayerIndex = index;
            index++;
        }

        scrollRectSelector.Select(localPlayerIndex);
    }

    private void Clear()
    {
        foreach (var key in panels.Keys)
        {
            Destroy(panels[key]);
        }

        panels.Clear();
    }

    private void OnScoreChanged(NetworkListEvent<PlayerScoreInfo> changeEvent)
    {
        switch (changeEvent.Type)
        {
            case NetworkListEvent<PlayerScoreInfo>.EventType.Add:
            case NetworkListEvent<PlayerScoreInfo>.EventType.Insert:
                AddToTheBoard(changeEvent.Value);
                break;

            case NetworkListEvent<PlayerScoreInfo>.EventType.Remove:
            case NetworkListEvent<PlayerScoreInfo>.EventType.RemoveAt:
                RemoveFromTheBoard(changeEvent.Value);
                break;

            case NetworkListEvent<PlayerScoreInfo>.EventType.Value:
                RefreshInfo(changeEvent.Value);
                break;

            case NetworkListEvent<PlayerScoreInfo>.EventType.Clear:
                Clear();
                return;
                break;

        }

        OrganizeBoard();
    }

    private void SetupOlderPlayers(NetworkList<PlayerScoreInfo> playerInfos)
    {
        foreach(var player in playerInfos)
        {
            AddToTheBoard(player);
        }
    }
}
