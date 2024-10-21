using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkedThrowCard : NetworkBehaviour
{
    [SerializeField] List<GameObject> cards;
    [SerializeField] GameObject killPile;
    [Networked] public bool first { get; set; } = false;
    [Networked] public bool pickUpCards { get; set; } = false;

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        if (!pickUpCards)
        {
            HandleCardLogic();
        }
        if (pickUpCards)
        {
            HandleCardPickup();
        }
    }

    public override void Render()
    {
        Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z), Vector3.down, Color.red);
    }

    private void HandleCardLogic()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z), Vector3.down, out hit, 1f))
        {
            if (hit.collider.tag == "Card" && IsCardInPile(hit.collider.gameObject) == false)
            {
                if (!first)
                {
                    AddCardToPile(hit.collider.gameObject);
                    first = true;
                }
                else if (hit.collider.GetComponent<Card>().GetCardNum() >= cards[cards.Count - 1].GetComponent<Card>().GetCardNum() || hit.collider.GetComponent<Card>().GetCardNum() == 2 || hit.collider.GetComponent<Card>().GetCardNum() == 10)
                {
                    AddCardToPile(hit.collider.gameObject);
                }
            }
        }
        ShowLastCard();
        CheckFor10();
        CheckLast4();
    }

    private void HandleCardPickup()
    {
        GameObject lastInactiveObject = FindLastInactive(cards);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.up, out hit, 1f))
        {
            if (hit.collider.tag != "Card")
            {
                if (lastInactiveObject != null)
                {
                    lastInactiveObject.SetActive(true);
                }
                else
                {
                    pickUpCards = false;
                }
            }
        }
    }

    private void AddCardToPile(GameObject card)
    {
        NetworkObject networkedCard = card.GetComponent<NetworkObject>();
        if (networkedCard != null)
        {
            RPC_AddCardToPile(networkedCard.Id);
        }
    }

    // Sync card addition across all clients
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_AddCardToPile(NetworkId cardId)
    {
        NetworkObject networkedCard = Runner.FindObject(cardId);
        if (networkedCard != null)
        {
            GameObject card = networkedCard.gameObject;
            cards.Add(card);
            //card.transform.position = this.transform.position;
            //card.SetActive(false);
        }
    }
    private GameObject FindLastInactive(List<GameObject> cards)
    {
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (!cards[i].activeSelf)
            {
                return cards[i];
            }
        }
        return null;
    }

    private bool IsCardInPile(GameObject checkCard)
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

    private void ShowLastCard()
    {
        if (cards.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].gameObject.SetActive(false);
            }
            cards[cards.Count - 1].SetActive(true);
        }
    }

    private void CheckLast4()
    {
        if (cards.Count > 3)
        {
            int a = cards[cards.Count - 1].GetComponent<Card>().GetCardNum();
            int b = cards[cards.Count - 2].GetComponent<Card>().GetCardNum();
            int c = cards[cards.Count - 3].GetComponent<Card>().GetCardNum();
            int d = cards[cards.Count - 4].GetComponent<Card>().GetCardNum();

            if (a == b && b == c && c == d)
            {
                KillCards();
            }
        }
    }

    private void CheckFor10()
    {
        if (cards.Count > 0 && cards[cards.Count - 1].GetComponent<Card>().GetCardNum() == 10)
        {
            KillCards();
        }
    }

    private void KillCards()
    {
        foreach (var card in cards)
        {
            card.GetComponent<Card>().KillCard();
            card.transform.position = killPile.transform.position;
            card.SetActive(true);
        }
        cards.Clear();
        first = false;
    }
}
