using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellBullet : MonoBehaviour
{
    public int damage = 5;
    public float speed;
    public bool isGhost = false;

    private Vector2 moveDir;

    private void OnEnable()
    {
        Invoke("Destroy", 3f);
    }

    private void Start()
    {
        speed = 5f;
    }

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDir = dir;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
            Destroy();
        }

        if (collision.transform.tag == "Bullet" || collision.transform.tag == "BulletHellBullet" || collision.transform.tag == "PlayerBullet")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else if (collision.transform.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else if (collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
            Destroy();
        }
        else if (collision.transform.tag == "Shield")
        {
            Destroy();
        }
        else
        {
            if (!isGhost)
            {
                Destroy();
            }
        }
    }
}
