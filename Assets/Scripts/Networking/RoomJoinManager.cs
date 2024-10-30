using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomJoinManager : MonoBehaviour
{
    [SerializeField] private GameObject roomButton = default;
    [SerializeField] private float refreshTime = 2f;
    [SerializeField] private Text info_txt;


    private MyNetworkManager NetworkManger => MyNetworkManager.singleton;


    private void OnEnable()
    {

        NetworkDiscoveryManager.singleton.M_SearchForMatch();

        Debug.Log("Searching.....");
        InvokeRepeating(nameof(Refresh), 0f, refreshTime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Refresh));

        if (!MyNetworkManager.singleton.IsServer)
            MyNetworkManager.singleton.M_StopBroadcast();

        Debug.Log("Stopped Searching");
    }

    private void Refresh()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int count = NetworkManger.lanConnrctions.Count;

        info_txt.gameObject.SetActive(count == 0);

        for (int i = 0; i < count; i++)
        {
            GameObject _roomButton = Instantiate(roomButton,transform);
            _roomButton.GetComponentInChildren<Text>().text = NetworkManger.lanConnrctions[i].roomName;
        }
    }


    public void Join(int number)
    {
        var connection = NetworkManger.lanConnrctions[number - 1]; // since ther is a text now
        NetworkManger.M_JoinMatch(connection);
    }


}
