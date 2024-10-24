using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnOnText : MonoBehaviour
{
    [SerializeField] GameObject pickUpText;
    [SerializeField] TextMeshPro currentIndex;
    void Update()
    {
        if (!FindObjectOfType<NetworkedCardHandler>().HasDealtAllCards())
        {
            pickUpText.SetActive(FindObjectOfType<SnapCounter>().LessThenThreeSnapped());
        }
        currentIndex.text = FindObjectOfType<NetworkedCardHandler>().CurrentIndex().ToString();
    }
}
