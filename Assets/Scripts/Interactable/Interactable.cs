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

    public enum Interactions
    {
        OpenChest,
        EnterShop,
        GoToVillage,
        GoToAdventure
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
                        SceneManager.LoadScene("Village");
                    }
                    //Else fer un soroll per mostrar que no te diners i que no pot viatjar
                    break;

                case Interactions.GoToAdventure:
                    //Faltar triar a quin nivell viatjara en funcio del datafile o en funcio de la seva tria
                    PlayerPrefs.SetInt("gold", SaveVariables.PLAYER_GOLD);
                    SceneManager.LoadScene("Forest");
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (interaction)
            {
                case Interactions.GoToVillage:
                    text.SetText("Press X to go to the Village by 30% of your gold. (" + Mathf.RoundToInt(SaveVariables.PLAYER_GOLD * 0.3f) + ")");
                    break;
                case Interactions.GoToAdventure:
                    text.SetText("Press X to go to the Forest. You have " + SaveVariables.PLAYER_GOLD);
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
        }
    }
}
