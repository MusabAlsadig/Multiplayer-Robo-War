using DialogueClasses;
using System;
using System.Threading;
using UnityEngine.Events;

/// <summary>
/// Methods for showing stuff on UI for the player, i.e. quick text <br/>
/// <br/>
/// <i>Basically a referral to <see cref="DialogueManager"/></i>
/// </summary>
public static class Dialogue
{
    public static void ShowPanel(string text) => ShowPanel(text, null, false);
    public static void ShowPanel(string text, UnityAction onConfirm) => ShowPanel(text, onConfirm, false);
    public static void ShowPanel(string text, UnityAction onConfirm, bool canCancel)
    {
        DialogueManager.Instance.ShowPanel(text, onConfirm, canCancel);
    }
    
    public static void ShowText(string text)
    {
        DialogueManager.Instance.CreateText(text);
    }

    public static void ShowWaitingPanel(string text, Action onCancel)
    {
        DialogueManager.Instance.ShowWaitingPanel(text, onCancel);
    }
    
    public static void ShowWaitingPanel(string text, CancellationTokenSource cancellationTokenSource)
    {
        DialogueManager.Instance.ShowWaitingPanel(text, cancellationTokenSource);
    }

    public static void HideWaitingPanel()
    {
        DialogueManager.Instance.HideWaitingPanel();
    }
}
