using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public static bool show = true;
    public static void LogError(object message)
    {
        if (!show)
            return;

        Debug.LogError("<color=red>Log Error</color>\n" + message);
    }
    
    public static void LogWarning(object message)
    {
        if (!show)
            return;

        Debug.LogError("<color=yellow>Log Warning</color>\n" + message);
    }
    
    public static void LogException(object message)
    {
        if (!show)
            return;

        Debug.LogError("<color=red>Log Exception</color>\n" + message);
    }
}
