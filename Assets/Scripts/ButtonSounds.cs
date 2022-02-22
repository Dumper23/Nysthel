using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioClip selectSound;
    public AudioClip pressSound;
    public AudioSource audioSourceSelect;
    public AudioSource audioSourcePress;

    public void SelectSound()
    {
        audioSourceSelect.clip = selectSound;
        audioSourceSelect.Play();
    }

    public void PressSound()
    {
        audioSourcePress.clip = pressSound;
        audioSourcePress.Play();
    }
}
