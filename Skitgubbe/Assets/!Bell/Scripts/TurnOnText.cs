using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnOnText : MonoBehaviour
{
    [SerializeField] GameObject pickUpText;
    void Update()
    {
        if (!FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards())
        {
            pickUpText.SetActive(FindObjectOfType<SnapCounter>().LessThenThreeSnapped());
        }
    }
}
