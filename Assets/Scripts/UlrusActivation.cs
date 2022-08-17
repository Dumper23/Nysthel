using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UlrusActivation : MonoBehaviour
{
    public GameObject barrier;
    public GameObject Ulrus;
    public AudioClip start;
    public int camZoom = 60;
    public int originalCamZoom = 108;

    private AudioSource a;
    private bool done = false;

    private void Start()
    {
        a = GetComponent<AudioSource>();
        Ulrus.GetComponent<BOSS_Ulrus>().activated = false;
        Ulrus.SetActive(false);
        barrier.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !done)
        {

            Statistics.Instance.isInBoss = true;
            Camera.main.GetComponent<Animator>().Play("zoomOut");
            
            done = true;
            a.clip = start;
            a.Play();
            Ulrus.SetActive(true);
            Ulrus.GetComponent<BOSS_Ulrus>().activate();
            Ulrus.GetComponent<BOSS_Ulrus>().changeAnimationState("teleportArrive");
            barrier.SetActive(true);
            Destroy(gameObject, 5);
        }
    }
}
