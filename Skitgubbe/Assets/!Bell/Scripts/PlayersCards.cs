using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersCards : MonoBehaviour
{
    public Vector3 corner1;
    public Vector3 corner2;

    // Position P to check
    public Vector3 positionP;

    void Update()
    {
        if (IsPointInBox(positionP, corner1, corner2))
        {
            Debug.Log("Position P is inside the box.");
        }
        else
        {
            Debug.Log("Position P is outside the box.");
        }
    }

    // Function to check if point P is inside the box
    bool IsPointInBox(Vector3 P, Vector3 corner1, Vector3 corner2)
    {
        // Calculate the min and max bounds of the box
        Vector3 min = Vector3.Min(corner1, corner2);
        Vector3 max = Vector3.Max(corner1, corner2);

        // Check if P is within bounds on all axes
        return (P.x >= min.x && P.x <= max.x) &&
               (P.y >= min.y && P.y <= max.y) &&
               (P.z >= min.z && P.z <= max.z);
    }
}
