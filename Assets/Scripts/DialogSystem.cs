using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DialogSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogDisplay;
    public GameObject panel;
    public string[] sentences;
    public float typingSpeed = 0.02f;
    public GameObject continueButton;

    private bool canContinue = false;
    private float originialSpeed;
    private int index;
    private Interactable interactable;

    private void Start()
    {
        sentences = new string[1];
        originialSpeed = typingSpeed;
        panel.SetActive(false);
        dialogDisplay.text = "";
    }

    private void Update()
    {
        if (dialogDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueButton);
            Invoke("continueSentences", 0.75f);
        }

        if (canContinue)
        {
            if (Input.anyKey || Input.GetButton("Interact"))
            {
                NextSenctence();
            }
        }

        if (Input.anyKey || Input.GetButton("Interact"))
        {
            typingSpeed = 0.0035f;
        }
        else
        {
            typingSpeed = originialSpeed;
        }
    }

    private IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSenctence()
    {
        continueButton.SetActive(false);
        canContinue = false;
        if (index < sentences.Length - 1)
        {
            index++;
            dialogDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            dialogDisplay.text = "";
            continueButton.SetActive(false);
            GameStateManager.Instance.SetState(GameState.Gameplay);
            FindObjectOfType<PauseManager>().enabled = true;
            if (interactable != null)
            {
                interactable.enabled = true;
                interactable.interactionDialog.SetActive(true);
            }
            foreach (AudioSource a in FindObjectOfType<Player>().GetComponents<AudioSource>())
            {
                a.enabled = true;
            }
            panel.SetActive(false);
        }
    }

    private void continueSentences()
    {
        CancelInvoke("continueSentences");
        if (dialogDisplay.text == sentences[index])
        {
            canContinue = true;
        }
    }

    public void startDialog(string[] s, Interactable interactable)
    {
        index = 0;
        sentences = new string[s.Length + 1];
        this.interactable = interactable;
        for (int i = 0; i < sentences.Length; i++)
        {
            sentences[i] = "";
        }

        GameStateManager.Instance.SetState(GameState.Paused);
        FindObjectOfType<PauseManager>().enabled = false;
        foreach (AudioSource a in FindObjectOfType<Player>().GetComponents<AudioSource>())
        {
            a.enabled = false;
        }

        panel.SetActive(true);
        sentences = s;
        EventSystem.current.SetSelectedGameObject(continueButton);
        dialogDisplay.text = "";
        StartCoroutine(Type());
    }
}