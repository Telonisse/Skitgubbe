using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Card : NetworkBehaviour
{
    [SerializeField] int cardNumber;
    [SerializeField] bool isDead = false;

    [SerializeField] GameObject grabObject;

    public void ToggleObjectActiveState(bool isActive)
    {
        // Call an RPC to toggle the object for all clients
        RPC_ToggleObjectState(isActive);
    }

    // The RPC that is called to synchronize the object state across clients
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ToggleObjectState(bool isActive)
    {
        grabObject.SetActive(isActive);
    }

    public int GetCardNum()
    {
        return cardNumber;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void KillCard()
    {
        isDead = true;
    }
}
