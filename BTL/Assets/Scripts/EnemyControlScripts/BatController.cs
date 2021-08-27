using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    //Moving enemy variables
    public float moveSpeed;
    public Transform leftPoint, rightPoint;
    private Rigidbody2D enemyRB;
    private bool movingRight;
    public GameObject Player;
    public Transform player;
    private Vector2 movement;
    public float distance;

    //To flip the sprite
    public Rigidbody2D enemyRb;
    public SpriteRenderer sprRend;


    //Hitting player variables
    public float hitTimer = 0.2f;
    public bool timerIsRunning = false;

    //Hitting enemy variables
    public float enemyCurrentHealth;
    public float enemyMaxHealth = 3;
    public float nextHitTime = 1; //cooldown between hits
    public float damage = 1; //hit damage

    //Animator batAC;



    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();

        //unparent the checkpoints so that they don't move with the enemy
        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = true;

        enemyCurrentHealth = enemyMaxHealth; //set enemy health

        //batAC = GetComponent<Animator>(); //Animator controller of Hog
    }

    void Update()
    {
        /*
        //players position  into movement
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

        //Calculate distance to player
        distance = Vector3.Distance(player.position, transform.position);

        if (distance < 0.5)
        {
            timerIsRunning = true;

            if (hitTimer > 0)
            {
                hitTimer -= Time.deltaTime;
            }
            else
            {
                //Knockback and damage the player
                if (PlayerHealthController.instance.invincibilityCounter <= 0f)
                {
                    PlayerController.instance.KnockBack(sprRend.flipX);
                    PlayerHealthController.instance.DealDamage(1);
                    timerIsRunning = false;
                    Debug.Log("Knocked back");
                }
            }
        }
        else
        {
            hitTimer = 0.2f;
        }*/
    }

    private void FixedUpdate()
    {
        moveSpeed = 3.0f;
        MoveBetweenPoints();
        if (enemyRb.velocity.x < 0) //Flip sprite according to points if not in attack range
        {
            sprRend.flipX = false;
        }
        if (enemyRb.velocity.x > 0)
        {
            sprRend.flipX = true;
        }
    }

    public void MoveBetweenPoints()
    {
        if (movingRight)
        {
            enemyRB.velocity = new Vector2(moveSpeed, enemyRB.velocity.y);

            if (transform.position.x > rightPoint.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            enemyRB.velocity = new Vector2(-moveSpeed, enemyRB.velocity.y);

            if (transform.position.x < leftPoint.position.x)
            {
                movingRight = true;
            }
        }
    }

    void TakeDamage()
    {
        //Reduce enemy health
        enemyCurrentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("monstergettinghit");
        //delete enemy when health runs out
        if (enemyCurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hitbox"))
        {
            Debug.Log("Löit vihollista, hienosti tehty");
            TakeDamage();
        }
    }
}
