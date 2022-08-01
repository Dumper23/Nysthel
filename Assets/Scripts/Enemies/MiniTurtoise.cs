using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTurtoise : Enemy
{
    private Vector2 moveDir;
    public float dieTime = 10f;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Destroy(gameObject, dieTime);
    }

    private void Update()
    {
        if (target != null && target.gameObject.GetComponent<Player>() && target.gameObject.GetComponent<Player>().scare)
        {
            isScared = true;
        }
        else
        {
            isScared = false;
        }
        if (!isScared)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate((-target.position + transform.position).normalized * moveSpeed * Time.deltaTime);
        }
        die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
        }
        moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}