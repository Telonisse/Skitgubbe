using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapped : MonoBehaviour
{
    public bool isSnappedBool;
    public void IsSnapped()
    {
        isSnappedBool = true;
    }

    public void IsNotSnapped()
    {
        isSnappedBool = false;
    }
}
