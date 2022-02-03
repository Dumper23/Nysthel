using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public GameObject inventoryUi;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);
            if (newGameState == GameState.Gameplay)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }

        if (Input.GetButtonDown("Inventory"))
        {
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
}
