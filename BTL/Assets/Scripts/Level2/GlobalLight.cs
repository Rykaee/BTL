using UnityEngine.Experimental.Rendering.LWRP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLight : MonoBehaviour
{
    UnityEngine.Experimental.Rendering.Universal.Light2D myLight;

    public GameObject Player;

    public float speed; //Speed of the fade
    private float direction = 1f;

    void Start()
    {
        myLight = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();

    }

    // Update is called once per frame
    private void Update()
    {
        //Reduce light to 0.2f when player is beyond XX
        if (myLight.intensity >= 0.2f && Player.transform.position.x < 415)
        {
            myLight.intensity += Time.deltaTime * speed * direction;
            direction = -1f;
        }

        //Increase light to 1.0f when player is above -25
        if (myLight.intensity <= 1.0f && Player.transform.position.x > 415)
        {
            myLight.intensity += Time.deltaTime * speed * direction;
            direction = 1f;
        }
    }
}
