using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 18f;
    private Rigidbody2D projectileRB;

    private void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();

        if (PlayerController.instance.facingRight)
        {
            projectileRB.velocity = new Vector2(speed, projectileRB.velocity.y);
        }
        else
        {
            projectileRB.velocity = new Vector2(-speed, projectileRB.velocity.y);
        }
    }
}
