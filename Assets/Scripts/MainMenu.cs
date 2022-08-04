using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject QuitAdvertise;
    public GameObject mainMenu;

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
                mainMenu.SetActive(true);
            }
            if (Input.GetButtonUp("Cancel") || Input.GetKeyUp(KeyCode.Escape))
            {
                delete = false;
                GameStateManager.Instance.SetState(GameState.Gameplay);
                QuitAdvertise.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
    }

    public void DeleteSavedGame()
    {
        mainMenu.SetActive(false);
        QuitAdvertise.SetActive(true);
        Invoke("setDelete", 0.4f);
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