using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed = 1.0f;
    public GameObject Camera;
    public Transform StartPoint, EndPoint;
    private Rigidbody2D cameraRB;

    public float pauseTimer = 5.0f;
    public bool pauseTimerIsRunning = false;



    public Transform target;
    public Transform doorTarget;

    public float smoothing;

    public bool stopFollowing;
    public bool doorOpened = false;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cameraRB = GetComponent<Rigidbody2D>();

        EndPoint.parent = null; //Unparent the endpoint so it stays at the door
    }

    private void FixedUpdate()
    {
        if (doorOpened == true)
        {           
            if (transform.position != doorTarget.position)
            {
                Vector3 doorPosition = new Vector3(doorTarget.position.x, doorTarget.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, doorPosition, smoothing);
            }
            
            pauseTimerIsRunning = true;

            if (pauseTimer > 0)
            {
                pauseTimer -= Time.deltaTime;
            }
            else
            {
                    if (transform.position != target.position)
                    {
                        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
                    }

                pauseTimerIsRunning = false;
                doorOpened = false;
            }

        }
        else
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

    public void ShowDoor()
    {
        doorOpened = true;
    }
}
