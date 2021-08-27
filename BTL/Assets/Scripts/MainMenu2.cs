using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu2 : MonoBehaviour
{
    /*
     *
     * Main Menu button functions
     * This part includes funcitons for main menu, settings menu, credits menu and level selection
     *
     */

    // Method that unlocks the levels that player have passed
    int LevelsUnlocked;
    public Button[] buttons;

    void Start()
    {
        LevelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < LevelsUnlocked; i++)
        {
            buttons[i].interactable = true;
        }
    }

    

    public static int btnId;

    //Main Menu play button function
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }

    //Main Menu Settings button function
    public void OpenSettings()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
   
    //Main Menu Credits button functions
    public void CreditsBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    //Main Menu quit button function
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quit"); //Tells Unity console when game closes
    }


    /*
     *
     * Functions for credits menu
     *
     */

    public Canvas BackGround;
    public static bool easterEggVisible = false;

    [SerializeField] GameObject easterEggVis;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (easterEggVisible)
            {
                OpenEgg();
            }
            else
            {
                CloseEgg();
            }
        }
    }

    public void OpenEgg()
    {
        easterEggVis.SetActive(false);
        easterEggVisible = false;
        BackGround.enabled = false;
    }

    void CloseEgg()
    {
        easterEggVis.SetActive(true);
        easterEggVisible = true;
        BackGround.enabled = true;

    }
    

    //Credits menu BackBtn functions
    public void CrdBackBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }


    /*
     *
     * Functions for settings menu
     *
     */

    // Settings menu Back button functions
    public void BackBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    // Settings menu Back button functions (Pause Menu)
    public void PMSetBackBtn()
    {
        SceneManager.LoadScene("Level1Scene");
    }


    /*
    *
    * Functions for level selection menu
    *
    */
    // Level selection menu back button
    public void LvlSelcBackBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }

    // Select level 1
    public void Level1Scene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
        //Lets set button id for later use. (Loadingscreen-script)
        btnId = 1;
        SceneManager.LoadScene("LoadingScreen");
    }

    // Select level 2
    public void Level2Scene()
    {
        //Lets set button id for later use. (For Loadingscreen-script)
        btnId = 2;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("LoadingScreen");
    }

    // Select level 3
    public void Level3Scene()
    {
        //Lets set button id for later use. (For Loadingscreen-script)
        btnId = 3;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("LoadingScreen");
    }

}
