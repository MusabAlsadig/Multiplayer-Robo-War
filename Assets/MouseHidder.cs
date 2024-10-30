using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHidder : MonoBehaviour
{
    public static void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    
    public static void ShowMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
