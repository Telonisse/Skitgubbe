using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandMenu : MonoBehaviour
{
    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Transform centerEye;

    [SerializeField]
    private UnityEvent onShow;

    [SerializeField]
    private UnityEvent onHide;

    [SerializeField]
    private float visibilityCoolDown = 0.5f;

    [SerializeField]
    private float visibilityAngle = 45f;

    private bool isVisible = false;

    private float lastVisibilityTime;


    // Start is called before the first frame update
    void Start()
    {
        OVRCameraRig rig = FindAnyObjectByType<OVRCameraRig>();
        centerEye = rig.centerEyeAnchor;
        lastVisibilityTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
       // if (lastVisibilityTime - Time.time < visibilityCoolDown)
       //     return;

        Vector3 toCamera = centerEye.position - orientation.position;
        //float dot = Vector3.Dot(toCamera, orientation.up);

        if (Vector3.Angle(orientation.up, toCamera.normalized) < visibilityAngle)// && isVisible == false)
        {
            isVisible = true;
            onShow?.Invoke();
            lastVisibilityTime = Time.time;
        }
        else
        {
            isVisible = false;
            onHide?.Invoke();

            lastVisibilityTime = Time.time;
        }
    }

    public void OnPoked()
    {

    }

    private static float axisLength = 0.3f;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(orientation.position, orientation.position + orientation.up * axisLength);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(orientation.position, orientation.position + orientation.right * axisLength);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(orientation.position, orientation.position + orientation.forward * axisLength);

        if (centerEye != null)
        {
            Vector3 toCamera = centerEye.position - orientation.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(orientation.position, orientation.position + toCamera.normalized * axisLength);
        }
    }
}
