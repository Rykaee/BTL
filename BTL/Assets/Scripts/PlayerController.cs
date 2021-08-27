using UnityEngine.Experimental.Rendering.LWRP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    UnityEngine.Experimental.Rendering.Universal.Light2D myLight;

    public float moveSpeed;
    public float jumpForce;
    public float waterjumpForce;
    public Rigidbody2D playerRb;
    private SpriteRenderer sprRend;

    private bool playerOnGround;
    public Transform groundCheck;
    public LayerMask groundLayerMask;

    private bool playerOnWater;
    public Transform waterCheck;
    [SerializeField] LayerMask waterLayerMask;

    private float waterCheckRadius = .1f;
    private float groundCheckRadius = .2f;
    private bool canDoubleJump;

    private Animator anim;

    public GameObject stonetablet;
    public GameObject sign;
    public GameObject sign2;

    Color lightBlue = new Color(0.10f, 0.55f, 0.69f);


    //Attacking variables
    public float attackCooldownTime, attackCooldown;

    //Knockback variables
    public float knockBackLength, knockBackForce;
    private float knockBackCounter;

    public bool facingRight;

    //Spell casting variables
    public Transform projectileSpawnPoint;
    public GameObject fireBall;

    public bool canFireSpell;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        sprRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        myLight = GetComponentInChildren<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        myLight.intensity = 0.0f;

        attackCooldown = attackCooldownTime;

        facingRight = true;

        canFireSpell = true;
}


    void Update()
    {
        if (knockBackCounter <= 0)
        {
            movePlayer();
            CheckGround();
            Jump();
            CheckWater();

            //if player is on the ground -> reset double jump
            if (playerOnGround)
            {
                canDoubleJump = true;

            }

            //attacking
            if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.C)) && attackCooldown <= 0)
            {
                FindObjectOfType<AudioManager>().Play("attackFist");
                Attack();
                attackCooldown = attackCooldownTime;
            }

            //Spell casting
            if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.X)) && attackCooldown <= 0)
            {
                if (canFireSpell)
                {
                    FindObjectOfType<AudioManager>().Play("valopallo");
                    ShootSpell();
                    attackCooldown = attackCooldownTime;
                }
            }

        } else
        {
            knockBackCounter -= Time.deltaTime;
        }

        // If player is on water lets set gravity scale to -0.01f and slow the player.
        if (playerOnWater == true)
        {
            playerRb.gravityScale = 0.1f;
            moveSpeed = 1.5f;
            //Debug.Log("Ollaa märkiä");
            canDoubleJump = false;
        }
        else
        {
            playerRb.gravityScale = 5f;
            moveSpeed = 7.5f;
        }
           
        //count down the attack cooldown
        attackCooldown -= Time.deltaTime;


        //set the variables used in the animator to the same variables in this script 
        anim.SetBool("playerOnGround", playerOnGround);
        anim.SetFloat("moveSpeed", Mathf.Abs(playerRb.velocity.x)); // <- take the absolute value of the velocity -> player animates while moving on a negative axis


        //footstep sounds not working
        /*
        if (playerOnGround == true && (playerRb.velocity.x < 0 || playerRb.velocity.x > 0) && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            FindObjectOfType<AudioManager>().Play("walking_grass");
        }
        else if (playerOnGround == false || playerRb.velocity.x == 0)
        {
           FindObjectOfType<AudioManager>().Stop("walking_grass");
        }
        */
    }

    //Player movement
    void movePlayer()
    {
        playerRb.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), playerRb.velocity.y);
        
        //flip the sprite based on which way the player is moving
        if (playerRb.velocity.x < 0)
        {

            //sprRend.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
            
        }
        else if (playerRb.velocity.x > 0)
        {
            //sprRend.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
    }
   
    //Check if the player is on the water
    void CheckWater()
    {
        playerOnWater = Physics2D.OverlapCircle(waterCheck.position, waterCheckRadius, waterLayerMask);
        //Debug.Log("VERES");
    }
    
    //Check if the player is on the ground
    void CheckGround()
    {
        playerOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);

    }

    //Jumping
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && playerOnGround)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            FindObjectOfType<AudioManager>().Play("jump");
        }
        else if (Input.GetButtonDown("Jump") && playerOnWater)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, waterjumpForce);
        }
        else
        {
            //double jumping
            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                FindObjectOfType<AudioManager>().Play("jump");
                canDoubleJump = false;
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("attack");
    }

    void ShootSpell()
    {
        anim.SetTrigger("ShootSpell");
        Instantiate(fireBall, projectileSpawnPoint.transform.position, transform.rotation);
    }


    public void KnockBack(bool facingRight)
    {
        knockBackCounter = knockBackLength;
        if(facingRight)
        {
            playerRb.velocity = new Vector2(knockBackForce, knockBackForce);
        } 
        else
        {
            playerRb.velocity = new Vector2(-knockBackForce, knockBackForce);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.name == "TriggerForUG" && SceneManager.GetActiveScene().name == "Level1Scene") //Turn light on when underground in lvl1
        {
            myLight.intensity = 0.8f;
        }

        if (col.gameObject.name == "DesertTrigger") //Turn off light when exiting cave in lvl2
        {
            myLight.intensity = 0.0f;
        }

        if (col.gameObject.tag == "Tablet")
        {
            stonetablet.SetActive(true);
        }

        if (col.gameObject.tag == "Sign")
        {
            sign.SetActive(true);
        }
        if (col.gameObject.name == "skull_sign")
        {
            sign2.SetActive(true);
        }

        /*if (col.gameObject.name == "Arkku") //Improved light when chest found
        {
            //Change light orientation (WIP)
            //myLight.pointLightInnerAngle = 0.0f;
            //myLight.pointLightOuterAngle = 110.0f;


            //Change light properties
            myLight.pointLightInnerRadius = 0.0f;
            myLight.pointLightOuterRadius = 10.0f;
            myLight.intensity = 2.75f;
            myLight.color = lightBlue;
        }*/

        if (col.gameObject.name == "PlayerTorchTrigger") //Turn light on when entering level2
        {
            myLight.intensity = 0.8f;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Tablet"))
        {
            stonetablet.SetActive(false);
        }

        if (col.gameObject.CompareTag("Sign"))
        {
            sign.SetActive(false);
        }
        if (col.gameObject.name == "skull_sign")
        {
            sign2.SetActive(false);
        }
    }
}
