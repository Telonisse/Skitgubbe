using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] IInteractor interactor;
    void Start()
    {
        interactor = GetComponentInChildren<IInteractor>();
        if (interactor != null)
        {
            Debug.Log("found");
        }
    }
}
