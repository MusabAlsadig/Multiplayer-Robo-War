using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NameChanger : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button editName_btn;
    [SerializeField] private Button x_button;
    [Header("Terms")]
    [SerializeField] private Animation tooShort_pop;
    [SerializeField] private Animation tooLong_pop;

    private string newName = string.Empty;

    private void Awake()
    {
        nameInputField.onValueChanged.AddListener(UpdateName);
        editName_btn.onClick.AddListener(EditName);

        
    }

    private void OnEnable()
    {
        bool haveName = Data.GameData.Current.playerInfo.HaveName;

        x_button.gameObject.SetActive(haveName); // will ensure no going back with no name
    }

    private void UpdateName(string _value)
    {
        if (_value.Length > 32)
        {
            tooLong_pop.Play();
            nameInputField.SetTextWithoutNotify(_value.Substring(0, 32));
            return;
        }

        newName = _value;
        
    }

    private void EditName()
    {
        if (newName.Length < 3) 
        {
            tooShort_pop.Play();
            return;
        }
        if (newName.Length > 32)
        {
            tooLong_pop.Play();
            return;
        }

        Data.GameData.Current.playerInfo.name = newName;
        x_button.onClick.Invoke();
        Dialogue.ShowText("Name changed successfully");
    }

}
