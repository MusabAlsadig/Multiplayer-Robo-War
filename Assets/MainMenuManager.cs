using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject lanPanel;
    [SerializeField] private GameObject onlinePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] private Button lanButton;
    [SerializeField] private Button onlineButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private List<Button> crossButtons = new List<Button>();

    private const Type defaultPanelType = Type.MainMenu;

    private readonly Dictionary<Type, GameObject> panels = new Dictionary<Type, GameObject>();


    private void Awake()
    {
        panels[Type.MainMenu] = mainMenuPanel;
        panels[Type.Lan] = lanPanel;
        panels[Type.Online] = onlinePanel;
        panels[Type.Settings] = settingsPanel;

        lanButton.onClick.AddListener(() => OpenPanel(Type.Lan));
        onlineButton.onClick.AddListener(() => OpenPanel(Type.Online));
        settingsButton.onClick.AddListener(() => OpenPanel(Type.Settings));
        crossButtons.ForEach(button => button.onClick.AddListener(ReturnToMain));
    }

    private void Start()
    {
        if (Data.GameData.Current.playerInfo.HaveName)
            ReturnToMain();

        print("opening main menu by default is disabled if player have no data");
    }

    private enum Type { MainMenu, Lan, Online, Settings }

    private void OpenDefault()
    {
        panels[defaultPanelType].SetActive(true);
    }
    
    private void OpenPanel(Type type)
    {
        CloseAll();
        panels[type].SetActive(true);
    }

    private void CloseAll()
    {
        foreach (var _panel in panels.Values)
        {
            if (_panel != null)
                _panel.SetActive(false);
        }
    }

    private void ReturnToMain()
    {
        CloseAll();
        OpenDefault();
    }

}
