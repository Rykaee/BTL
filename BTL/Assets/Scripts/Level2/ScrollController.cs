using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{

    public GameObject Player;
    public Transform player;
    public float distance;


    //Animation 
    private Animator scrollAC;

    //Delay for opening
    public float openTimer = 1.5f;
    public bool openTimerIsRunning = false;

    // Use this for initialization
    void Start()
    {
        scrollAC = GetComponent<Animator>();
        scrollAC.SetFloat("Speed", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate distance to player, because colliders didn't behave in this context
        distance = Vector3.Distance(player.position, transform.position);

        if (distance < 3)
        {

            openTimerIsRunning = true;

            if (openTimer > 0)
            {
                openTimer -= Time.deltaTime;
                StartCoroutine(waitingTime());
            }
            else
            {
                scrollAC.SetFloat("Speed", 1.0f);
            }
        }
        else
        {
            //openTimer = 1.5f;
        }

        //Disable ScrollController script for pickup system.
        IEnumerator waitingTime()
        {
            yield return new WaitForSeconds(1.1f);
            this.GetComponent<ScrollController>().enabled = false;
            this.GetComponent<Animator>().enabled = false;
        }
    }
   
}
