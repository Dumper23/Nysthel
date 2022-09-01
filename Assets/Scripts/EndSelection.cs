using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSelection : MonoBehaviour
{
    public AudioSource music;
    public AudioClip audioClip;
    public AudioSource ownAudio;
    public AudioClip select;
    public AudioClip press;

    private bool pressed = false;


    public void startNewMusic()
    {
        Invoke("changeScene", 4.5f);
       /* music.clip = audioClip;
        music.volume = 0.8f;
        music.Play();*/
    }

    public void killUlrus()
    {
        //Final 3
        Invoke("changeScene", 2.5f);
    }

    public void SaveUlrus()
    {
        if(SaveVariables.HAS_EMMYR_ITEM == 1 && SaveVariables.EMMYR_STATUE == 2)
        {
            //Final 2
            Invoke("changeScene", 2.5f);
        }
        else
        {
            //Final 1
            Invoke("changeScene", 2.5f);
        }
    }

    private void changeScene()
    {
        SceneManager.LoadScene("Castle_Cinematic");
    }

    public void selectButton()
    {
        if (!pressed)
        {
            ownAudio.clip = select;
            ownAudio.Play();
        }
    }

    public void pressButton()
    {
        if (!pressed)
        {
            pressed = true;
            ownAudio.clip = press;
            ownAudio.Play();
        }
    }
}
