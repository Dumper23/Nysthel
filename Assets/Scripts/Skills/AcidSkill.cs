using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSkill : MonoBehaviour
{
    public GameObject fire;
    public bool isInFire = false;
    public AudioClip fireSound;

    private void Start()
    {
        fire.SetActive(false);
        isInFire = false;
    }

    public void incendiate()
    {
        GetComponent<AudioSource>().clip = fireSound;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
        fire.SetActive(true);
        isInFire = true;
    }
}