﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Miekka tuhoutuu");
            Destroy(gameObject);
        }
    }
}
