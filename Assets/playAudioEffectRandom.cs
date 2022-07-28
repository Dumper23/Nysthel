using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudioEffectRandom : MonoBehaviour
{
    public float start = 0, end = 0.25f;

    private void Start()
    {
        Invoke("playA", Random.Range(start, end));
    }

    private void playA()
    {
        GetComponent<AudioSource>().Play();
    }
}