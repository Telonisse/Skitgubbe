using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Tester : NetworkBehaviour
{
    [SerializeField] GameObject win;

    private void Update()
    {
        if (GetComponent<PlayersCards>().YourCards() == true && GetComponent<PlayersCards>().NoCardsLeft() == true && FindObjectOfType<SnapCounter>().AreAllSnapPointsUnsnappped() == true && FindObjectOfType<SnapCounter>().LastCardThrow() == true)
        {
            ToggleObjectActiveState(true);
        }
    }

    void ToggleObjectActiveState(bool isActive)
    {
        RPC_ToggleObjectState(isActive);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ToggleObjectState(bool isActive)
    {
        win.SetActive(isActive);
    }
}
