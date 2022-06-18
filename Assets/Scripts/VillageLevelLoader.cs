using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageLevelLoader : MonoBehaviour
{
    public string levelToLoad = "Forest";
    public bool isEndOfTutorial = false;
    public GameObject blackOut;
    public GameObject text;
    public float timeToChange = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isEndOfTutorial)
            {
                SaveVariables.TUTORIAL_DONE = 1;

                blackOut.GetComponent<Animator>().Play("FadeOut");
                text.GetComponent<Animator>().Play("fadeInText");
                Invoke("changeScene", timeToChange);
            }
            else
            {
                changeScene();
            }
        }
    }

    private void changeScene()
    {
        SaveManager.Instance.SaveGame();
        SceneManager.LoadScene(levelToLoad);
    }
}