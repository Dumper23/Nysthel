using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageLevelLoader : MonoBehaviour
{
    public string levelToLoad = "Forest";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //We will need to check wich level to go or create different exits but for now we go to the forest (1st lvl)
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
