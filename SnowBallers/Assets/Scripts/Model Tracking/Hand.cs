using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class Hand : MonoBehaviour
{
    // Physics Movement
    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private bool leftHand;
    private Transform _followTarget;
    private Rigidbody _body;

    [SerializeField] private bool follow;

    // Start is called before the first frame update
    void Start()
    {
        if(!follow)
            return;

        if(!followObject)
        {
            if(leftHand)
            {
                followObject = GameObject.FindGameObjectsWithTag("LeftController")[0];
            }
            else
            {
                followObject = GameObject.FindGameObjectsWithTag("RightController")[0];
            }
        }

        // Physics Movement
        _followTarget = followObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 26f;

        // Teleport hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if(!follow)
            return;
        
        PhysicsMove();
    }

    private void PhysicsMove()
    {
        // Position
        var positionWithOffset = _followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance (positionWithOffset, transform.position);
        _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        // Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }

    public void SetFollowTarget(GameObject newFollowTarget)
    {
        followObject = newFollowTarget;
    }
}
