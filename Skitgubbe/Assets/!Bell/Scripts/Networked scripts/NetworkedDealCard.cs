using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

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
        }
        else
        {
            Debug.LogError("Assigned card isnt networked");
        }
        //card.GetComponent<Card>().GrabObject(false);
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
        }
    }
}
