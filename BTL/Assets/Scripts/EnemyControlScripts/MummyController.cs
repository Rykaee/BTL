using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyController : MonoBehaviour
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

    //Animation
    private Animator anim;
    public ParticleSystem blood;
    public float deathTimer = 0.8f;
    public bool deathTimerIsRunning = false;

    //Knockback variables
    private float knockBackLength = 0.25f;
    private float knockBackForce = 10f;
    private float knockBackCounter;


    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();

        //unparent the checkpoints so that they don't move with the enemy
        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = true;

        enemyCurrentHealth = enemyMaxHealth; //set enemy health

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //If the enemy has not been knocked back
        if (knockBackCounter <= 0)
        {
            //players position into movement
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
            }

            //Debug.Log(enemyRb.velocity.x);
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //If the enemy has not been knocked back
        if (knockBackCounter <= 0)
        {
            //Increased movement speed when player is close and follow
            if (distance < 4.5 && enemyRB.position.x > leftPoint.position.x && enemyRB.position.x < rightPoint.position.x) //Only follow between points
            {
                anim.SetBool("isAttacking", true); //Change animation to attack
                moveSpeed = 5;
                MoveTowardsPlayer(movement);

                if (enemyRb.position.x > player.position.x) //Flip sprite according to player if in attack range
                {
                    sprRend.flipX = true;
                }
                else
                {
                    sprRend.flipX = false;
                }
            }
            else
            {
                anim.SetBool("isAttacking", false);
                moveSpeed = 1.5f;
                MoveBetweenPoints();

                if (enemyRb.velocity.x < 0) //Flip sprite according to points if not in attack range
                {
                    sprRend.flipX = true;
                }
                if (enemyRb.velocity.x > 0)
                {
                    sprRend.flipX = false;
                }

            }
        }

        if (enemyCurrentHealth <= 0)
        {
            DontMove(movement);
            anim.enabled = false;
            Destroy(gameObject.GetComponent<Collider2D>()); //Destroy collider so enemy can't be hit
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            hitTimer = 2.0f; //Increase hit timer so dead enemy won't hit player

            //Destroy after 0.8s allowing particles to play
            deathTimerIsRunning = true;
            if (deathTimer > 0)
            {
                deathTimer -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void DontMove(Vector2 direction) //Bubblegum solution to stop the enemy
    {
        enemyRB.MovePosition((Vector2)transform.position + (direction * 0 * Time.deltaTime));
    }

    void MoveTowardsPlayer(Vector2 direction)
    {
        enemyRB.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
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

    void TakeDamage(float damage)
    {
        //Reduce enemy health
        enemyCurrentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("monstergettinghit");
        //Play particle system when enemy dies, called here to only have it play once
        if (enemyCurrentHealth <= 0)
        {
            blood.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "AttackHitbox")
        {
            Debug.Log("Löit vihollista, hienosti tehty");
            damage = 1.0f;
            TakeDamage(damage);
            KnockBack(PlayerController.instance.facingRight);
        }
        if (other.gameObject.name == "AttackHitboxExt")
        {
            damage = 2.0f;
            TakeDamage(damage);
            Debug.Log("Tupladamage, hienosti tehty");
            KnockBack(PlayerController.instance.facingRight);
        }
    }

    //Knockback
    public void KnockBack(bool facingRight)
    {
        knockBackCounter = knockBackLength;
        if (facingRight)
        {
            enemyRB.velocity = new Vector2(knockBackForce, knockBackForce);
        }
        else
        {
            enemyRB.velocity = new Vector2(-knockBackForce, knockBackForce);
        }
    }
}

