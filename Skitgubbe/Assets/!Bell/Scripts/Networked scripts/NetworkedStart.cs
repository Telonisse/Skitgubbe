using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedStart : NetworkBehaviour
{
    [SerializeField] GameObject[] setActiveObjects;

    public void StartAll()
    {
        RPC_ToggleObjectState(true);
    }

    // The RPC that is called to synchronize the object state across clients
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ToggleObjectState(bool isActive)
    {
        foreach (var obj in setActiveObjects)
        {
            obj.SetActive(isActive);
        }
    }
}
