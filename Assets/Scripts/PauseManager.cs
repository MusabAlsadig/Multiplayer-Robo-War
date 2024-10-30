using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Slider sensitivity_slider;
    [SerializeField] private float sensetivityMultiplier = 4;

    public static Transform Canvas { get; private set; }

    private bool isPaused;
    private bool matchEnded;

    private void Awake()
    {
        Canvas = transform;
        ChangePauseState(false); // to make sure its off

        ChangeSensivity(sensitivity_slider.value);
        sensitivity_slider.onValueChanged.AddListener(ChangeSensivity);

        MatchManagerBase.OnMatchEnded += MatchManagerBase_OnMatchEnded;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void OnDestroy()
    {
        MouseHidder.ShowMouse();
        MatchManagerBase.OnMatchEnded -= MatchManagerBase_OnMatchEnded;
    }


    private void MatchManagerBase_OnMatchEnded()
    {
        ChangePauseState(false);
        matchEnded = true;
    }

    private void ChangeSensivity(float _Sensivity)
    {
        PlayerMotor.rotationSpeedMultiplier = _Sensivity * sensetivityMultiplier;
    }

    private void ChangePauseState(bool value)
    {
        pausePanel.SetActive(value);
        isPaused = value;

        MouseHidder.ShowMouse();

        if (matchEnded)
            return;

        if (isPaused)
        {
            MouseHidder.ShowMouse();
            PlayerInput.DisableInput();
        }
        else
        {
            MouseHidder.HideMouse();
            PlayerInput.EnableInput();
        }
    }

    #region For UI
    public void TogglePause()
    {
        ChangePauseState(!isPaused);
    }
    public void Quit()
    {
        GameManager.Instance.QuitMatch();
    }

    #endregion
}
