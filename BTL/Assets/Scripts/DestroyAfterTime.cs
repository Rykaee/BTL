using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public int Seconds = 5;

    void Update()
    {
        Destroy(gameObject, Seconds);
    }
}
