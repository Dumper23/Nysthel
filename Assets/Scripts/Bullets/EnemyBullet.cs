using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public bool isSeeker = false;
    public float timeAlive = 3f;
    public int damage = 10;
    public float timeToWait = 0f;

    private Transform target;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private bool readyToMove = false;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("EnemyBullet"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Decoration"), LayerMask.NameToLayer("EnemyBullet"), true);
        if (isSeeker)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Destroy(gameObject, timeAlive);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isSeeker)
        {
            Invoke("readyToMoveToggle", timeToWait);
            if (readyToMove)
            {
                rb.velocity = ((target.position - transform.position).normalized * speed);
            }
        }
        else
        {
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
    }

    private void readyToMoveToggle()
    {
        readyToMove = true;
    }

    public void setMoveDirection(Vector2 v)
    {
        moveDir = v;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Bullet" || collision.transform.tag == "BulletHellBullet" || collision.transform.tag == "Enemy" || collision.transform.tag == "PlayerBullet")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if (collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}