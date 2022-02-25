using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    public GameObject inventoryUi;
    public GameObject pauseUi;
    public GameObject ResumeButton;
    public GameObject Map;
    public Camera minimapCam;
    public Toggle toggle;

    private void Start()
    {
        toggle.isOn = SaveVariables.PLAYER_USING_CONTROLLER;
    }

    void Update()
    {

        if(GameStateManager.Instance.CurrentGameState == GameState.Paused)
        {
            Cursor.visible = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().audioSource[0].Pause();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().audioSource[1].Pause();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().audioSource[2].Pause();
        }
        else
        {
            Cursor.visible = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().audioSource[0].UnPause();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().audioSource[1].UnPause();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().audioSource[2].UnPause();
        }

            

        if (Input.GetButtonDown("Cancel"))
        {
            GameStateManager.Instance.SetState(GameState.Gameplay);
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().timeSlowed)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0.5f;
            }
            pauseUi.SetActive(false);
            inventoryUi.SetActive(false);
        }

        if (Input.GetButtonDown("Pause") && !GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inShop)
        {
            EventSystem.current.SetSelectedGameObject(ResumeButton);
            inventoryUi.SetActive(false);
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState;

            if (currentGameState == GameState.Paused)
            {
                newGameState = GameState.Gameplay;
            }
            else
            {
                newGameState = GameState.Paused;
            }

            GameStateManager.Instance.SetState(newGameState);

            if (newGameState == GameState.Gameplay)
            {
                if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().timeSlowed)
                {
                    Time.timeScale = 1f;
                }
                else
                {
                    Time.timeScale = 0.5f;
                }
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
                if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().timeSlowed)
                {
                    Time.timeScale = 1f;
                }
                else
                {
                    Time.timeScale = 0.5f;
                }
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
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().timeSlowed)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.5f;
        }
        pauseUi.SetActive(false);
    }

    public void MainMenu()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Time.timeScale = 1f;
        SaveManager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        SaveManager.Instance.SaveGame();
        Application.Quit();
    }
}
