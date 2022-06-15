using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead : Enemy
{
    private SpriteRenderer sprite;

    void Start()
    {
        target = FindObjectOfType<Player>().transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            if (target.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

            die();
            Seek();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Debug.Log("xocant");
        }
    }
}
