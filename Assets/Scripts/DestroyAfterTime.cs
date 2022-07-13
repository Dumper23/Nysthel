using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeToDestroy = 1f;
    public bool isSimple = false;
    public bool isPool = false;

    private Animator anim;
    private float startTime = 0;

    void Start()
    {
        if (!isSimple)
        {
            anim = GetComponent<Animator>();
            startTime = Time.time;
        }
        if (isPool)
        {
            Invoke("destroy", timeToDestroy);
        }
        else
        {
            Destroy(gameObject, timeToDestroy);
        }
    }


    private void destroy()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (transform.CompareTag("Coin") || transform.CompareTag("Coin2") || transform.CompareTag("Coin3"))
        {
            if (Time.time >= (startTime + timeToDestroy) - 0.4 * timeToDestroy)
            {
                anim.Play("CoinDesapear");
            }
        }
    }
}
