using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent : Enemy
{
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        die();

        if(target.position.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

    }

    private void FixedUpdate()
    {
        Seek();
    }


    void Seek()
    {
        if(Vector3.Magnitude(target.position - transform.position) < range)
        {
            transform.Translate((target.position-transform.position).normalized * moveSpeed * Time.fixedDeltaTime);
        }
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
