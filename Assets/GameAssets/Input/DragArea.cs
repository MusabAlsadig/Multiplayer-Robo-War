using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace InputManagement
{
    public class DragArea : OnScreenControl, IDragHandler
    {
        public static bool IsOnSmartPhone => PlayerInput.IsOnSmartPhone;

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private void Awake()
        {
            if (IsOnSmartPhone)
            {
                var image = GetComponent<Image>();
                var color = image.color;
                //color.a = 0f;
                image.color = color;
            }
            else
                gameObject.SetActive(false);
        }
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!IsOnSmartPhone)
                return;

            SendValueToControl(eventData.delta);
        }

    }
}