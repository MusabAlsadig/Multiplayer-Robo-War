using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SlowTypingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;
    [Tooltip("Delay between letters, by seconds")]
    [SerializeField] private float displayDelay = 0.2f;

    private string message;
    private int currentCount = 0;

    private bool OnLastLetter => currentCount >= message.Length;
    public Color Color
    {
        get => txt.color;
        set => txt.color = value;
    }

    private void ShowNextLetter()
    {
        if (OnLastLetter)
        {
            return;
        }
        currentCount++;
        UpdateText();
    }

    private void UpdateText()
    {
        // TODO : auto locate rich text
        txt.SetText(message.Substring(0, currentCount));
    }

    #region Public Methods
    public async Task SetText(string text, float? customDelay = null)
    {
        message = text;

        if (customDelay != null)
        {
            displayDelay = customDelay.Value;
        }

        int totalDelay = (int)(displayDelay * 1000);

        for (int i = 0; i < message.Length; i++)
        {
            await Task.Delay(totalDelay);
            ShowNextLetter();
        }
    }

    /// <summary>
    /// Add 3 dots which will be repeted for infinity
    /// </summary>
    //public async void AddDots()
    //{
    //    int maxTrys = 10000;
    //
    //    for (int i = 0; i < maxTrys; i++)
    //    {
    //        await Task.Delay((int)(displayDelay * 3 * 1000));
    //
    //        if (!OnLastLetter)
    //            continue;
    //
    //        string currentText = txt.text;
    //        if (currentText.EndsWith("..."))
    //        {
    //            currentText.Remove(currentText.Length - 3, 3);
    //        }
    //        else
    //        {
    //            currentText += '.';
    //        }
    //
    //        txt.SetText(currentText);
    //    }
    //}

    public void AddText(string text)
    {
        //TODO : message += text;
    }

    public void Clear()
    {
        txt.text = string.Empty;
    }

    #endregion

    public void Hide()
    {
        // TODO : remove letters slowly
    }
}
