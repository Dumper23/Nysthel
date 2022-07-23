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
    public GameObject QuitAdvertise;
    public float minZoom = 35;
    public float maxZoom = 150;

    private Player player;
    public bool quit = false;
    public bool menu = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        if(SaveVariables.PLAYER_USING_CONTROLLER == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    private void Update()
    {
        if (SaveVariables.PLAYER_USING_CONTROLLER == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        if (quit)
        {
            GameStateManager.Instance.SetState(GameState.Paused);
            if (Input.GetButtonUp("Interact"))
            {
                GameStateManager.Instance.SetState(GameState.Gameplay);
                Time.timeScale = 1f;
                Application.Quit();
            }
            if (Input.GetButtonUp("Cancel") || Input.GetKeyUp(KeyCode.Escape))
            {
                GameStateManager.Instance.SetState(GameState.Gameplay);
                quit = false;
                QuitAdvertise.SetActive(false);
            }
        }
        if (menu)
        {
            GameStateManager.Instance.SetState(GameState.Paused);

            if (Input.GetButtonUp("Interact"))
            {
                GameStateManager.Instance.SetState(GameState.Gameplay);
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
            if (Input.GetButtonUp("Cancel") || Input.GetKeyUp(KeyCode.Escape))
            {
                GameStateManager.Instance.SetState(GameState.Gameplay);
                menu = false;
                QuitAdvertise.SetActive(false);
            }
        }

        if (player.isDead)
        {
            this.enabled = false;
        }
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused)
        {
            Cursor.visible = true;
            player.audioSource[0].Pause();
            player.audioSource[1].Pause();
            player.audioSource[2].Pause();
        }
        else
        {
            Cursor.visible = false;
            player.audioSource[0].UnPause();
            player.audioSource[1].UnPause();
            player.audioSource[2].UnPause();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameStateManager.Instance.SetState(GameState.Gameplay);
            if (!player.timeSlowed)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0.5f;
            }
            pauseUi.SetActive(false);
            QuitAdvertise.SetActive(false);
            inventoryUi.SetActive(false);
        }

        if (Input.GetButtonDown("Pause") && !player.inShop)
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
                if (!player.timeSlowed)
                {
                    Time.timeScale = 1f;
                }
                else
                {
                    Time.timeScale = 0.5f;
                }
                pauseUi.SetActive(false);
                QuitAdvertise.SetActive(false);
            }
            else
            {
                pauseUi.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        if (Input.GetAxisRaw("Map") != 0)
        {
            Map.transform.localScale = new Vector3(4, 4, 4);
            minimapCam.orthographicSize = maxZoom;
        }
        else
        {
            Map.transform.localScale = new Vector3(2, 2, 2);
            minimapCam.orthographicSize = minZoom;
        }

        if (Input.GetButtonDown("Inventory"))
        {
            pauseUi.SetActive(false);
            QuitAdvertise.SetActive(false);
            inventoryUi.GetComponent<UIInventory>().navigationRefresh();

            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);

            if (newGameState == GameState.Gameplay)
            {
                if (!player.timeSlowed)
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
        if (!player.timeSlowed)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.5f;
        }
        pauseUi.SetActive(false);
        QuitAdvertise.SetActive(false);
    }

    public void MainMenu()
    {
        pauseUi.SetActive(false);
        QuitAdvertise.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Paused);
        Time.timeScale = 1;
        Invoke("setMenu", 0.3f);
    }

    public void Quit()
    {
        pauseUi.SetActive(false);
        QuitAdvertise.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Paused);
        Time.timeScale = 1;
        Invoke("setQuit", 0.3f);
    }

    private void setQuit()
    {
        menu = false;
        quit = true;
    }

    private void setMenu()
    {
        quit = false;
        menu = true;
    }

    public void usingControllerToggle()
    {
        player.usingController = toggle.isOn;
        SaveVariables.PLAYER_USING_CONTROLLER = toggle.isOn ? 1 : 0;
        /*if (player.usingController)
        {
            toggle.isOn = false;
            player.usingController = false;
            SaveVariables.PLAYER_USING_CONTROLLER = 0;

        }
        else
        {
            toggle.isOn = true;
            player.usingController = true;
            SaveVariables.PLAYER_USING_CONTROLLER = 1;
        }*/
    }
}