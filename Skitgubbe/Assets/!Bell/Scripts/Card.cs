using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Card : NetworkBehaviour
{
    [SerializeField] int cardNumber;
    [SerializeField] bool isDead = false;

    [SerializeField] GameObject grabObject;
    [Networked] public bool isGrabbed { get; set; } = false;

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
        ToggleObjectActiveState(false);
    }
    public void GrabCard()
    {
        if (HasStateAuthority)  // Only the server (authority) updates this
        {
            isGrabbed = true;  // Set the networked grab state to true
            Debug.Log("Card grabbed.");
        }
    }

    // Function to check if the card has been grabbed (returns the networked bool)
    public bool IsGrabbed()
    {
        return isGrabbed;
    }
}
