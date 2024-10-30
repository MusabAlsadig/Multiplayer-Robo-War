using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PrematchPanel : NetworkBehaviour
{

    [SerializeField] private Transform mainPanel;
    [SerializeField] private Text time_txt;
    [SerializeField] private ScrollRectSelector scrollRectSelector;
    [Header("Prefab")]
    [SerializeField] private PrematchInfo_SubPanel infoPanel_prefab;
    [SerializeField] private UIHolder uiCanvas_prefab;
    [Space]
    [SerializeField] private int startAfter = 5;

    private float timeLeft;
    private bool allReady;
    private bool matchStarted;

    private readonly List<PrematchInfo_SubPanel> prematchInfoPanels = new List<PrematchInfo_SubPanel>();
    private void Awake()
    {
        timeLeft = startAfter;

    }

    private void Start()
    {
        foreach (var playerInfo in PlayersHolder.GetCurrentPlayerInfos())
        {
            CreatePanel(playerInfo, prematchInfoPanels.Count);
        }

        PlayersHolder.OnPlayerInfoChanged += PlayersHolder_OnPlayerInfoChanged_Callback;
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
        PlayersHolder.OnPlayerInfoChanged -= PlayersHolder_OnPlayerInfoChanged_Callback;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ToggleState();



        if (allReady)
        {
            time_txt.enabled = true;

            if (timeLeft > 0)
                time_txt.text = Mathf.CeilToInt(timeLeft).ToString();
            else if (timeLeft < -2)
                time_txt.text = "host is a bit slow...";

            timeLeft -= Time.deltaTime;
        }
        else
        {
            time_txt.enabled = false;
            timeLeft = startAfter;
        }

        if (timeLeft <= 0 && !matchStarted)
            StartMatch();
    }

    private void PlayersHolder_OnPlayerInfoChanged_Callback(NetworkListEvent<PlayerInfo> changeEvent)
    {
        int panelIndex = changeEvent.Index;
        switch (changeEvent.Type)
        {
            case NetworkListEvent<PlayerInfo>.EventType.Add:
            case NetworkListEvent<PlayerInfo>.EventType.Insert:
                CreatePanel(changeEvent.Value, panelIndex);
                break;

            case NetworkListEvent<PlayerInfo>.EventType.Remove:
            case NetworkListEvent<PlayerInfo>.EventType.RemoveAt:
                DeletePanel(panelIndex);
                break;

            case NetworkListEvent<PlayerInfo>.EventType.Value:
                EditPanel(changeEvent.Value, panelIndex);
                break;

            case NetworkListEvent<PlayerInfo>.EventType.Clear:
                Clear();
                break;
        }
    }

    private void CreatePanel(PlayerInfo playerInfo, int index)
    {
        PrematchInfo_SubPanel infoPanel = Instantiate(infoPanel_prefab, mainPanel);
        infoPanel.Setup(playerInfo, IsServer);
        prematchInfoPanels.Insert(index, infoPanel);

        infoPanel.transform.SetSiblingIndex(index);

        if (playerInfo.id == NetworkManager.LocalClientId)
            scrollRectSelector.Select(index);

        if (!playerInfo.isReady)
            allReady = false;
    }
    private void DeletePanel(int index)
    {
        print($"deleting at {index}");
        Destroy(prematchInfoPanels[index].gameObject);
        prematchInfoPanels.RemoveAt(index);
    }

    private void EditPanel(PlayerInfo playerInfo, int index)
    {
        print("Edited");
        prematchInfoPanels[index].Refresh(playerInfo);

        CheckPlayers();
    }

    private void CheckPlayers()
    {
        allReady = false;

        foreach (var playerInfo in PlayersHolder.GetCurrentPlayerInfos())
        {
            if (!playerInfo.isReady)
                return;
        }

        allReady = true;
    }


    private void Clear()
    {
        foreach (var panel in prematchInfoPanels)
        {
            Destroy(panel.gameObject);
        }

        prematchInfoPanels.Clear();
    }

    

    #region For Buttons
    public void Ready()
    {
        ToggleState();
    }
    #endregion


    private void ToggleState()
    {
        ulong id = NetworkManager.LocalClientId;
        bool wasReady = PlayersHolder.GetPlayerInfo(id).isReady;

        SetState_ServerRpc(id, !wasReady);
    }

    private void StartMatch()
    {
        matchStarted = true;


        if (IsServer)
        {
            Server_CreateMatchManager();
            GameManager.Instance.Server_StartChoosenMatch();
        }
    }

    private void Server_CreateMatchManager()
    {
        NetworkObject uiCanvas = Instantiate(uiCanvas_prefab).GetComponent<NetworkObject>();
        uiCanvas.Spawn();
    }

    #region Info Sync
    [ServerRpc(RequireOwnership = false)]
    private void SetState_ServerRpc(ulong id, bool isReady)
    {
        PlayerInfo info = PlayersHolder.GetPlayerInfo(id);
        info.isReady = isReady;
        PlayersHolder.Server_EditPlayer(id, info);
    }
    #endregion


    /* private void Refresh()
    {
        allReady = true;

        var allPlayers = PlayersHolder.GetCurrentPlayerInfos();

        if (allPlayers.Count < 1)
            allReady = false;

        for (int i = 0; i < allPlayers.Count; i++)
        {
            PlayerInfo currentPlayer = allPlayers[i];

            if (!currentPlayer.isReady)
                allReady = false;


            if (i >= prematchInfoPanels.Count)
            {
                PrematchInfo_SubPanel infoPanel = Instantiate(infoPanel_prefab, mainPanel);
                infoPanel.Setup(currentPlayer, IsServer);

                prematchInfoPanels.Add(infoPanel);
            }
            else
                prematchInfoPanels[i].Refresh(currentPlayer);

        }


    }  */
}
