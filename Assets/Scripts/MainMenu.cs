using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        if (SaveVariables.TUTORIAL_DONE == 1)
        {
            SceneManager.LoadScene("Village");
        }
        else
        {
            SceneManager.LoadScene("Cinematic1");
        }
    }

    public void DeleteSavedGame()
    {
        SaveVariables.clearVariables();
        SaveManager.Instance.DeleteSaveGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
