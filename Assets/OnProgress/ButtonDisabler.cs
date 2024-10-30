using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ButtonDisabler : MonoBehaviour
{
    [SerializeField] private Button goOnlineButton;
    [SerializeField] private Button lanButton;

    [SerializeField] private int refreshDelay = 2;

    private float time;

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = refreshDelay;
            RefreshButtons();
        }
    }

    private void RefreshButtons()
    {
        bool canPlayLan = Helper.TryGetAddress(out _);
        lanButton.interactable = canPlayLan;

        StartCoroutine(CheckInternetConnection(ToggleOnlineButton));
    }

    private void ToggleOnlineButton(bool state)
    {
        goOnlineButton.interactable = state;
    }


    private IEnumerator CheckInternetConnection(Action<bool> syncResult)
    {
        const string echoServer = "https://google.com";

        bool result;
        using (var request = UnityWebRequest.Head(echoServer))
        {
            request.timeout = refreshDelay;
            yield return request.SendWebRequest();
            
            result = request.result == UnityWebRequest.Result.Success;
        }
        syncResult(result);
    }
}
