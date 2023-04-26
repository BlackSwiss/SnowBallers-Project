using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class Head : MonoBehaviour
{
    [SerializeField] private Transform rootObject, followObject;
    [SerializeField] private Vector3 positionOffset, rotationOffset, headBodyOffset;
    [SerializeField] private bool follow;

    // Start is called before the first frame update
    void Start()
    {
        if(!follow)
            return;
        if(!followObject)
        {
            XROrigin rig = FindObjectOfType<XROrigin>();
            followObject = rig.transform.Find("Camera Offset/Main Camera");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!follow)
            return;

        rootObject.position = transform.position + headBodyOffset;
        rootObject.forward = Vector3.ProjectOnPlane(followObject.up, Vector3.up).normalized;

        transform.position = followObject.TransformPoint(positionOffset);
        transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);
    }
}
