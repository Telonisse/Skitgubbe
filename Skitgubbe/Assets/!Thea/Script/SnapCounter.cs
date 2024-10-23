using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCounter : MonoBehaviour
{
    public List<GameObject> snapPoints; 
    private int currentSnapIndex = 3;

    GameObject lastCardThrown;
   

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Card") && IsOnlyOneSnapped())
        {
            Debug.LogError("only one snapped and Card is in hand");
            lastCardThrown = other.gameObject;
        }
    }

    public bool LastCardThrow()
    {
        if (lastCardThrown != null)
        {
            return lastCardThrown.GetComponent<Card>().IsThrown();
        }
        else
        {
            return false;
        }
    }

    int GetLastSnappedIndex()
    {
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
        foreach (GameObject snapPoint in snapPoints)
        {
            Snapped snappedScript = snapPoint.GetComponent<Snapped>();
            if (snappedScript != null && snappedScript.isSnappedBool)
            {
                return false; 
            }
        }
        return true; 
     }

    public bool IsOnlyOneSnapped()
    {
        int snappedCount = 0;
        foreach (GameObject snapPoint in snapPoints)
        {
            Snapped snappedScript = snapPoint.GetComponent<Snapped>();
            if (snappedScript != null && snappedScript.isSnappedBool)
            {
                snappedCount++;
            }

            if (snappedCount > 1)  
            {
                return false;
            }
        }

        return snappedCount == 1;  
    }
}
