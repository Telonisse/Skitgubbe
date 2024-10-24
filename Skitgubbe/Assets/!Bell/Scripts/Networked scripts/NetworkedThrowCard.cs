using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.WitAi;

public class NetworkedThrowCard : NetworkBehaviour
{
    [SerializeField] List<GameObject> cards;
    [SerializeField] GameObject killPile;
    [SerializeField] GameObject pickUpText;
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

    public void PickUpCards()
    {
        RPC_PickUpCards();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_PickUpCards()
    {
        if (pickUpCards)
        {
            pickUpCards = false;
        }
        else if (!pickUpCards)
        {
            pickUpCards = true;
        }
    }

    public override void Render()
    {
        Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z), Vector3.down, Color.red);
    }

    private void HandleCardLogic()
    {
        ShowLastCard();
        CheckFor10();
        CheckLast4();
        PickUpCardText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!pickUpCards)
        {
            if (other.tag == "Card" && IsCardInPile(other.gameObject) == false)
            {
                Card cardscript = other.GetComponent<Card>();
                if (cardscript != null)
                {
                    if (!first)
                    {
                        AddCardToPile(other.gameObject);
                        other.GetComponent<Card>().SetIsThrown(true);
                        other.GetComponent<Card>().ToggleObjectActiveState(false);
                        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        other.transform.position = this.transform.position;
                        first = true;
                    }
                    else if (other.GetComponent<Card>().GetCardNum() >= cards[cards.Count - 1].GetComponent<Card>().GetCardNum() || other.GetComponent<Card>().GetCardNum() == 2 || other.GetComponent<Card>().GetCardNum() == 10)
                    {
                        AddCardToPile(other.gameObject);
                        other.GetComponent<Card>().SetIsThrown(true);
                        other.GetComponent<Card>().ToggleObjectActiveState(false);
                        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        other.transform.position = this.transform.position;
                    }
                }
            }
        }
    }
    private void HandleCardPickup()
    {
        RPC_HandleCardPickup();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_HandleCardPickup()
    {
        foreach (var card in cards)
        {
            card.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.1f, this.transform.position.z);
            card.GetComponent<MeshRenderer>().enabled = true;
            card.GetComponent<Card>().ToggleObjectActiveState(true);
            card.GetComponent<Card>().SetIsThrown(false);
            card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        first = false;
        cards.Clear();
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

    public void ShowLastCard()
    {
        RPC_ShowLastCard();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ShowLastCard()
    {
        if (cards.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponent<MeshRenderer>().enabled = false;
            }
            cards[cards.Count - 1].GetComponent<MeshRenderer>().enabled = true;
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
                RPC_KillCards();
            }
        }
    }

    private void CheckFor10()
    {
        if (cards.Count > 0 && cards[cards.Count - 1].GetComponent<Card>().GetCardNum() == 10)
        {
            RPC_KillCards();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_KillCards()
    {
        KillCards();
    }

    private void KillCards()
    {
        foreach (var card in cards)
        {
            card.GetComponent<Card>().KillCard();
            card.GetComponent<MeshRenderer>().enabled = true;
            card.transform.position = killPile.transform.position;
            card.SetActive(true);
        }
        cards.Clear();
        first = false;
    }

    public void PickUpCardText()
    {
        if (!FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards())
        {
            pickUpText.SetActive(FindObjectOfType<SnapCounter>().LessThenThreeSnapped());
        }
    }
}
