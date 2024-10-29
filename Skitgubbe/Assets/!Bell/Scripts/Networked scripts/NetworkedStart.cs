using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedStart : NetworkBehaviour
{
    [SerializeField] GameObject[] setActiveObjects;
    public FindSpawnPos findSpawnPos;

    public void StartAll()
    {
        RPC_ToggleObjectState(true);
    }

    // The RPC that is called to synchronize the object state across clients
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ToggleObjectState(bool isActive)
    {
        if (HasStateAuthority)
        {
            if (findSpawnPos == null || findSpawnPos.GetTablePositions().Count == 0) return;

            // Get the first table position
            Vector3 firstTablePosition = findSpawnPos.GetTablePositions()[0];

            // Check if there is at least one object in the setActiveObjects array
            if (setActiveObjects.Length > 0)
            {
                // Move the first object to the first table position
                GameObject firstObject = setActiveObjects[0];
                firstObject.transform.position = firstTablePosition;
                OVRManager ovrm = FindObjectOfType<OVRManager>();
                Vector3 pos = ovrm.GetComponent<OVRCameraRig>().centerEyeAnchor.position;
                pos.y = 0f; //firstObject.transform.position.y;
                firstObject.transform.rotation = Quaternion.LookRotation(pos);
            }
        }
        foreach (var obj in setActiveObjects)
        {
            obj.SetActive(isActive);
        }
    }
}
