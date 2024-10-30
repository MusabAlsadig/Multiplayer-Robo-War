using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueClasses
{

    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text_txt;
        [SerializeField] private Button confirm_btn;
        [SerializeField] private Button cancle_btn;
        public void Setup(string text, UnityAction onConfirm, bool canCancle)
        {
            text_txt.text = text;

            onConfirm += Finish;
            confirm_btn.onClick.AddListener(onConfirm);

            if (canCancle)
                cancle_btn.onClick.AddListener(Finish);
            else
                cancle_btn.gameObject.SetActive(false);

        }

        private void Finish()
        {
            Destroy(gameObject);
            DialogueManager.Instance.Finish();
        }
    }
}
