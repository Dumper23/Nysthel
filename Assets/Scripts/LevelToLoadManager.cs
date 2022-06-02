using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelToLoadManager : MonoBehaviour
{
    public GameObject[] worlds;
    public GameObject loadingScreen;

    private void Start()
    {
        switch (SaveVariables.MAX_WORLD)
        {
            case 0:
                worlds[0].SetActive(true);
                worlds[1].SetActive(false);
                worlds[2].SetActive(false);
                worlds[3].SetActive(false);
                worlds[4].SetActive(false);
                break;
            case 1:
                worlds[0].SetActive(true);
                worlds[1].SetActive(true);
                worlds[2].SetActive(true);
                worlds[3].SetActive(false);
                worlds[4].SetActive(false);
                break;
            case 2:
                worlds[0].SetActive(true);
                worlds[1].SetActive(true);
                worlds[2].SetActive(true);
                worlds[3].SetActive(false);
                worlds[4].SetActive(false);
                break;
            case 3:
                worlds[0].SetActive(true);
                worlds[1].SetActive(true);
                worlds[2].SetActive(true);
                worlds[3].SetActive(true);
                worlds[4].SetActive(false);
                break;
            case 4:
                worlds[0].SetActive(true);
                worlds[1].SetActive(true);
                worlds[2].SetActive(true);
                worlds[3].SetActive(true);
                worlds[4].SetActive(true);
                break;
        }
    }

    public void loadNewLevel(string levelName)
    {
        loadingScreen.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Time.timeScale = 1f;
        switch (levelName)
        {
            case "Forest":
                SaveVariables.CURRENT_WORLD = 0;
                break;
            case "Ruins":
                SaveVariables.CURRENT_WORLD = 1;
                break;
            case "Mines":
                SaveVariables.CURRENT_WORLD = 2;
                break;
        }
        SaveManager.Instance.SaveGame();
        SceneManager.UnloadSceneAsync("Village");
        SceneManager.LoadSceneAsync(levelName);
    }
}
