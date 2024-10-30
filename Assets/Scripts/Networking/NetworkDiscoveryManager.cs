using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;
using System;


public class NetworkDiscoveryManager : NetworkDiscovery
{
    public static NetworkDiscoveryManager singleton;


    [SerializeField] private float timeout = 1f;

    Dictionary<LanConnectionInfo, float> lanAddresses = new Dictionary<LanConnectionInfo, float>();

    

    protected new void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();
    }

    public void M_SearchForMatch()
    {
        ListenToBroadcast();
        StartCoroutine(nameof(CleanupExpiredEntities));
        
    }

    public bool M_ShowMatch(string ipAddress, ushort port, string roomName)
    {
        M_StopBroadcast();
        return StartBroadcasting(new LanConnectionInfo(ipAddress, port, roomName));
    }

    public void M_StopBroadcast()
    {
        StopBroadcasting();
        lanAddresses.Clear();
        UpdateMatchInfo();
    }

    IEnumerable CleanupExpiredEntities()
    {
        var delay = new WaitForSecondsRealtime(timeout);
        while (true)
        {
            bool changed = false;

            var keys = lanAddresses.Keys.ToList();

            foreach (var key in keys)
            {
                if (lanAddresses[key] <= Time.time)
                {
                    lanAddresses.Remove(key);
                    changed = true;
                }
            }

            if (changed)
                UpdateMatchInfo();

            yield return delay;
        }

    }

    protected override void OnReceivedBroadcast(LanConnectionInfo info)
    {
        if (Helper.TryGetAddress(out string ipAddress) && ipAddress == info.ipAddress)
            return;

        if (lanAddresses.ContainsKey(info) == false)
        {
            lanAddresses.Add(info, Time.time + timeout);
            UpdateMatchInfo();
            Debug.Log("found someone \n" + info);
        }
    
    }

    void UpdateMatchInfo()
    {
        MyNetworkManager.singleton.lanConnrctions = lanAddresses.Keys.ToList();
    }
}
