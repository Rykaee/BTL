using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public static DoorController instance; //Individual torches call this script

    //Variables for all the torches
    public float TimeOfTorch1;
    public bool Torch1On = false;

    public float TimeOfTorch2;
    public bool Torch2On = false;

    public float TimeOfTorch3;
    public bool Torch3On = false;

    public float TimeOfTorch4;
    public bool Torch4On = false;

    //as soon as the game starts, set instance to this
    private void Awake()
    {
        instance = this;
    }

    public void ChangeBool1(bool torch1lit)
    {
        if (torch1lit == true)
        {
            Torch1On = true;
            TimeOfTorch1 = Time.time; //Mark the time lit
        }
        else
        {
            Torch1On = false;
        }
    }

    public void ChangeBool2(bool torch2lit)
    {
        if (torch2lit == true)
        {
            Torch2On = true;
            TimeOfTorch2 = Time.time; //Mark the time lit
        }
        else
        {
            Torch2On = false;
        }
    }

    public void ChangeBool3(bool torch3lit)
    {
        if (torch3lit == true)
        {
            Torch3On = true;
            TimeOfTorch3 = Time.time; //Mark the time lit
        }
        else
        {
            Torch3On = false;
        }
    }
    
    public void ChangeBool4(bool torch4lit)
    {
        if (torch4lit == true)
        {
            Torch4On = true;
            TimeOfTorch4 = Time.time; //Mark the time lit
        }
        else
        {
            Torch4On = false;
        }

        CheckDoor(); //Check on the final torch
    }

    void CheckDoor()
    {
        //Check the torches were turned on in the right order
        if (Torch1On == true && TimeOfTorch1 < TimeOfTorch2 && Torch2On == true && Torch3On == true && TimeOfTorch2 < TimeOfTorch3 && TimeOfTorch3 < TimeOfTorch4 && Torch4On == true)
        {
            Debug.Log("Open door");
            CameraController.instance.ShowDoor(); //Call the camera to move
            DoorAnimation.instance.DoorTrigger(); //Call the door animation
            FindObjectOfType<AudioManager>().Play("doorAvailable"); //Play sound
        }
    }

}
