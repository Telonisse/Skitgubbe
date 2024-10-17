using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] int cardNumber;
    [SerializeField] bool isDead = false;

    [SerializeField] GameObject grabObject;

    public void GrabObject(bool grabObjectOn)
    {
        grabObject.SetActive(grabObjectOn);
    }

    public int GetCardNum()
    {
        return cardNumber;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void KillCard()
    {
        isDead = true;
    }
}
