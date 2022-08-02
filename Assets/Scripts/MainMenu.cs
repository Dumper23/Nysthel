using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject QuitAdvertise;

    private bool delete = false;

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

    private void Update()
    {
        if (QuitAdvertise.activeInHierarchy && delete)
        {
            if (Input.GetButtonUp("Interact"))
            {
                SaveVariables.clearVariables();
                SaveManager.Instance.DeleteSaveGame();
                QuitAdvertise.SetActive(false);
                delete = false;
            }
            if (Input.GetButtonUp("Cancel") || Input.GetKeyUp(KeyCode.Escape))
            {
                delete = false;
                GameStateManager.Instance.SetState(GameState.Gameplay);
                QuitAdvertise.SetActive(false);
            }
        }
    }

    public void DeleteSavedGame()
    {
        QuitAdvertise.SetActive(true);
        Invoke("setDelete", 1f);
    }

    private void setDelete()
    {
        delete = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}