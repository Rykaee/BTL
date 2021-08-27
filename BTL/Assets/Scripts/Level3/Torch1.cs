using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch1 : MonoBehaviour
{

    private Animator anim;
    public bool Torch1Lit = false;

    public float distance;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Distance again here because reasons
        distance = Vector3.Distance(player.position, transform.position);

        if (distance < 3)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                anim.SetBool("TorchLit", true);
                Torch1Lit = true;
                FindObjectOfType<AudioManager>().Play("torchLightup");
                DoorController.instance.ChangeBool1(true);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                anim.SetBool("TorchLit", false);
                Torch1Lit = false;
                DoorController.instance.ChangeBool1(false);
            }
        }


    }
}
