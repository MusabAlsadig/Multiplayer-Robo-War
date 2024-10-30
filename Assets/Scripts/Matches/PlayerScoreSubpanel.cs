using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// created and destroyed by <see cref="ScoreBoard"/>
/// </summary>
public class PlayerScoreSubpanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name_txt;
    [SerializeField] private TextMeshProUGUI score_txt;
    [SerializeField] private Image background;
    [SerializeField] private GameObject indicator;
    [Space]
    [SerializeField] private MatchSettingsHolder settings;
    [SerializeField] private TeamColorsHolder teamColorsHolder;

    public PlayerScoreInfo ScoreInfo { get; private set; }
    public void Setup(PlayerScoreInfo info)
    {
        name_txt.text = PlayersHolder.GetPlayerInfo(info.playerId).name.ToString();
        score_txt.text = info.Score.Current.ToString();
        background.color = teamColorsHolder.Colors[info.team];

        ScoreInfo = info;
        bool isLocalPlayer = info.playerId == MyNetworkManager.singleton.LocalClientId;
        indicator.SetActive(isLocalPlayer);
    }

    public void ChangeScore(PlayerScoreInfo info)
    {
        score_txt.text = info.Score.Current.ToString();

        ScoreInfo = info;
    }
}
