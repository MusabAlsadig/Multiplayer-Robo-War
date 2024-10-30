using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PrematchTeamControlPanel : MonoBehaviour
{
    [SerializeField] Image teamImage;
    [Space]
    [SerializeField] TeamColorsHolder teamColorsHolder;
    


    int teamIndex = -1;

    private TeamName Team => (TeamName)teamIndex;

    private void Awake()
    {

        teamImage.color = Color.clear;
        teamImage.enabled = true;
    }

    public void LeftButton()
    {
        teamIndex = Helper.Decrease_in_a_Loop(teamIndex, -1, teamColorsHolder.Colors.Count - 2);

        ChangeTeam();
    }

    public void RightButton()
    {
        teamIndex = Helper.Increase_in_a_Loop(teamIndex, -1, teamColorsHolder.Colors.Count - 2);

        ChangeTeam();
    }

    void ChangeTeam()
    {
        ChangeImageColor();
        PrematchPlayer.LocalPlayer.Owner_ChooseTeam(Team);
    }

    void ChangeImageColor()
    {
        teamImage.color = teamColorsHolder.Colors[Team];
    }
}