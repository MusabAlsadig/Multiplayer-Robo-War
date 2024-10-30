using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class HitIndicator : MonoBehaviour
    {
        [SerializeField] Animation animation;
        [SerializeField] Image image;

        [Header("Settings")]
        [SerializeField, Range(0, MaxIndencity)] private float indencityPerHit = 0.1f;
        [Tooltip("Seconds to remove indicator from full indencity")]
        [SerializeField, Range(0.5f, 3)] private float totalFadeTime = 1;

        private bool isHit;
        private float currentIndencity;

        private const float MaxIndencity = 0.5f;

        private void Update()
        {
            currentIndencity -= Time.deltaTime * MaxIndencity / totalFadeTime;
            currentIndencity = Mathf.Clamp(currentIndencity, 0 , MaxIndencity);
            UpdateImageColor();
        }

        [ContextMenu("Show")]
        public void Show()
        {
            currentIndencity += indencityPerHit;
            UpdateImageColor();
        }

        private void UpdateImageColor()
        {
            Color color = image.color;
            color.a = currentIndencity;
            image.color = color;
        }
        
    }

}