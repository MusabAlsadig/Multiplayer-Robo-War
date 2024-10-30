using HelpingMethods;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSelectorTest : MonoBehaviour
{
    [SerializeField] private int index;
    
    [SerializeField] private GameObject objectToInstanciate;

    [SerializeField] private Transform content;
    private Image olderImage;
    private void Awake()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(objectToInstanciate, content);
        }

        Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            index = 0;
        else if (Input.GetKeyDown(KeyCode.Keypad1))
            index = 1;
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            index = 2;
        else if (Input.GetKeyDown(KeyCode.Keypad3))
            index = 3;
        else if (Input.GetKeyDown(KeyCode.Keypad4))
            index = 4;
        else if (Input.GetKeyDown(KeyCode.Keypad5))
            index = 5;
        else if (Input.GetKeyDown(KeyCode.Keypad6))
            index = 6;
        else if (Input.GetKeyDown(KeyCode.Keypad7))
            index = 7;
        else if (Input.GetKeyDown(KeyCode.Keypad8))
            index = 8;
        else if (Input.GetKeyDown(KeyCode.Keypad9))
            index = 9;
        else
            return;

        Select();

    }

    [ContextMenu("Select")]
    private void Select()
    {
        var selectedObject = content.GetChild(index);
        if (olderImage != null)
            olderImage.color = Color.white;

        selectedObject.GetComponent<Image>().color = Color.blue;



        olderImage = selectedObject.GetComponent<Image>();

        GetComponent<ScrollRectSelector>().Select(index);
    }
}
