using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Defaults
{
    public const string PlayerName = "Player";

    public static Settings Settings => new Settings()
    {
        mapIndex = 0,
        mode = Mode.TDM,
        maxScore = 10,
        
    };

    public const ulong MaxScore = 10;

    public const ulong ReviveTime = 3;

    public const string ScriptableObject = "Scriptable Object";


    // matches
    public const ushort FlagScore = 1;


    // testing
    public const int PlatformSpeed = 2;
    public const int PlatformStayDuration = 30;
}
