using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCardsHandler : MonoBehaviour
{
    [SerializeField] GameObject[] dealCardPos;
    [SerializeField] List<GameObject> cards;
    [SerializeField] int currentDealCardPos;

    public void DealCard(GameObject card)
    {
        if (currentDealCardPos < dealCardPos.Length && IsCardInPile(card) == false)
        {
            card.transform.position = dealCardPos[currentDealCardPos].transform.position;
            dealCardPos[currentDealCardPos].GetComponent<DealCard>().AssignCard(card);
            currentDealCardPos++;
            cards.Add(card);
        }
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
