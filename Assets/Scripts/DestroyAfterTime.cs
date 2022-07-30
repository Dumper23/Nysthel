using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeToDestroy = 1f;
    public bool isSimple = false;
    public bool isPool = false;
    public bool isSkill = false;

    private Animator anim;
    private float startTime = 0;

    private void OnEnable()
    {
        startTime = Time.time;
    }

    private void Start()
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
            if (isSkill)
            {
                Invoke("desapear", timeToDestroy);
            }
            else
            {
                Destroy(gameObject, timeToDestroy);
            }
        }
    }

    private void desapear()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy.isInWater)
            {
                enemy.isInWater = false;
            }
            if (enemy.isInAcid)
            {
                enemy.isInAcid = false;
            }
        }
        Destroy(gameObject);
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
            if (Time.time >= (startTime + timeToDestroy))
            {
                destroy();
            }
        }
    }
}