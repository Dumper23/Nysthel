using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public Interactions interaction;
    public GameObject interactionDialog;
    public TextMeshProUGUI text;
    public UiShop uiShop;

    private Player player;
    private bool inShop = false;

    public enum Interactions
    {
        OpenChest,
        EnterShop,
        GoToVillage,
        GoToAdventure,
        EnterBlackSmith,
        Save
    };

    private bool inRange = false;

    private void Update()
    {
        if (inRange && Input.GetButtonDown("Interact"))
        {
            //Functionality of the interaction
            switch (interaction)
            {
                case Interactions.GoToVillage:
                    if (SaveVariables.PLAYER_GOLD - Mathf.RoundToInt(SaveVariables.PLAYER_GOLD * 0.3f) >= 0)
                    {
                        SaveVariables.PLAYER_GOLD -= Mathf.RoundToInt(SaveVariables.PLAYER_GOLD * 0.3f);
                        PlayerPrefs.SetInt("gold", SaveVariables.PLAYER_GOLD);
                        PlayerPrefs.SetInt("attack", SaveVariables.PLAYER_ATTACK);
                        PlayerPrefs.SetInt("life", SaveVariables.PLAYER_LIFE);
                        PlayerPrefs.SetFloat("speed", SaveVariables.PLAYER_SPEED);
                        PlayerPrefs.SetFloat("attackSpeed", SaveVariables.PLAYER_ATTACK_SPEED);
                        PlayerPrefs.SetFloat("range", SaveVariables.PLAYER_RANGE);
                        PlayerPrefs.SetFloat("dashRecovery", SaveVariables.PLAYER_DASH_RECOVERY);
                        PlayerPrefs.SetFloat("dashRange", SaveVariables.PLAYER_DASH_RANGE);
                        SceneManager.LoadScene("Village");
                    }
                    //Else fer un soroll per mostrar que no te diners i que no pot viatjar
                    break;

                case Interactions.GoToAdventure:
                    //Faltar triar a quin nivell viatjara en funcio del datafile o en funcio de la seva tria
                    PlayerPrefs.SetInt("gold", SaveVariables.PLAYER_GOLD);
                    PlayerPrefs.SetInt("attack", SaveVariables.PLAYER_ATTACK);
                    PlayerPrefs.SetInt("life", SaveVariables.PLAYER_LIFE);
                    PlayerPrefs.SetFloat("speed", SaveVariables.PLAYER_SPEED);
                    PlayerPrefs.SetFloat("attackSpeed", SaveVariables.PLAYER_ATTACK_SPEED);
                    PlayerPrefs.SetFloat("range", SaveVariables.PLAYER_RANGE);
                    PlayerPrefs.SetFloat("dashRecovery", SaveVariables.PLAYER_DASH_RECOVERY);
                    PlayerPrefs.SetFloat("dashRange", SaveVariables.PLAYER_DASH_RANGE);
                    SceneManager.LoadScene("Forest");
                    break;

                case Interactions.EnterBlackSmith:
                    if (!inShop)
                    {
                        uiShop.show(player);
                        inShop = true;
                        Time.timeScale = 0f;
                        GameStateManager.Instance.SetState(GameState.Paused);
                    }
                    break;

                case Interactions.Save:
                    //Save
                    PlayerPrefs.SetInt("gold", SaveVariables.PLAYER_GOLD);
                    PlayerPrefs.SetInt("attack", SaveVariables.PLAYER_ATTACK);
                    PlayerPrefs.SetInt("life", SaveVariables.PLAYER_LIFE);
                    PlayerPrefs.SetFloat("speed", SaveVariables.PLAYER_SPEED);
                    PlayerPrefs.SetFloat("attackSpeed", SaveVariables.PLAYER_ATTACK_SPEED);
                    PlayerPrefs.SetFloat("range", SaveVariables.PLAYER_RANGE);
                    PlayerPrefs.SetFloat("dashRecovery", SaveVariables.PLAYER_DASH_RECOVERY);
                    PlayerPrefs.SetFloat("dashRange", SaveVariables.PLAYER_DASH_RANGE);
                    break;
            }
        }

        if ((Input.GetButtonDown("Pause") || Input.GetButtonDown("Inventory") || Input.GetButtonDown("Cancel")) && inShop)
        {
            Time.timeScale = 1f;
            GameStateManager.Instance.SetState(GameState.Gameplay);
            uiShop.hide();
            inShop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
            switch (interaction)
            {
                case Interactions.GoToVillage:
                    text.SetText("Press X to go to the Village by 30% of your gold. (" + Mathf.RoundToInt(SaveVariables.PLAYER_GOLD * 0.3f) + ")");
                    break;
                case Interactions.GoToAdventure:
                    text.SetText("Press X to go to the Forest");
                    break;
                case Interactions.EnterBlackSmith:
                    text.SetText("Press X to talk with the BlackSmith");
                    break;
                case Interactions.Save:
                    text.SetText("Press X to save your progress.");
                    break;
            }
            
            interactionDialog.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionDialog.SetActive(false);
            inRange = false;
            if (inShop)
            {
                inShop = false;
                uiShop.hide();
                Time.timeScale = 1f;
                GameStateManager.Instance.SetState(GameState.Gameplay);
            }
        }
    }
}
