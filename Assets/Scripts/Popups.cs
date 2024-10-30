using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Popups : MonoBehaviour
    {
        public TextMeshProUGUI text_prefab;
        private Camera Cam => CameraHolder.MainCamera;

        public static Popups Instance { get; private set; }


        private const int peakDamage = 50;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDamageDelt(int damage, Vector3 worldPosition)
        {
            // TODO : spwan them going away, with a little random. (so you know, not all of them should be the same, right?)

            var damageText = Instantiate(text_prefab, transform);
            damageText.transform.position = Cam.WorldToScreenPoint(worldPosition);
            damageText.text = damage.ToString();
            damageText.color = GetColorForDamage(damage);
        }



        #region Utilites
        private Color GetColorForDamage(int damage)
        {
            Color color = Color.white;
            damage = Mathf.Abs(damage);
            float percentage = Mathf.Clamp01(damage / (float)peakDamage);
            float invertedPercentage = 1 - percentage;

            // more damage = more red
            color.b = invertedPercentage;
            color.g = invertedPercentage;

            print($"{color} \n damage = {damage}, peakDamage = {peakDamage},\n percentage = {percentage}, invertedPercentage = {invertedPercentage}");
            return color;
        }

        #endregion
    }
}