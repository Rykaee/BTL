﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerGeneric : MonoBehaviour
{
    public static CameraControllerGeneric instance;

    public bool stopFollowing = false;
    public Transform target;
    public float smoothing;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (stopFollowing == false)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
            }
        }
    }
}