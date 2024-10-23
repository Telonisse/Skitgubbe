using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if ( yourCards)
        {
            Debug.Log(NoCardsLeft());
        }
    }

    private void IsYourCards()
    {
        if (yourCards && FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards() && AllCardsGrabbed() == false && FindObjectOfType<SnapCounter>().AreAllSnapPointsUnsnappped() == true)
        {
            foreach (NetworkedDealCard card in cardHandlers)
            {
                if (!card.IsDown())
                {
                    Debug.Log("Upper cards turned on");
                    card.TurnOnCards();
                    Debug.Log(card.IsGrabbed());
                }
            }
        }
        if (yourCards && FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards() && AllCardsGrabbed() == true && FindObjectOfType<SnapCounter>().AreAllSnapPointsUnsnappped() == true)
        {
            foreach (NetworkedDealCard card in cardHandlers)
            {
                if (card.IsDown())
                {
                    Debug.Log("Lower cards turned on");
                    card.TurnOnCards();
                }
            }
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
