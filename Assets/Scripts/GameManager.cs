using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MyNetworkManager network;

    [Header("Scriptable objects")]
    [SerializeField] private MatchSettingsHolder settings;
    [SerializeField] private ScenesHolder scenes_tdm;
    [SerializeField] private ScenesHolder scenes_ctf;
    [SerializeField] private ScenesHolder scenes_survival;

    public static Dictionary<Mode, ScenesHolder> Lists { get; private set; } = new Dictionary<Mode, ScenesHolder>();
    public const string MainMenu = "1_MainMenu";
    public const string Prematch = "2_Prematch";
    public static GameManager Instance { get; private set; }

    #region Unity Methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Lists[Mode.TDM] = scenes_tdm;
        Lists[Mode.CTF] = scenes_ctf;
        Lists[Mode.Survival] = scenes_survival;
    }

    private void Start()
    {
        network = MyNetworkManager.singleton;
    }
    #endregion

    public static bool IsMatchScene(string sceneName) => sceneName != MainMenu && sceneName != Prematch;

    public Mode CurrentMode => settings.Mode;

    public Dictionary<string, Robot> Robots { get; } = new Dictionary<string, Robot>();


    [HideInInspector] public Player LocalPlayer { get; set; }

    public TeamName LocalPlayerTeam { get; set; }


    #region Edit Robot List
    public void AddRobot(string _netID, Robot _robot)
    {
        Robots.Add(_netID, _robot);
    }
    public Robot GetRobot(string _robotID)
    {
        return Robots[_robotID];
    }
    public void RemoveRobot(string _robotID)
    {
        Robots.Remove(_robotID);
    }
    #endregion

    public void Server_StartChoosenMatch()
    {
        network.LoadScene(Lists[settings.Mode].levels[settings.MapIndex].name);
    }

    public void Server_ReturnToMatchSelection()
    {
        if (!network.IsServer)
        {
            Debug.LogError("This should only be called on server");
            return;
        }
        try
        {
            MatchManagerBase.current.NetworkObject.Despawn();

        }
        catch (System.Exception e)
        {
            Logger.LogException(e);
        }
        ulong id = network.LocalClientId;
        PlayersHolder.Server_SetPlayerState(id, false);
        network.LoadScene(Prematch);
    }

    public void QuitMatch()
    {
        network.QuitMatch();
    }
}
