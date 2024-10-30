using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityHelper
{
    public static void ResetRectTransform(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

}
