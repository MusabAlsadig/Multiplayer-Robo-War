using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Match Goals", menuName = Defaults.ScriptableObject + "/ Match Goals")]
[SerializeField]
public class DefaultMatchGoals : ScriptableObject
{
    [SerializeField] private Duration[] defaultMatchDurations = new Duration[] { new Duration(0, 30), new Duration(1, 0) };
    [SerializeField] private ulong[] defaultMatchMaxScores = new ulong[] { 2, 5, 10, 15, 20, 30, 40, 50 };

    

    public Duration[] MaxTimeDefaults => defaultMatchDurations;
    public ulong[] MaxScoreDefaults => defaultMatchMaxScores;

    
}
