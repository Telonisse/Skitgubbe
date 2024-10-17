using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class testScript : NetworkBehaviour
{
    [SerializeField] private GameObject objectToToggle;  // Reference to the object you want to enable/disable

    // This RPC will be called across the network to all clients
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ToggleObject(bool isActive)
    {
        objectToToggle.SetActive(isActive);
    }

    // You can call this from the host or any client
    public void ToggleObject(bool isActive)
    {
        if (HasStateAuthority)
        {
            RPC_ToggleObject(isActive);  // Call the RPC on all clients
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))  // Example key press to toggle
        {
            ToggleObject(!objectToToggle.activeSelf);  // Toggle the object state
        }
    }
}
