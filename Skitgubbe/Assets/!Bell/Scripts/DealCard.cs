using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fusion;

public class DealCard : MonoBehaviour
{
    [SerializeField] GameObject card;
    [SerializeField] bool isDown;

    [SerializeField] NetworkRunner runner;

    public void AssignCard(GameObject assignCard)
    {
        card = assignCard;
        card.GetComponent<Card>().GrabObject(false);
        if (!isDown)
        {
            card.transform.Rotate(180, 0, 0);
        }
    }
}
