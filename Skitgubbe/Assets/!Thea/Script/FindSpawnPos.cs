using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using Meta.XR.MRUtilityKit;
using static Meta.XR.MRUtilityKit.MRUKAnchor;


public class FindSpawnPos : MonoBehaviour

{ [SerializeField] private List<Vector3> tablePositions = new List<Vector3>();

    public List<Vector3> GetTablePositions()
    {
        return tablePositions;
    }

    public void FindSpawnPosOnSurface()
    {
        // Find all objects with MRUKAnchor in the scene
        MRUKAnchor[] anchors = FindObjectsOfType<MRUKAnchor>();

        foreach (MRUKAnchor anchor in anchors)
        {
            if (anchor.HasAnyLabel(SceneLabels.TABLE))
            {
                tablePositions.Add(anchor.transform.position);
            }
            else
            {
                Debug.Log("These are not tables!");
            }
        }
    }

    //private void Start()
    //{
    //    // Call this when you want to initialize the positions
    //    FindSpawnPosOnSurface();
    //}
}
