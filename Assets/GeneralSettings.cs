using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GeneralSettings : MonoBehaviour
{
    [SerializeField] private bool enableLogger;



    private void Awake()
    {
        Logger.show = enableLogger;
    }
}
