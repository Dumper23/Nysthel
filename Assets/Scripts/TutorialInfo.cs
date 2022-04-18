using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInfo : MonoBehaviour
{
    public GameObject infoToShow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
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
