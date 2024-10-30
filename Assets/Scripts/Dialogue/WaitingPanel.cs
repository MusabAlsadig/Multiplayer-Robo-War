using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_txt;
    [SerializeField] private Button button;

    private event Action OnCancel;
    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        button.onClick.AddListener(Cancel);
    }

    private void Cancel()
    {
        OnCancel?.Invoke();
        cancellationTokenSource?.Cancel();
        text_txt.text = string.Empty;
        gameObject.SetActive(false);
    }

    public void Show(string text, Action onCancel)
    {
        gameObject.SetActive(true);
        this.OnCancel = onCancel;

        text_txt.text = text;
    }
    
    public void Show(string text, CancellationTokenSource cancellationTokenSource)
    {
        gameObject.SetActive(true);
        this.cancellationTokenSource = cancellationTokenSource;

        text_txt.text = text;
    }
}
