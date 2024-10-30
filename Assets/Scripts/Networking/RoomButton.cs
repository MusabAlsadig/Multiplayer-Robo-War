using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField] private Text roomName_txt;
    public void JoinMatch()
    {
        transform.GetComponentInParent<RoomJoinManager>().Join(transform.GetSiblingIndex());
    }
}
