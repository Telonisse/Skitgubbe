using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapped : MonoBehaviour
{
    public bool isSnappedBool;
    public MeshRenderer childMeshRenderer;
    public void IsSnapped()
    {
        isSnappedBool = true;

        if (childMeshRenderer != null)
        {
            childMeshRenderer.enabled = false;
        }
    }

    public void IsNotSnapped()
    {
        isSnappedBool = false;

        if (childMeshRenderer != null)
        {
            childMeshRenderer.enabled = true;
        }
    }
}
