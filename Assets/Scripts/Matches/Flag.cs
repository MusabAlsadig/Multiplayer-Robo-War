using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flag : NetworkBehaviour
{
    [SerializeField, Tooltip("to which team does this flag belong")] TeamName team;
    [SerializeField] Transform flagBase;

    NetworkVariable<bool> isHolden = new NetworkVariable<bool>(false);
    NetworkVariable<bool> isOnBase = new NetworkVariable<bool>(true);


    public override void OnNetworkSpawn()
    {
        // make sure this only work on server
        if (!IsServer)
            enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) 
            return;

        // need some rework here
        //if (other.CompareTag("Player") && !isHolden.Value)
        //{
        //    // touched on base by a player
        //
        //    if (team == other.GetComponentInParent<PrematchPlayer>().Team)
        //    {
        //        Server_SetParent(flagBase.gameObject);
        //        isOnBase.Value = false;
        //    }
        //
        //    else
        //    {
        //        // enemy player
        //        
        //        isHolden.Value = true;
        //        Server_SetParent(other.GetComponent<Player>().flagHolder.gameObject);
        //    }
        //}
        //
        //else if (other.TryGetComponent(out Flag otherFlag))
        //{
        //    if (otherFlag.isOnBase.Value)
        //    {
        //        transform.SetParent(flagBase);
        //        Server_Score();
        //    }
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("flag is totching " + other.name);
    }

    private void OnTransformParentChanged()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void Server_Detach()
    {
        if (!NetworkObject.TryRemoveParent())
            Debug.LogError($"fro some reason flag can't be detached");
    }

    private void Server_SetParent(GameObject parent)
    {
        if (!NetworkObject.TrySetParent(parent, false))
            Debug.LogError($"Can't set to this parent\n you sure that <color=yellow>{parent.name}</color> have a network object?", parent);
    }
    private void Server_Score()
    {
        MatchManagerBase.current.Server_AddScore(OwnerClientId, Defaults.FlagScore);
    }
}
