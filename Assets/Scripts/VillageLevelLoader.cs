using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageLevelLoader : MonoBehaviour
{
    public string levelToLoad = "Forest";
    public bool isEndOfTutorial = false;
    public GameObject blackOut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (isEndOfTutorial)
            {
                SaveVariables.TUTORIAL_DONE = 1;
                SaveManager.Instance.SaveGame();
                blackOut.GetComponent<Animator>().Play("FadeOut");
                Invoke("changeScene", 3f);
            }
            else
            {
                SceneManager.LoadScene(levelToLoad);
            }

           
        }
    }

    void changeScene()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
