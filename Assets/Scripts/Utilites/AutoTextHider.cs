using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilites
{

    public class AutoTextHider : MonoBehaviour
    {
        [SerializeField] private int hideAfter;
        private void OnEnable()
        {
            Invoke(nameof(Hide), hideAfter);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}