using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using System.Linq;
using HelpingMethods;

public class MatchSettingsPanel : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Dropdown modeDropDown;
    [SerializeField] private Text mapName;
    [SerializeField] private RawImage mapImage;
    [Space]
    [SerializeField] private Button confirm_btn;
    [SerializeField] private Text settingsName_txt;

    /// <summary>
    /// only the server
    /// (for time and kills)
    /// </summary>
    [Header("For server only")]
    [SerializeField] private GameObject Server_settingsPanel;
    [SerializeField] private Text maxTime_txt, maxKills_txt;

    [Header("Info")]
    [SerializeField] private Text changeInfo_txt;
    [SerializeField] private Text choosenMode_txt;
    [SerializeField] private RawImage choosenMap;
    [SerializeField] private Text maxTime_txt2, maxKills_txt2;

    [Header("Other References")]
    [SerializeField] private MatchSettingsHolder settings;
    [SerializeField] private DefaultMatchGoals defaults;


    private int counter_maxTime = 0;
    private int counter_maxScore = 0;

    private Duration SelectedDuration => defaults.MaxTimeDefaults[counter_maxTime];
    private ulong SelectedScore => defaults.MaxScoreDefaults[counter_maxScore];
    private int SelectedMap { get; set; }
    private Mode SelectedMode { get; set; }


    private ulong localClientID;
    private NetworkList<Settings> Suggestions;

    private NetworkVariable<Settings> currentSettings = new NetworkVariable<Settings>();

    #region Unity

    private void Awake()
    {
        Suggestions = new NetworkList<Settings>();
    }

    private void Start()
    {
        Local_Setup();
        Local_SetInfo();

        Local_ShowMap(settings.MapIndex);
    }

    public override void OnNetworkSpawn()
    {
        localClientID = NetworkManager.LocalClientId;


        currentSettings.OnValueChanged += CurrentSettingsChanged_Callback;
        Suggestions.OnListChanged += SuggestionsChanged_Callback;

        if (IsServer)
        {
            settingsName_txt.text = "Settings";
            currentSettings.Value = settings.Current;
        }
        else
        {
            settingsName_txt.text = "Suggest";
            settings.Setup(currentSettings.Value);
        }

        #region Setup counters
        // this will help switching settings for the 1st time
        for (int i = 0; i < defaults.MaxScoreDefaults.Length; i++)
        {
            if (defaults.MaxScoreDefaults[i] >= settings.MaxScore)
            {
                counter_maxScore = i;
                break;
            }
        }
        
        for (int i = 0; i < defaults.MaxTimeDefaults.Length; i++)
        {
            if (defaults.MaxTimeDefaults[i] >= settings.Duration)
            {
                counter_maxTime = i;
                break;
            }
        }

        SelectedMode = settings.Mode;
        SelectedMap = settings.MapIndex;
        #endregion
    }

    #endregion

    private void Client_SuggestSettings()
    {
        Settings _suggestedSettings = new Settings(SelectedMap, SelectedMode, SelectedScore, SelectedDuration);

        SuggestSettings_ServerRpc(localClientID, _suggestedSettings);

    }

    private void Server_Confirm()
    {
        Settings _selectedSettings = new Settings(SelectedMap, SelectedMode, SelectedScore, SelectedDuration);
        currentSettings.Value = _selectedSettings;
    }


    [ServerRpc(RequireOwnership = false)]
    private void SuggestSettings_ServerRpc(ulong clientId, Settings _suggestedSettings)
    {
        int index = (int)clientId;
        if (index >= Suggestions.Count)
            Suggestions.Add(_suggestedSettings);
        else
            Suggestions[index] = _suggestedSettings;

        print($"suggestion called for map {_suggestedSettings.mapIndex} of {_suggestedSettings.mode}");
    }


    #region Local Methods
    private void Local_Setup()
    {
        Local_SetupModeDropDown();

        confirm_btn.onClick.AddListener(Local_ClosePanel);

        if (IsServer)
        {
            Server_settingsPanel.SetActive(true);

            confirm_btn.onClick.AddListener(Server_Confirm);
        }
        else
        {
            Server_settingsPanel.SetActive(false);

            confirm_btn.onClick.AddListener(Client_SuggestSettings);
        }

    }


    private void Local_SetupModeDropDown()
    {
        List<string> modeNames = Enum.GetNames(typeof(Mode)).ToList();
        modeDropDown.ClearOptions();
        modeDropDown.AddOptions(modeNames);
        modeDropDown.onValueChanged.AddListener(Local_ChooseMode);

        modeDropDown.gameObject.SetActive(false);
        print("mode dropdown is disable untill i finish other modes");
    }

    private void Local_ChooseMode(int modeIndex)
    {
        SelectedMode = (Mode)modeIndex;
        Local_ShowMap(0);
    }


    private void Local_ShowMap(int mapIndex)
    {
        var selectedMap = GameManager.Lists[SelectedMode].levels[mapIndex];

        mapImage.texture = selectedMap.texture;
        mapName.text = selectedMap.actualName;
    }

    private void Local_SetInfo()
    {
        changeInfo_txt.text = "Settings has been changed";
        Invoke(nameof(Local_HideText), 3);

        choosenMode_txt.text = settings.Mode.ToString();
        choosenMap.texture = GameManager.Lists[settings.Mode].levels[settings.MapIndex].texture;

        maxTime_txt.text = settings.Duration.ToString();
        maxTime_txt2.text = settings.Duration.ToString();

        maxKills_txt.text = settings.MaxScore.ToString();
        maxKills_txt2.text = settings.MaxScore.ToString();
    }

    private void Local_HideText()
    {
        changeInfo_txt.text = "";
    }

    private void Local_ClosePanel()
    {
        settingsPanel.SetActive(false);
    }
    #endregion

    #region For Buttons

    public void NextMap()
    {
        var length = GameManager.Lists[settings.Mode].levels.Length;
        SelectedMap = Helper.Increase_in_a_Loop(SelectedMap, 0, length - 1);

        Local_ShowMap(SelectedMap);
    }
    
    public void PreviousMap()
    {
        var length = GameManager.Lists[settings.Mode].levels.Length;
        SelectedMap = Helper.Decrease_in_a_Loop(SelectedMap, 0, length - 1);

        Local_ShowMap(SelectedMap);
    }

    public void IncreaseMaxTime()
    {
        counter_maxTime = Helper.Increase_in_a_Loop(counter_maxTime, 0, defaults.MaxTimeDefaults.Length - 1);
        maxTime_txt.text = defaults.MaxTimeDefaults[counter_maxTime].ToString();
    }

    public void DecreaseMaxTime()
    {
        counter_maxTime =  Helper.Decrease_in_a_Loop(counter_maxTime, 0, defaults.MaxTimeDefaults.Length - 1);
        maxTime_txt.text = defaults.MaxTimeDefaults[counter_maxTime].ToString();
    }

    public void IncreaseMaxKills()
    {
        counter_maxScore =  Helper.Increase_in_a_Loop(counter_maxScore, 0, defaults.MaxScoreDefaults.Length - 1);
        maxKills_txt.text = defaults.MaxScoreDefaults[counter_maxScore].ToString();
    }

    public void DecreaseMaxKills()
    {
        counter_maxScore =  Helper.Decrease_in_a_Loop(counter_maxScore, 0, defaults.MaxScoreDefaults.Length - 1);
        maxKills_txt.text = defaults.MaxScoreDefaults[counter_maxScore].ToString();
    }
    #endregion

    #region Callbacks
    private void CurrentSettingsChanged_Callback(Settings oldSettings, Settings newSettings)
    {
        settings.Setup(newSettings);

        
        Local_ClosePanel();
        Local_SetInfo();
    }

    private void SuggestionsChanged_Callback(NetworkListEvent<Settings> changeEvent)
    {
        Logger.LogWarning("suggestion still not Implemented");
    }
    #endregion

}


