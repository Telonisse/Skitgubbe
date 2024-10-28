using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersCards : NetworkBehaviour
{
    [SerializeField] GameObject corner1;
    [SerializeField] GameObject corner2;

    // Position P to check
    [SerializeField] Vector3 positionP;
    [SerializeField] OVRManager ovrm;

    [SerializeField] bool yourCards;

    [SerializeField] NetworkedDealCard[] cardHandlers;

    [SerializeField] TextMeshPro currentIndex;

    [Networked] bool secondPlayerReadyUpper {  get; set; }
    [Networked] bool secondPlayerReadyLower {  get; set; }

    private bool turningOn;
    private void Start()
    {
        ovrm = FindAnyObjectByType<OVRManager>();
    }
    void Update()
    {
        positionP = ovrm.GetComponent<OVRCameraRig>().centerEyeAnchor.transform.position;

        if (IsPointInBox(positionP, corner1.transform.position, corner2.transform.position))
        {
            yourCards = true;
            Debug.Log("In pos");
        }
        else
        {
            yourCards = false;
        }
        IsYourCards();
        if (yourCards)
        {
            Debug.Log(NoCardsLeft());
        }
        if (HasStateAuthority && secondPlayerReadyUpper)
        {
            TurnOnCardsForSecondPlayerUpper();
        }
        if (HasStateAuthority && secondPlayerReadyLower)
        {
            TurnOnCardsForSecondPlayerLower();
        }
    }

    private void TurnOnCardsForSecondPlayerUpper()
    {
        if (!turningOn)
        {
            StartCoroutine(TurnOnCardDelay1());
        }
    }
    private void TurnOnCardsForSecondPlayerLower()
    {
        if (!turningOn)
        {
            StartCoroutine(TurnOnCardDelay2());
        }
    }

    private void IsYourCards()
    {
        if (yourCards && FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards() && AllCardsGrabbed() == false && FindObjectOfType<SnapCounter>().AreAllSnapPointsUnsnappped() == true)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                RPC_RequestUpper();
            }
            else if (HasStateAuthority)
            {
                StartCoroutine(TurnOnCardDelay1());
                secondPlayerReadyUpper = false;
            }
            else
            {
                secondPlayerReadyUpper = false;
            }
        }
        if (yourCards && FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards() && AllCardsGrabbed() == true && FindObjectOfType<SnapCounter>().AreAllSnapPointsUnsnappped() == true)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                RPC_RequestLower();
            }
            else if (HasStateAuthority)
            {
                StartCoroutine(TurnOnCardDelay2());
                secondPlayerReadyLower = false;
            }
            else
            {
                secondPlayerReadyLower = false;
            }
        }

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestUpper()
    {
        if (HasStateAuthority)
        {
            secondPlayerReadyUpper = true;
            Debug.Log("Player requested and allowed");
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestLower()
    {
        if (HasStateAuthority)
        {
            secondPlayerReadyLower = true;
            Debug.Log("Player requested and allowed");
        }
    }

    IEnumerator TurnOnCardDelay1()
    {
        turningOn = true;
        foreach (NetworkedDealCard card in cardHandlers)
        {
            if (!card.IsDown())
            {
                Debug.Log("Upper cards turned on");
                card.TurnOnCards();
            }
            yield return new WaitForSeconds(1);
        }
        turningOn = false;
    }
    IEnumerator TurnOnCardDelay2()
    {
        turningOn = true;
        foreach (NetworkedDealCard card in cardHandlers)
        {
            if (card.IsDown())
            {
                Debug.Log("Lower cards turned on");
                card.TurnOnCards();
            }
            yield return new WaitForSeconds(1);
        }
        turningOn = false;
    }

    bool AllCardsGrabbed()
    {
        bool grabbedAll = true;
        foreach (NetworkedDealCard card in cardHandlers)
        {
            if (!card.IsGrabbed() && !card.IsDown())
            {
                grabbedAll = false;
            }
        }
        return grabbedAll;
    }

    int CardsOn()
    {
        int cardCount = 0;
        foreach(NetworkedDealCard card in cardHandlers)
        {
            if (!card.IsDown() && card.IsGrabbed())
            {
                cardCount++;
            }
        }
        return cardCount;
    }

    bool IsPointInBox(Vector3 P, Vector3 corner1, Vector3 corner2)
    {
        Vector3 min = Vector3.Min(corner1, corner2);
        Vector3 max = Vector3.Max(corner1, corner2);

        //return (P.x >= min.x && P.x <= max.x) && (P.y >= min.y && P.y <= max.y) && (P.z >= min.z && P.z <= max.z);
        return (P.x >= min.x && P.x <= max.x) && (P.z >= min.z && P.z <= max.z);
    }

    public bool YourCards()
    {
        return yourCards;
    }

    public bool NoCardsLeft()
    {
        bool grabbedAll = true;
        foreach (NetworkedDealCard card in cardHandlers)
        {
            if (!card.IsGrabbed() && !card.IsThrown())
            {
                grabbedAll = false;
            }
        }
        return grabbedAll;
    }
}
