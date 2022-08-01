using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead : Enemy
{
    private SpriteRenderer sprite;
    public GameObject zombieDead;

    private AudioSource audioSource;
    private float timeToStep = 0.2f;
    private float time = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = FindObjectOfType<Player>().transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            if (target != null && target.gameObject.GetComponent<Player>() && target.gameObject.GetComponent<Player>().scare)
            {
                isScared = true;
            }
            else
            {
                isScared = false;
            }
            time += Time.deltaTime;
            if (time >= timeToStep)
            {
                time = 0;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
            if (target.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

            if (health <= 0)
            {
                Instantiate(zombieDead, transform.position, Quaternion.identity);
                die();
            }
            Seek();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
        }
    }
}