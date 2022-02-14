using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{

    public GameObject inventoryUi;
    public GameObject pauseUi;
    public GameObject ResumeButton;
    public GameObject Map;
    public Camera minimapCam;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameStateManager.Instance.SetState(GameState.Gameplay);
            Time.timeScale = 1f;
            pauseUi.SetActive(false);
            inventoryUi.SetActive(false);
        }

            if (Input.GetButtonDown("Pause"))
        {
            EventSystem.current.SetSelectedGameObject(ResumeButton);
            inventoryUi.SetActive(false);
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);

            if (newGameState == GameState.Gameplay)
            {
                Time.timeScale = 1f;
                pauseUi.SetActive(false);
            }
            else
            {
                pauseUi.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        if (Input.GetAxisRaw("Map") != 0)
        {
            Map.transform.localScale = new Vector3(3, 3, 3);
            minimapCam.orthographicSize = 150;
        }
        else
        {
            Map.transform.localScale = new Vector3(1, 1, 1);
            minimapCam.orthographicSize = 35;
        }

        if (Input.GetButtonDown("Inventory"))
        {
            pauseUi.SetActive(false);
            inventoryUi.GetComponent<UIInventory>().navigationRefresh();

            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);

            if (newGameState == GameState.Gameplay)
            {
                Time.timeScale = 1f;
                inventoryUi.SetActive(false);
            }
            else
            {
                inventoryUi.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void Resume()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Time.timeScale = 1f;
        pauseUi.SetActive(false);
    }

    public void MainMenu()
    {
        //Load Main menu
        PlayerPrefs.DeleteAll();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
