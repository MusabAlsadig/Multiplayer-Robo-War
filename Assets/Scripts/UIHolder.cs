using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(NetworkObject))]
    public class UIHolder : MonoBehaviour
    {
        public static UIHolder Instance { get; private set; }

        public NetworkObject NetworkObject { get; private set; }
        private void Awake()
        {
            Instance = this;
            NetworkObject = GetComponent<NetworkObject>();
        }


    }

}