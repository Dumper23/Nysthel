using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0, 1)]
    public float range = 1f;
    public float speed = 1f;

    private int damage = 10;


    private Vector2 moveDir;
    private void Start()
    {
        Destroy(gameObject, range);
    }

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    public void setDirection(Vector2 dir)
    {
        moveDir = dir;
    }

    public void setRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void setDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
        }

        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().takeDamage(damage);
        }

        if (collision.transform.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.transform.tag != "Player" && collision.transform.tag != "EnemyZone" && collision.transform.tag != "Interactable" && collision.transform.tag != "SpawnPoint" && collision.transform.tag != "Shield" && collision.transform.tag != "PlayerBullet" && collision.transform.tag != "Collectable" && collision.transform.tag != "Coin")
        {
            Destroy(gameObject);
        }
    }

}
