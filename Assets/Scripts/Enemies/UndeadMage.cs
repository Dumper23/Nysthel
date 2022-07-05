using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMage : Enemy
{
    public GameObject undeadMinion;
    [Range(1, 4)]
    public int minionQuantity;
    public int maxMinionSpawned = 24;
    public GameObject deadSound;

    private SpriteRenderer sprite;
    private int minionsSpawned = 0;
    private AudioSource audioSource;
    private float timeToSound = 1f;
    private float timeRandom;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<Player>().transform;
        audioSource = GetComponent<AudioSource>();
        timeRandom = Random.Range(0.8f, 2f);
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            

            minionsSpawned = FindObjectsOfType<Undead>().Length;

            if (health <= 0)
            {
                Instantiate(deadSound, transform.position, Quaternion.identity);
                die();
            }
            Seek();

            if (target.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }


            if (Time.time > nextShot && Vector3.Magnitude(target.position - transform.position) < range)
            {
                timeToSound += Time.time;
                if (timeToSound >= timeRandom)
                {
                    timeRandom = Random.Range(0.5f, 4f);
                    timeToSound = 0;
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.Play();
                }
                nextShot = Time.time + attackRate;
                if (minionsSpawned < maxMinionSpawned) {
                    Spawn();
                }
            }
        }
    }

    private void Spawn()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        if (minionQuantity == 1)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
        }
        else if(minionQuantity == 2)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(0, -1), Quaternion.identity);
        }
        else if(minionQuantity == 3)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(0, -1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(1, 0), Quaternion.identity);
        }else if(minionQuantity == 4)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(0, -1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(1, 0), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(-1, 0), Quaternion.identity);
        }
    }
}