[Serializable]
public struct Settings : INetworkSerializable, IEquatable<Settings>
{

    public int mapIndex;
    public Mode mode;
    public ulong maxScore;


    [SerializeField] private byte minutes;
    [SerializeField] private byte seconds;

    
    public readonly Duration Duration => new Duration(minutes, seconds);

    public Settings(int _mapIndex = 0, Mode _mode = Mode.TDM, ulong _maxScore = Defaults.MaxScore, Duration _duration = default)
    {
        mapIndex = _mapIndex;
        mode = _mode;
        maxScore = _maxScore;

        minutes = _duration.minutes;
        seconds = _duration.seconds;
    }

    void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
    {
        serializer.SerializeValue(ref mapIndex);
        serializer.SerializeValue(ref mode);
        serializer.SerializeValue(ref maxScore);

        serializer.SerializeValue(ref minutes);
        serializer.SerializeValue(ref seconds);
    }

    bool IEquatable<Settings>.Equals(Settings other)
    {
        return mapIndex == other.mapIndex &&
               mode == other.mode &&
               maxScore == other.maxScore &&
               minutes == other.minutes &&
               seconds == other.seconds;
    }

}

[Serializable]
public struct Duration
{
    public byte minutes;
    public byte seconds;

    public readonly int TotalSecons => minutes * 60 + seconds;

    public Duration(byte minutes, byte seconds)
    {
        this.minutes = minutes;
        this.seconds = seconds;
    }

    public static bool operator >=(Duration a, Duration b)
    {
        return a.TotalSecons >= b.TotalSecons;
    }
    
    public static bool operator <=(Duration a, Duration b)
    {
        return a.TotalSecons <= b.TotalSecons;
    }

    public override string ToString()
    {
        return $"{minutes} : {seconds}";
    }
}