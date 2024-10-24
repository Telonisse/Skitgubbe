using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XRConsole : MonoBehaviour
{
    public string output = "";
    public string stack = "";



    [SerializeField]
    private TMP_Text stackText;
    [SerializeField]
    private TMP_Text outputText;

    ConcurrentQueue<string> outputQueue = new ConcurrentQueue<string>();

    private Transform centerEyeTransform;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float rotationSpeed = 1.0f;

    [SerializeField]
    ScrollRect scrollRect;

    [Flags]
    public enum MessageTypes
    {
        Log         = (1 << 0),
        Warning     = (1 << 1),
        Error       = (1 << 2),
        Exception   = (1 << 3),
        Assert      = (1 << 4)       
        
    }

    [SerializeField]
    private MessageTypes messageType;

    void Awake()
    {
        outputText.text = "";

        
    }

    private void Start()
    {
        OVRCameraRig rig = FindAnyObjectByType<OVRCameraRig>();
        centerEyeTransform = rig.centerEyeAnchor;
    }

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        bool allow = false;
        switch (type)
        {
            case LogType.Error:
                if (messageType.HasFlag(MessageTypes.Error))
                    allow = true;
                break;
            case LogType.Assert:
                if (messageType.HasFlag(MessageTypes.Assert))
                    allow = true;
                break;
            case LogType.Warning:
                if (messageType.HasFlag(MessageTypes.Warning))
                    allow = true;
                break;
            case LogType.Log:
                if (messageType.HasFlag(MessageTypes.Log))
                    allow = true;
                break;
            case LogType.Exception:
                if (messageType.HasFlag(MessageTypes.Exception))
                    allow = true;
                break;
            default:
                break;
        }

        if (allow)
        {
            outputQueue.Enqueue(logString);
            //guiDirty = true;
            output += logString;
            //stack = stackTrace;

            //UpdateGUI();
        }
    }


    void Update()
    {
        bool contentHasChanged = false;
        if(outputQueue.Count > 0) 
        {
            foreach (string item in outputQueue)
                outputText.text += item + "\n";

            outputQueue.Clear();

            contentHasChanged = true;
        }

        if(contentHasChanged) 
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
        }


        /*

        public RectTransform _element;

  
        float scrollValue = 1 + _element.anchoredPosition.y / scrollRect.content.GetHeight();
        scrollRect.verticalScrollbar.value = _scrollValue;
        */

        Vector3 targetPosition = centerEyeTransform.position +
                                centerEyeTransform.forward * offset.z +
                                centerEyeTransform.right * offset.x +
                                centerEyeTransform.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        Vector3 toCamera = transform.position - centerEyeTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(toCamera.normalized, centerEyeTransform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }
}
