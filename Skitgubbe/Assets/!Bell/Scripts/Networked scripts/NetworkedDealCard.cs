using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Oculus.Interaction;

public class NetworkedDealCard : NetworkBehaviour
{
    [Networked] NetworkId networkedCardID {  get; set; }
    [SerializeField] bool isDown;
    [SerializeField] GameObject card;

    public void AssignCard(GameObject assignCard)
    {
        NetworkObject networkedCard = assignCard.GetComponent<NetworkObject>();
        if (networkedCard != null)
        {
            networkedCardID = networkedCard.Id;
            card = assignCard;
            card.GetComponent<Card>().ToggleObjectActiveState(false);
            if (!isDown)
            {
                card.transform.Rotate(180, 0, 0);
            }
            card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            Debug.LogError("Assigned card isnt networked");
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (networkedCardID.IsValid)
        {
            NetworkObject networkedCard = Runner.FindObject(networkedCardID);
            if (networkedCard != null)
            {
                card = networkedCard.gameObject;
            }
            else
            {
                Debug.Log("Card is null");
            }
        }
    }

    public void TurnOnCards()
    {
        RPC_TurnOnCards();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_TurnOnCards()
    {
        card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        card.GetComponent<Card>().ToggleObjectActiveState(true);
    }

    public bool IsDown()
    {
        return isDown;
    }

    public bool IsGrabbed()
    {
        if (card != null)
        {
            return card.GetComponent<Card>().IsGrabbed();
        }
        else
        {
            return false;
        }
    }

    public bool IsThrown()
    {
        if (card != null)
        {
            return card.GetComponent<Card>().IsThrown();
        }
        else
        {
            return false;
        }
    }

    public bool IsGrabOn()
    {
        if (card != null)
        {
            return card.GetComponent<Card>().GrabbableOn();
        }
        else
        {
            return false;
        }
    }
}
