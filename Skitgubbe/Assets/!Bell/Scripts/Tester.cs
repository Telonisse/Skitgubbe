using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Card")
        {
            other.GetComponentInChildren<IInteractor>().Unselect();
            Debug.Log("Deselected" + other.gameObject.name);
            other.GetComponentInChildren<IInteractor>().Select();
        }
    }
}
