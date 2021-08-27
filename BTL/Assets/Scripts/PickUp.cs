using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    UnityEngine.Experimental.Rendering.Universal.Light2D myLight; //To control players light
    Color lightBlue = new Color(0.10f, 0.55f, 0.69f); //Set the blue color
    public Transform lightSphere;
    public GameObject lightSphereObj;
    public GameObject lightSphereLight;
    public SpriteRenderer sphereRenderer;
    public float distance;

    public GameObject canvas;
    public Text putext;
    public GameObject pickUpables;
    private List<GameObject> listOfNearbyItems = new List<GameObject>();
    public GameObject inventoryslots;
    public bool pickedUp;
    private string[] itemNames = { "Staff", "Scroll Of Light", "Sword", "Health Potion"};

    //Animation change when staff equipped
    private Animator anim;


    /*On the Start lets automatically set correct tags, boxcolliders and Blinker-script to
     gameobject children.*/
    public void Start()
    {
        for (int i = 0; i < pickUpables.transform.childCount; i++)
        {
            GameObject child = pickUpables.transform.GetChild(i).gameObject;
            child.tag = "PickUpItem";

            if (child.GetComponent<BoxCollider2D>() == null)
            {
                BoxCollider2D col = child.AddComponent<BoxCollider2D>();
                col.size = new Vector2(0.5f, 0.5f);
                col.isTrigger = true;
            }
            Blinker blink = child.AddComponent<Blinker>();
        }

        anim = GetComponent<Animator>();
        myLight = GetComponentInChildren<UnityEngine.Experimental.Rendering.Universal.Light2D>();

    }

    public void Update()
    {
        distance = Vector3.Distance(lightSphere.position, transform.position);
     
        /*If pressed E, and there is an empty slot for item,
        canvas gameobject + text will appears and shows what have you done*/

        // Pick up item
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Pick up one item from list of nearby items
            if (listOfNearbyItems.Count > 0)
            {
                GameObject slotParent = null;
                StopAllCoroutines();
                //Finding empty inventory slot.
                for (int i = 0; i < inventoryslots.transform.childCount; i++)
                {
                    GameObject child = inventoryslots.transform.GetChild(i).gameObject;

                    for (int j = 0; j < child.transform.childCount; j++)
                    {
                        GameObject childsChild = child.transform.GetChild(j).gameObject;

                       if (childsChild.GetComponent<Transform>().childCount == 1 || childsChild.GetComponent<Transform>().gameObject.name == "Icon")
                       {
                         slotParent = childsChild;
                         break;
                       }
                    }
                }

                if (slotParent != null)
                {
                    FindObjectOfType<AudioManager>().Play("itempickup");
                    pickedUp = true;
                    if (listOfNearbyItems[0].name.Contains(itemNames[0]))
                    {
                        slotParent.GetComponent<Button>().onClick.AddListener(UsingStaff);
                    }
                    if (listOfNearbyItems[0].name.Contains(itemNames[1]))
                    {
                        slotParent.GetComponent<Button>().onClick.AddListener(UsingScroll);
                    }
                    if (listOfNearbyItems[0].name.Contains(itemNames[2]))
                    {
                        slotParent.GetComponent<Button>().onClick.AddListener(UsingSword);
                    }
                    if (listOfNearbyItems[0].name.Contains(itemNames[3]))
                    {
                        slotParent.GetComponent<Button>().onClick.AddListener(UsingPotion);
                    }

                    listOfNearbyItems[0].GetComponent<Blinker>().enabled = false;
                    StartCoroutine(waitingTime(listOfNearbyItems[0].name));
                    slotParent.gameObject.GetComponent<Image>().sprite = listOfNearbyItems[0].gameObject.GetComponent<SpriteRenderer>().sprite;
                    listOfNearbyItems[0].transform.position = slotParent.transform.position;
                    listOfNearbyItems[0].transform.SetParent(slotParent.transform);
                }
                else
                {
                    pickedUp = false;
                    StartCoroutine(waitingTime(listOfNearbyItems[0].name));
                }
            }
        }
    }

    /*Collision method which tells what to do with the gameobject. */
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUpItem"))
        {
            // Add item to list if not already in list
            if (!listOfNearbyItems.Contains(other.gameObject))
            {
                Blinker blinkerScript = other.GetComponent<Blinker>();
                if (blinkerScript != null)
                {
                    blinkerScript.StartBlinking();
                    listOfNearbyItems.Add(other.gameObject);
                }
            }
        }
    }

    /*Collision exit method which tells what to do with the gameobject.*/
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUpItem"))
        {
            // Remove item from list if it's there
            if (listOfNearbyItems.Contains(other.gameObject))
            {
                Blinker blinkerScript = other.GetComponent<Blinker>();
                if (blinkerScript != null)
                {
                    blinkerScript.StopBlinking();
                    listOfNearbyItems.Remove(other.gameObject);
                }
            }
        }
    }

    /*Coroutine method for canvas enabling/disabling*/
    private IEnumerator waitingTime(string name)
    {
        if (pickedUp == true)
        {
            canvas.SetActive(true);
            putext.text = "You picked up " + name;
            yield return new WaitForSeconds(2.0f);
            canvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
            putext.text = "You can't pick up " + name + "." + " Your inventory is full!";
            yield return new WaitForSeconds(2.0f);
            canvas.SetActive(false);
        }
    }
    /*Methods for using items.*/
    public void UsingStaff()
    {
        if (anim.GetBool("playerHasStaff")) //Unequip if staff already in hand
        {
            anim.SetBool("playerHasStaff", false);
        }
        else
        {
            anim.SetBool("playerHasStaff", true); //Equip if no staff yet
            anim.SetBool("playerHasSword", false);
        }
        //Add here what happens if clicked staff -> equip / unequip it.
    }

    public void UsingScroll()
    {
        Debug.Log("UsingScroll");
        if (distance < 7)
        {
            myLight.pointLightInnerRadius = 0.0f;
            myLight.pointLightOuterRadius = 10.0f;
            myLight.intensity = 2.75f;
            myLight.color = lightBlue;

            sphereRenderer.enabled = false;
            Destroy(lightSphereLight);
        }
        //Add here what happens if clicked scroll -> If light sphere is near, catch it
    }

    public void UsingSword()
    {
        if (anim.GetBool("playerHasSword")) //Unequip if sword already in hand
        {
            anim.SetBool("playerHasSword", false);
        }
        else
        {
            anim.SetBool("playerHasSword", true); //Equip if no sword yet
            anim.SetBool("playerHasStaff", false);
        }
        //Add here what happens if clicked sword -> equip / unequip it.
    }

    public void UsingPotion()
    {
        PlayerHealthController.instance.Heal();
        //Add here what happens if clicked potion -> use it and add health.
    }
}
