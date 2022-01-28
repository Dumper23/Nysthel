using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent : Enemy
{
    public bool isRanged = false;
    public GameObject bullet;

    private Transform target;
    private float nextShot = 0f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        changeAnimationState("Idle");
    }

    void Update()
    {
        die();

        if(target.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

    }

    private void FixedUpdate()
    {
        if (!isRanged)
        {
            Seek();
        }
        else
        {
            if (Vector3.Magnitude(target.position - transform.position) < range)
            {
                if (Time.time > nextShot)
                {
                    nextShot = Time.time + attackRate;
                    changeAnimationState("Attack");
                    Invoke("Shoot", 0.5f);
                }
            }
            else
            {
                changeAnimationState("Idle");
            }
        }
    }


    void Seek()
    {
        if(Vector3.Magnitude(target.position - transform.position) < range)
        {
            transform.Translate((target.position-transform.position).normalized * moveSpeed * Time.fixedDeltaTime);
            changeAnimationState("Walk");
        }
        else
        {
            changeAnimationState("Idle");
        }
    }

    void Shoot()
    {
        changeAnimationState("idle");
        Instantiate(bullet, firePoint.position, transform.rotation);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().takeDamage(damage);
        }

        if (collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
