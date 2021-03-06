using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    //used so that we can use DealDamage() function in other scripts
    public static PlayerHealthController instance;

    public int playerCurrentHealth, playerMaxHealth;

    public float invincibilityLength;
    public float invincibilityCounter;

    private SpriteRenderer sprRend;



    //as soon as the game starts, set instance to this
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Set players health to max at the start of the game/level
        playerCurrentHealth = playerMaxHealth;

        sprRend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(invincibilityCounter > 0f)
        {
            invincibilityCounter -= Time.deltaTime;

            if(invincibilityCounter <= 0f)
            {
                //set the players alpha back to normal
                sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, 1f);
            }
        }
    }

    //Function that deals given damage, can be called from other scripts
    public void DealDamage(int damage)
    {
        
        if (invincibilityCounter <= 0f)
        {

            playerCurrentHealth -= damage;
            
            //disable the player if health is 0 or less
            if (playerCurrentHealth <= 0)
            {
                FindObjectOfType<AudioManager>().Play("0hp");
                playerCurrentHealth = 0;
                gameObject.SetActive(false);

                //restart the current level
                LevelManager.instance.RestartLevel();
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("gettinghit");   
                //reset the invincibility counter
                invincibilityCounter = invincibilityLength;

                //set the players alpha to half
                sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, .5f);
            }

            UIController.instance.UpdateHealthUI();
        }
    }

    public void Heal()
    {
        playerCurrentHealth = 6; //Give max health
        UIController.instance.UpdateHealthUI();
    }
}