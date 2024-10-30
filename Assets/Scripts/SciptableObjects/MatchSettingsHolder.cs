using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Match Settengs Holder", menuName = Defaults.ScriptableObject + "/ Match Settings Holder")]
[Serializable]
public class MatchSettingsHolder : ScriptableObject
{
    [SerializeField] private Settings settings = Defaults.Settings;

    public Settings Current => settings;

    public int MapIndex => settings.mapIndex;
    public Mode Mode => settings.mode;
    public ulong MaxScore => settings.maxScore;

    public Duration Duration => settings.Duration;


    public void Setup(Settings _settings)
    {
        settings = _settings;
    } 

    public bool HaveReachedMaxScore(Score score)
    {
        return score.Current >= settings.maxScore;
    }

}
