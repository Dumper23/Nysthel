using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    private void Update()
    {
        tempTime += Time.deltaTime;
    }

    public void showStatistics()
    {
        if (statisticsText != null && statisticsUI != null)
        {
            EventSystem.current.SetSelectedGameObject(button);
            statisticsText.SetText(
                  "-Gold collected: " + goldCollected + "\n"
                + "-Dashes done: " + dashesDone + "\n"
                + "-Attacks done: " + attacksDone + "\n"
                + "-Time of the run: " + Mathf.FloorToInt(tempTime / 60) + "m " + Mathf.RoundToInt(tempTime - (Mathf.FloorToInt(tempTime / 60)*60))  + "s\n"
                + "-Enemies killed: " + enemiesKilled);
            statisticsUI.SetActive(true);
        }
    }

    public void goToVillage()
    {
        Time.timeScale = 1f;
        GameStateManager.Instance.SetState(GameState.Gameplay);
        SceneManager.LoadScene("Village");
    }
}
