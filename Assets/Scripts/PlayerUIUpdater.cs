using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUIUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform hp_fill;
        [SerializeField] private Transform energy_fill;
        [SerializeField] private Image crosshair;
        [SerializeField] private HitIndicator hitIndicator;
        [SerializeField] private TextMeshProUGUI respawning_Text;
        [Space]
        [SerializeField] private TextMeshProUGUI waitingForPlayers_txt;
        [Space]
        [SerializeField] private AllyOrEnemyColors colors;

        public static PlayerUIUpdater Instance { get; private set; }

        private bool isReviving;
        private float currentRevivingCountdown;

        private void Awake()
        {
            Instance = this;
            StopRespawningTimer();

            MatchManagerBase.OnMatchStarted += MatchManagerBase_OnMatchStarted;
        }

        

        private void Update()
        {
            if (isReviving)
                UpdateRevivingText();

        }


        private void MatchManagerBase_OnMatchStarted()
        {
            waitingForPlayers_txt.gameObject.SetActive(false);
        }

        public void UpdateHp(float amount)
        {
            Vector3 currentScale = hp_fill.localScale;
            currentScale.x = amount;
            hp_fill.localScale = currentScale;
        }

        public void UpdateEnergy(float amount)
        {
            Vector3 currentScale = energy_fill.localScale;
            currentScale.x = amount;
            energy_fill.localScale = currentScale;
        }

        public void UpdateCrosshair(AllyOrEnemy allyOrEnemy)
        {
            crosshair.color = colors[allyOrEnemy];
        }

        public void ShowHitIndicator()
        {
            hitIndicator.Show();
        }

        public void StartRespawningTimer()
        {
            isReviving = true;
            currentRevivingCountdown = Defaults.ReviveTime;
            respawning_Text.gameObject.SetActive(true);
        }
        
        public void StopRespawningTimer()
        {
            isReviving = false;
            respawning_Text.gameObject.SetActive(false);
        }

        private void UpdateRevivingText()
        {
            respawning_Text.text = "Respawning on.. " + currentRevivingCountdown.ToString("0.00");
            currentRevivingCountdown -= Time.deltaTime;
        }

    }

}