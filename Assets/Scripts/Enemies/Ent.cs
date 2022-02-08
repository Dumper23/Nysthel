using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent : Enemy
{
    public bool isRanged = false;
    public GameObject bullet;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        changeAnimationState("Idle");
        if(firePoint == null)
        {
            firePoint = transform;
        }
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
                    //triga en atacar per l'animació
                    Invoke("Shoot", 0.5f);
                }
            }
            else
            {
                changeAnimationState("Idle");
            }
        }
    }

    void Shoot()
    {
        changeAnimationState("Idle");
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
}
