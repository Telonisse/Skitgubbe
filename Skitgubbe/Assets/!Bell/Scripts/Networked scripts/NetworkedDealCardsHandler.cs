using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkedDealCardsHandler : NetworkBehaviour
{
    [SerializeField] GameObject[] dealCardPos;
    [SerializeField] List<GameObject> cards;
    [Networked] int CurrentDealCardPos { get; set; }

    private GameObject card;

    public void DealCard(GameObject currentCard)
    {
        if (!HasStateAuthority)
        {
            return;
        }

        if (CurrentDealCardPos < dealCardPos.Length && IsCardInPile(currentCard) == false)
        {
            card = currentCard;
            RPC_AssignCardToPos();
            cards.Add(currentCard);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_AssignCardToPos()
    {
        card.transform.position = dealCardPos[CurrentDealCardPos].transform.position;
        dealCardPos[CurrentDealCardPos].GetComponent<NetworkedDealCard>().AssignCard(card);
        CurrentDealCardPos++;
    }

    bool IsCardInPile(GameObject checkCard)
    {
        foreach (var card in cards)
        {
            if (card == checkCard)
            {
                return true;
            }
        }
        return false;
    }
}
