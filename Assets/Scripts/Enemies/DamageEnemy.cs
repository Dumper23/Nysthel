using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : Enemy
{
    public bool isColumnContinuous = false;

    private float speed = 50;
    private Vector3 moveDir;
    public override void takeDamage(int value)
    {
        //Nothing
    }

    public void setDirection(Vector3 dir)
    {
        moveDir = dir.normalized;
    }

    private void Update()
    {
        if (isColumnContinuous) {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet"))
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
        }

        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
        }
        moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet"))
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
        }
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
        }
        moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
