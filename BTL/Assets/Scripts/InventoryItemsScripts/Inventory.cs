using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Canvas uiCanvas;
    public static bool isGamePaused = false;

    [SerializeField] GameObject inventoryMenu;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isGamePaused)
            {
                FindObjectOfType<AudioManager>().Play("inventory_close");
                ResumeGame();
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("inventory_open");
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        inventoryMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        uiCanvas.enabled = true;
    }

    void PauseGame()
    {
        inventoryMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        uiCanvas.enabled = false;
    }

}
