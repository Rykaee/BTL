using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDestroyer : MonoBehaviour
{
    public float fadeOutTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col) //Movespeed to 0.4f when player in range
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(DoFadeIn(GetComponent<SpriteRenderer>()));
            Destroy(gameObject, 2f);
        }
    }

    IEnumerator DoFadeIn(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;

        while (tmpColor.a <= 1f)
        {
            tmpColor.a -= Time.deltaTime / fadeOutTime;
            _sprite.color = tmpColor;

            if (tmpColor.a >= 1f)
                tmpColor.a = 1.0f;

            yield return null;
        }
        _sprite.color = tmpColor;
    }
}
