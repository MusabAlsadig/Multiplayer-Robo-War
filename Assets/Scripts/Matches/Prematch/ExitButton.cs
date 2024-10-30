using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Dialogue.ShowPanel("You are about to leave this room", onConfirm: ExitGame, canCancel: true);
    }

    private void ExitGame()
    {
        GameManager.Instance.QuitMatch();
    }
}
