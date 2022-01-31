using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{

    public bool isRanged = false;
    public GameObject bullet;
    public float bulletForce = 10f;

    private int shotsPerJump = 4;
    private float nextShot = 0f;
    private Vector2[] shotPatterns;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        changeAnimationState("Idle");
        shotPatterns = new Vector2[4];
        shotPatterns[0] = new Vector2(1, 0);
        shotPatterns[1] = new Vector2(-1, 0);
        shotPatterns[2] = new Vector2(0, 1);
        shotPatterns[3] = new Vector2(0, -1);
    }

    void Update()
    {
        die();

        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        if (isRanged)
        {
            if (Time.time > nextShot && Vector3.Magnitude(target.position - transform.position) < range)
            {
                nextShot = Time.time + attackRate;
                Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        Seek();
    }

    void Shoot()
    {
        int j = 0;
        for (int i = 0; i < shotsPerJump; i++)
        {
            GameObject go = Instantiate(bullet, firePoint.position + new Vector3(shotPatterns[j].x, shotPatterns[j].y), transform.rotation);
            Rigidbody2D r = go.GetComponent<Rigidbody2D>();
            if (j < shotPatterns.Length)
            {
                r.AddForce(shotPatterns[j].normalized * bulletForce, ForceMode2D.Impulse);
                j++;
            }
            else
            {
                j = 0;
                r.AddForce(shotPatterns[j].normalized * bulletForce, ForceMode2D.Impulse);
            }
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
}
