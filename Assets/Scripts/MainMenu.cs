using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    private void Start()
    {
        SaveManager.Instance.loadGame();
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        LoadingScreen.SetActive(true);
        if (SaveVariables.TUTORIAL_DONE != 0)
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
