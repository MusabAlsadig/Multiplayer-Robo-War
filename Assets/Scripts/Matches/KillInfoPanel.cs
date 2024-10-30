using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillInfoPanel : MonoBehaviour
{
    [SerializeField] GameObject killInfo_prefab;
    [SerializeField] Transform panel;

    [Header("Settings")]
    [SerializeField, Tooltip("Time before deleting old messages")] private int shofInfoFor = 5;

    


    public void ShowKillInfo(ulong killerId, ulong victimId)
    {

        var killer = PlayersHolder.GetPlayerInfo(killerId);
        var victim = PlayersHolder.GetPlayerInfo(victimId);

        string k_color = GetColorFor(killer);
        string v_color = GetColorFor(victim);

        GameObject _killinfo = Instantiate(killInfo_prefab, panel);
        _killinfo.GetComponentInChildren<Text>().text = $"<color={k_color}>{killer.name}</color> have killed <color={v_color}>{victim.name}</color>";

        Destroy(_killinfo, shofInfoFor);
    }

    private string GetColorFor(PlayerInfo player)
    {
        ulong myID = MyNetworkManager.singleton.LocalClientId;

        if (player.id == myID)
            return "green";
        else if (PlayersHolder.IsMyAlly(player.id))
            return "blue";
        else
            return "red";
    }
    
}
