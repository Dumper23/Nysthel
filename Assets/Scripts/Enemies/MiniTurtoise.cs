using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTurtoise : Enemy
{
    private Vector2 moveDir;
    public float dieTime = 10f;

    private void Start()
    {
        moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Destroy(gameObject, dieTime);
    }

    private void Update()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
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
