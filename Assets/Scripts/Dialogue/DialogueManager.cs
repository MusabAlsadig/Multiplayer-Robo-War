using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DialogueClasses
{

    internal class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [Header("Prefabs")]
        [SerializeField] private DialoguePanel panle_prefab;
        [SerializeField] private TextMeshProUGUI quickText_prefab;
        [SerializeField] private WaitingPanel waitingPanel;

        public static DialogueManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.activeSceneChanged += OnSceneChange;
            }
            else
                Destroy(gameObject);
            
        }

        internal void ShowPanel(string text, UnityAction onConfirm, bool canCancel)
        {
            Instantiate(panle_prefab, transform).Setup(text, onConfirm, canCancel);
            background.SetActive(true);
        }

        internal void Finish()
        {
            background.SetActive(false);
        }

        internal void CreateText(string text)
        {
            Instantiate(quickText_prefab, transform).text = text;
        }

        internal void ShowWaitingPanel(string text, Action onCancel)
        {
            waitingPanel.Show(text, onCancel);
        }
        
        internal void ShowWaitingPanel(string text, CancellationTokenSource cancellationTokenSource)
        {
            waitingPanel.Show(text, cancellationTokenSource);
        }


        internal void HideWaitingPanel()
        {
            waitingPanel.gameObject.SetActive(false);
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            HideWaitingPanel();
        }
    }

}