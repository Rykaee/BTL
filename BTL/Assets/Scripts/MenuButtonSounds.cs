using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonSounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverSound()
    {
        FindObjectOfType<AudioManager>().Play("menuhover");
    }

    public void ClickSound()
    {
        FindObjectOfType<AudioManager>().Play("menuclick");
    }
}
