using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Card : NetworkBehaviour
{
    [SerializeField] int cardNumber;
    [SerializeField] bool isDead = false;

    [SerializeField] GameObject grabObject;
    [Networked] public bool isGrabbed { get; set; }
    [Networked] public bool isThrown { get; set; }

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
        RPC_GrabCard();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_GrabCard()
    {
        if (HasStateAuthority)
        {
            isGrabbed = true;
            Debug.Log("Card grabbed.");
        }
    }

    // Function to check if the card has been grabbed (returns the networked bool)
    public bool IsGrabbed()
    {
        return isGrabbed;
    }
    public bool IsThrown()
    {
        return isThrown;
    }

    public void SetIsThrown(bool thrown)
    {
        isThrown = thrown;
    }

    public bool GrabbableOn()
    {
        return grabObject.activeSelf;
    }
}
