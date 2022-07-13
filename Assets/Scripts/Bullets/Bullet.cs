using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0, 1)]
    public float range = 1f;
    public float speed = 1f;
    public GameObject afterDestroySound;
    public GameObject destroyGameObject;
    public Transform centerOfSeeking;
    public bool isSeeker = false;
    public float seekRange = 2f;
    public LayerMask enemyLayer;
    public bool isTrueBullet = false;

    private int damage = 10;
    


    private Vector2 moveDir;
    private void Start()
    {
        Destroy(gameObject, range);
    }

    private void Update()
    {
        if (isSeeker){
            GameObject closestEnemy = null;
            float closestDistance = 999f;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(centerOfSeeking.position, seekRange, enemyLayer);
            if (enemies.Length > 0)
            {
                foreach (Collider2D enemy in enemies)
                {
                    if (Mathf.Abs((enemy.transform.position - this.transform.position).magnitude) < closestDistance)
                    {
                        closestDistance = (enemy.transform.position - this.transform.position).magnitude;
                        closestEnemy = enemy.gameObject;
                    }
                }
                transform.Translate((closestEnemy.transform.position - transform.position) * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(moveDir * speed * Time.deltaTime);
            }
            
        }
        else
        {
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "coinContainer")
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coinContainer"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

            if (collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
            Instantiate(destroyGameObject, transform.position, Quaternion.identity);
        }

        if (collision.transform.tag == "Enemy" || collision.transform.tag == "sparring")
        {
            if (collision.transform.GetComponent<Enemy>())
            {
                collision.transform.GetComponent<Enemy>().takeDamage(damage);
                Instantiate(destroyGameObject, transform.position, Quaternion.identity);
            }
        }

        if ((collision.transform.tag == "Bullet" || collision.transform.tag == "BulletHellBullet"))
        {
            
            if (SaveVariables.HOLY_STATUE != 2)
            {
                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else
            {
                if (collision.transform.CompareTag("BulletHellBullet"))
                {
                    collision.gameObject.SetActive(false);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
                Instantiate(destroyGameObject, transform.position, Quaternion.identity);
                Instantiate(afterDestroySound, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

        if (collision.transform.tag != "Bullet" && collision.transform.tag != "coinContainer" && collision.transform.tag != "BulletHellBullet" && collision.transform.tag != "Player" && collision.transform.tag != "EnemyZone" && collision.transform.tag != "Interactable" && collision.transform.tag != "SpawnPoint" && collision.transform.tag != "Shield" && collision.transform.tag != "PlayerBullet" && collision.transform.tag != "Collectable" && collision.transform.tag != "Coin" && collision.transform.tag != "Coin2" && collision.transform.tag != "Coin3" && collision.transform.tag != "Wood")
        {
            Instantiate(afterDestroySound, transform.position, Quaternion.identity);
            Instantiate(destroyGameObject, transform.position, Quaternion.identity);
            if (!isTrueBullet)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (centerOfSeeking != null)
        {
            Gizmos.DrawWireSphere(centerOfSeeking.position, seekRange);
        }
    }
}
