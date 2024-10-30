using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HiddenPlatformsManager : NetworkBehaviour
{
    [SerializeField] private HidenPlatform_Server hidenPlatform_prefab;

    [Header("Settings")]
    [SerializeField] private int numberOfPlatforms = 5;
    [SerializeField] private Vector3Int maxLimits;

    public HiddenPlatformsManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            InvokeRepeating(nameof(SpawnPlatforms), 1, Defaults.PlatformStayDuration + 5);
        else
            enabled = false;
    }

    private void SpawnPlatforms()
    {
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            NetworkObject platform = Instantiate(hidenPlatform_prefab).NetworkObject;
            platform.transform.position = RandomPosition();

            platform.Spawn();
            platform.TrySetParent(NetworkObject);
        }
    }

    private Vector3Int RandomPosition()
    {
        float random = Random.value;

        int x, y, z;

        y = GetRandom(maxLimits.y, 1);

        if (random > 0.5f)
        {
            x = GetRandom(maxLimits.x);
            z = maxLimits.z;
        }

        else
        {
            x = maxLimits.x;
            z = GetRandom(maxLimits.z);
        }    

        return new Vector3Int(x, y, z);
    }

    private int GetRandom(int maxValue, int? min = null)
    {
        return Random.Range( min ?? -maxValue, maxValue + 1);
    }

}
