using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueHolder : MonoBehaviour
{
    [Range(0, 1)] public float value;


    private void Update()
    {
        CanvasShower.instance.Refresh(value, transform.position);
    }
}
