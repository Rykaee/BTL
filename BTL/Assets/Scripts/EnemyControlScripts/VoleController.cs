using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoleController : MonoBehaviour
{
    //Moving enemy variables
    private Rigidbody2D enemyRB;
    public GameObject Player;
    public Transform player;
    public float distance;

    //To flip the sprite
    public Rigidbody2D enemyRb;
    public SpriteRenderer sprRend;


    //Hitting player variables
    public float hitTimer = 0.1f;
    public bool timerIsRunning = false;

    //Hitting enemy variables
    public float enemyCurrentHealth;
    public float enemyMaxHealth = 3;
    public float nextHitTime = 1; //cooldown between hits
    public float damage = 3; //hit damage, vole dies from one hit

    //Animation
    Animator voleAC;
    public ParticleSystem blood;
    public float deathTimer = 0.8f;
    public bool deathTimerIsRunning = false;


    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();

        enemyCurrentHealth = enemyMaxHealth; //set enemy health

        voleAC = GetComponent<Animator>(); //Animator controller of Vole
    }

    void Update()
    {

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
            hitTimer = 0.1f;
        }
    }

    private void FixedUpdate()
    {
        //Vole animation control
        if (distance < 5.0f)
        {
            //voleAC.SetFloat("direction", 1);
            voleAC.SetBool("isAttacking", true);

            if (enemyRb.position.x > player.position.x) //Flip sprite according to player if in attack range
            {
                sprRend.flipX = false;
            }
            else
            {
                sprRend.flipX = true;
            }
        }
        else
        {
            //voleAC.SetFloat("direction", -1);
            voleAC.SetBool("isAttacking", false);
        }

        if (enemyCurrentHealth <= 0)
        {
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

    void TakeDamage(float damage)
    {
        //Reduce enemy health
        enemyCurrentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("monstergettinghit");
        //delete enemy when health runs out
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
            damage = 3.0f;
            TakeDamage(damage);
        }
        if (other.gameObject.name == "AttackHitboxExt")
        {
            damage = 3.0f;
            TakeDamage(damage);
            Debug.Log("Tupladamage, hienosti tehty");
        }
    }
}
