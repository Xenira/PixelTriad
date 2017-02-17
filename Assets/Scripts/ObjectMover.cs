using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    private Vector3 origin;
    public Vector3 target;
    public float smoothTime = 0.3F;
    public float magnitudeMin = 0.1f;
    private Vector3 velocity = Vector3.zero;
    public bool objectMoving = false;

    // Use this for initialization
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectMoving)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            if (velocity.magnitude <= magnitudeMin)
            {
                transform.position = target;
                objectMoving = false;
            }
        }
    }

    internal void MoveToOffset(Vector3 offset)
    {
        StartMovement(origin + offset);
    }

    internal void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    internal void ResetPosition()
    {
        StartMovement(origin);
    }

    internal void MoveToLocation(Vector3 target)
    {
        StartMovement(target);
    }

    private void StartMovement(Vector3 target)
    {
        this.target = target;
        objectMoving = true;
    }
}
