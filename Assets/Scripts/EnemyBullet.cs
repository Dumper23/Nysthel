using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public bool isSeeker = false;
    
    public float timeAlive = 3f;

    public int damage = 10;

    private Transform target;
    private Rigidbody2D rb;


    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("EnemyBullet"), true);
        if (isSeeker)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Destroy(gameObject, timeAlive);
        rb = GetComponent<Rigidbody2D>();
        
    }



    void Update()
    {
        if (isSeeker)
        {
            rb.velocity = ((target.position - transform.position).normalized * speed);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
            Destroy(gameObject);
        }

        if(collision.transform.tag == "PlayerBullet")
        {
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Bullet" || collision.transform.tag == "BulletHellBullet" || collision.transform.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else if(collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            Destroy(gameObject);
        }
    }
}
