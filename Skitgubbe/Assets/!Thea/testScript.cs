using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    [SerializeField] GameObject gameobjectTurnOff;

    public void TurnOff()
    {
        gameobjectTurnOff.SetActive(false);
    }
}
