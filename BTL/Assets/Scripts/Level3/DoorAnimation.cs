using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorAnimation : MonoBehaviour
{
    public static DoorAnimation instance; //Call this script from doorcontrol

    private void Awake()
    {
        instance = this;
    }

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

        anim = GetComponent<Animator>();
    }

    public void DoorTrigger()
    {
        anim.SetTrigger("OpenDoor");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Level End Here");
            SceneManager.LoadScene("ToBeContinued");
        }
    }
}
