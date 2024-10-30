using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Team Colors", menuName = Defaults.ScriptableObject + "/ Team Colors")]
[SerializeField]
public class TeamColorsHolder : ScriptableObject
{
    public Dictionary<TeamName, Color> Colors { get; private set; } = new Dictionary<TeamName, Color>();

    [SerializeField] private Color solo = Color.clear;
    [SerializeField] private Color yellow = Color.yellow;
    [SerializeField] private Color red = Color.red;
    [SerializeField] private Color blue = Color.blue;
    [SerializeField] private Color green = Color.green;

    public TeamColorsHolder()
    {
        Colors[TeamName.Solo] = solo;
        Colors[TeamName.Yellow] = yellow;
        Colors[TeamName.Red] = red;
        Colors[TeamName.Blue] = blue;
        Colors[TeamName.Green] = green;
    }


    
}
