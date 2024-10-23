using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCounter : MonoBehaviour
{
    public List<GameObject> snapPoints; 
    private int currentSnapIndex = 3;
   

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            snapPoints[i].SetActive(true);
        }
    }

    void Update()
    {
        UpdateSnapPoints();

        if (AreAllSnapPointsUnsnappped())
        {
            Debug.LogError("meow");
        }
    }

    void UpdateSnapPoints()
    {
        int lastSnappedIndex = GetLastSnappedIndex();

   
        if (lastSnappedIndex == currentSnapIndex - 1 && currentSnapIndex < snapPoints.Count)
        {
            snapPoints[currentSnapIndex].SetActive(true);
            currentSnapIndex++;
        }
        else if (lastSnappedIndex < currentSnapIndex - 2 && currentSnapIndex > 3)
        {
            currentSnapIndex--;
            snapPoints[currentSnapIndex].SetActive(false);
        }
        
    }

    int GetLastSnappedIndex()
    {
        // Find the highest index where the snap point is snapped
        for (int i = snapPoints.Count - 1; i >= 0; i--)
        {
            Snapped snappedScript = snapPoints[i].GetComponent<Snapped>();
            if (snappedScript != null && snappedScript.isSnappedBool)
            {
                return i; 
            }
        }

        return -1; 
    }

     public bool AreAllSnapPointsUnsnappped()
     {
        // Check if all snap points are unsnapped
        foreach (GameObject snapPoint in snapPoints)
        {
            Snapped snappedScript = snapPoint.GetComponent<Snapped>();
            if (snappedScript != null && snappedScript.isSnappedBool)
            {
                return false; // At least one is snapped, so return false
            }
        }
        return true; // All are unsnapped, return true
     }
}
