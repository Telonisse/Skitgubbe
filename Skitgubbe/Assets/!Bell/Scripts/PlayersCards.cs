using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersCards : MonoBehaviour
{
    [SerializeField] GameObject corner1;
    [SerializeField] GameObject corner2;

    // Position P to check
    [SerializeField] Vector3 positionP;
    [SerializeField] OVRManager ovrm;

    [SerializeField] bool yourCards;

    [SerializeField] NetworkedDealCard[] cardHandlers;

    [SerializeField] TextMeshPro currentIndex;

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

    }

    private void IsYourCards()
    {
        if (turningOn == false &&yourCards && FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards() && AllCardsGrabbed() == false && FindObjectOfType<SnapCounter>().AreAllSnapPointsUnsnappped() == true)
        {
            StartCoroutine(TurnOnCardDelay1());
        }
        if (yourCards && FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards() && AllCardsGrabbed() == true && FindObjectOfType<SnapCounter>().AreAllSnapPointsUnsnappped() == true)
        {
            StartCoroutine(TurnOnCardDelay2());
        }
    }

    IEnumerator TurnOnCardDelay1()
    {
        turningOn = true;
        if (cardHandlers[0] == null)
        {
            currentIndex.text += "No 1 card";
        }
        if (cardHandlers[1] == null)
        {
            currentIndex.text += "No 2 card";
        }
        if (cardHandlers[2] == null)
        {
            currentIndex.text += "No 3 card";
        }
        if (cardHandlers[3] == null)
        {
            currentIndex.text += "No 4 card";
        }
        if (cardHandlers[4] == null)
        {
            currentIndex.text += "No 5 card";
        }
        if (cardHandlers[5] == null)
        {
            currentIndex.text += "No 6 card";
        }
        if (cardHandlers[0].IsDown() == false)
        {
            currentIndex.text += "First card turned on";
            cardHandlers[0].TurnOnCards();
        }
        if (cardHandlers[1].IsDown() == false)
        {
            currentIndex.text += "Second card turned on";
            cardHandlers[1].TurnOnCards();
        }
        if (cardHandlers[2].IsDown() == false)
        {
            currentIndex.text += "Third card turned on";
            cardHandlers[2].TurnOnCards();
        }
        if (cardHandlers[3].IsDown() == false)
        {
            currentIndex.text += "Fourth card turned on";
            cardHandlers[3].TurnOnCards();
        }
        if (cardHandlers[4].IsDown() == false)
        {
            currentIndex.text += "Fifth card turned on";
            cardHandlers[4].TurnOnCards();
        }
        if (cardHandlers[5].IsDown() == false)
        {
            currentIndex.text += "Sixth card turned on";
            cardHandlers[5].TurnOnCards();
        }

        //foreach (NetworkedDealCard card in cardHandlers)
        //{
        //    if (!card.IsDown())
        //    {
        //        Debug.Log("Upper cards turned on");
        //        card.RPC_TurnOnCards();
        //    }
        //    yield return new WaitForSeconds(1);
        //}
        yield return new WaitForSeconds(1);
        turningOn = false;
    }
    IEnumerator TurnOnCardDelay2()
    {
        foreach (NetworkedDealCard card in cardHandlers)
        {
            if (card.IsDown())
            {
                Debug.Log("Lower cards turned on");
                card.TurnOnCards();
            }
            yield return new WaitForSeconds(1);
        }
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
