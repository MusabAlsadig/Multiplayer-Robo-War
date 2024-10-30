using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private SlowTypingText txt;
    [SerializeField] private SlowTypingText reason_txt;
    [Space]
    [SerializeField] private MatchSettingsHolder settings;

    [Header("Server part")]
    [SerializeField] private Button returnButton;
    [Header("Client part")]
    [SerializeField] private SlowTypingText waitingForRematch_txt;
    private bool beenSetup;

    private bool allyTeamWin;
    private bool soloWin;

    private void Awake()
    {
        if (!beenSetup)
        {
            Debug.LogWarning("This panel should be disabled by default", this);
            gameObject.SetActive(false);
        }
    }
    public async void Show(bool didTimeEnd)
    {
        try
        {
            beenSetup = true;

            returnButton.gameObject.SetActive(false);
            waitingForRematch_txt.Clear();
            gameObject.SetActive(true);

            txt.Clear();
            reason_txt.Clear();


            await ShowReasonAsync(didTimeEnd);

            CheckScore();

            await ShowMatchResultAsync();

            if (MyNetworkManager.singleton.IsServer)
                ShowReturnButtom();
            else
            {
                waitingForRematch_txt.gameObject.SetActive(true);
                _ = waitingForRematch_txt.SetText("Wating for host to rematch...", 0.1f);
                //waitingForRematch_txt.AddDots();
            }


        }
        catch (System.Exception e)
        {
            Logger.LogException("Slower Than Server?\n" + e);
        }
    }
    private void CheckScore()
    {
        Dictionary<TeamName, Score> teamScores = new Dictionary<TeamName, Score>();
        Dictionary<ulong, Score> soloScores = new Dictionary<ulong, Score>();
        foreach (var info in PlayersHolder.GetCurrentScoreInfos())
        {
            if (info.team == TeamName.Solo)
                soloScores[info.playerId] = info.Score;
            else
            {
                if (teamScores.ContainsKey(info.team))
                    teamScores[info.team].Add(info.Score);
                else
                    teamScores[info.team] = info.Score;
            }
        }

        // Get highest scores
        ulong highestScore = 0;
        foreach (var soloScore in soloScores)
        {
            if (soloScore.Value.Current > highestScore)
                highestScore = soloScore.Value.Current;
        }
        foreach (var teamScore in teamScores)
        {
            if (teamScore.Value.Current > highestScore)
                highestScore = teamScore.Value.Current;
        }

        if (highestScore == 0)
        {
            // no winner if no-one scored
            allyTeamWin = false;
            soloWin = false;
            return;
        }

        // Get winners
        var winnerTeams = teamScores.Where(pair => pair.Value.Current == highestScore);
        var WinerSolos = soloScores.Where(pair => pair.Value.Current == highestScore);

        // Check if I'm a winner
        PlayerInfo localPlayerInfo = PlayersHolder.LocalPlayerInfo;
        allyTeamWin = winnerTeams.Any(pair => pair.Key == localPlayerInfo.team);
        soloWin = WinerSolos.Any(pair => pair.Key == localPlayerInfo.id);
    }

    private async Task ShowMatchResultAsync()
    {
        string resultText;
        Color resultColor;
        if (soloWin)
        {
            resultColor = Color.blue;
            resultText = "Victory";
        }
        else if (allyTeamWin)
        {
            resultColor = Color.blue;
            resultText = "Victory for the team";
        }
        else
        {
            resultColor = Color.red;
            resultText = "Match Lost";
        }
        txt.Color = resultColor;
        await txt.SetText(resultText);
    }

    private async Task ShowReasonAsync(bool timeEnded)
    {
        string endReason;
        if (timeEnded)
            endReason = "Time's up";
        else
            endReason = "Max score been reached";

        await reason_txt.SetText(endReason);
    }

    private void ShowReturnButtom()
    {
        returnButton.gameObject.SetActive(true);
        returnButton.onClick.AddListener(Server_ReturnToMatchSelection);
    }

    private void Server_ReturnToMatchSelection()
    {
        GameManager.Instance.Server_ReturnToMatchSelection();
    }
}
