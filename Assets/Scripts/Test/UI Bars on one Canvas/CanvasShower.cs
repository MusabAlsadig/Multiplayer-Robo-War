using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasShower : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;

    public static CanvasShower instance;

    private void Awake()
    {
        instance = this;
    }

    public void Refresh(float _value, Vector3 worldPosition)
    {
        slider.value = _value;
        slider.transform.position = cam.WorldToScreenPoint(worldPosition);
        
    }
}
