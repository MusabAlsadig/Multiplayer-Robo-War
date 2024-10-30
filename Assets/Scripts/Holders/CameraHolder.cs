using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraHolder : MonoBehaviour
{
    [SerializeField] private bool isMainCamera;

    public static Camera MainCamera { get; private set; }

    private void Start()
    {
        if (!isMainCamera)
            return;

        if (MainCamera != null)
            Debug.LogWarning("there is already amain camera");

        MainCamera = GetComponent<Camera>();
    }
}
