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
}
