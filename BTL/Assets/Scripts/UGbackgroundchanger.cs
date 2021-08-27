using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UGbackgroundchanger : MonoBehaviour
{
    public Canvas bgcanvas;
    public GameObject canvas;
    public Image srImage;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level2Scene")
        {
            canvas.SetActive(false);
        }
    }
    
    //Check the collision to player and then change bg for ug.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && SceneManager.GetActiveScene().name == "Level1Scene")
        {
            srImage = bgcanvas.GetComponentInChildren<Image>();
            Destroy(srImage);
        }

        if (collision.gameObject.tag == "Player" && SceneManager.GetActiveScene().name == "Level2Scene")
        {
            canvas.SetActive(true);
            srImage = bgcanvas.GetComponentInChildren<Image>();
        }

        if (collision.gameObject.tag == "Player" && SceneManager.GetActiveScene().name == "Level3Scene")
        {
            collision.gameObject.transform.position = new Vector3(2f, -78.0f);
            srImage = bgcanvas.GetComponentInChildren<Image>();
            Destroy(srImage);
        }
    }

}
