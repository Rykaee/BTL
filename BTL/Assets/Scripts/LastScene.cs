using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastScene : MonoBehaviour
{
    public GameObject lastText;
    public GameObject anotherText;
    public GameObject player;

    void Start()
    {
        StartCoroutine(BlinkForText());
    }

    IEnumerator BlinkForText()
    {
        lastText.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        lastText.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        anotherText.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        anotherText.SetActive(false);
        player.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("CreditsScene");
    }
}
