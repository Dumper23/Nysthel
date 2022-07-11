using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInfo : MonoBehaviour
{
    public GameObject infoToShow;
    public bool isEmmyr = false;
    public GameObject fireball;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (isEmmyr)
            {
                fireball.SetActive(true);
            }
            infoToShow.SetActive(true);
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            infoToShow.SetActive(false);
        }
    }
}
