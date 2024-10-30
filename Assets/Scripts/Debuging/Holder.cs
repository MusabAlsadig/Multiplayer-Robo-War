using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debuging
{
    /// <summary>
    /// just hold important stuff on editor for me,<br/>
    /// it will be destroyed once game started
    /// </summary>
    public class Holder : MonoBehaviour
    {
        [Header("Important Stuff")]
        [SerializeField] private ScriptableObject[] scriptableObjects;
        [SerializeField] private GameObject[] prefabs;

        private void Awake()
        {
            Destroy(gameObject);
        }
    }

}