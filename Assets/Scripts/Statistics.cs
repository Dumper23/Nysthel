using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [HideInInspector]
    public int goldCollected, dashesDone, attacksDone, enemiesKilled;

    [HideInInspector]
    public float timeSpent;

    private float tempTime;

    public TextMeshProUGUI statisticsText;
    public GameObject statisticsUI;
    public GameObject button;
    public GameObject loadingScreen;
    public GameObject cameraContainer;

    public static Statistics Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        goldCollected = 0;
        dashesDone = 0;
        attacksDone = 0;
        timeSpent = 0;
        enemiesKilled = 0;
    }

    public void shake()
    {
        cameraContainer.GetComponent<Animator>().Play("shake");
    }

    private void Update()
    {
        tempTime += Time.deltaTime;
    }

    public void showStatistics()
    {
        if (statisticsText != null && statisticsUI != null)
        {
            EventSystem.current.SetSelectedGameObject(button);
            if (SceneManager.GetActiveScene().name.Equals("GoldRush"))
            {
                if (enemiesKilled > SaveVariables.KILLS_GOLD_RUSH)
                {
                    SaveVariables.KILLS_GOLD_RUSH = enemiesKilled;
                }
                if (tempTime > SaveVariables.SECONDS_GOLD_RUSH)
                {
                    SaveVariables.SECONDS_GOLD_RUSH = tempTime;
                }
            }
            statisticsText.SetText(
                  "-Gold collected: " + goldCollected + "\n"
                + "-Dashes done: " + dashesDone + "\n"
                + "-Attacks done: " + attacksDone + "\n"
                + "-Time of the run: " + Mathf.FloorToInt(tempTime / 60) + "m " + Mathf.RoundToInt(tempTime - (Mathf.FloorToInt(tempTime / 60) * 60)) + "s\n"
                + "-Enemies killed: " + enemiesKilled);
            statisticsUI.SetActive(true);
        }
    }

    public void goToVillage()
    {
        Time.timeScale = 0f;
        GameStateManager.Instance.SetState(GameState.Paused);
        loadingScreen.SetActive(true);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync("Village");
    }
}