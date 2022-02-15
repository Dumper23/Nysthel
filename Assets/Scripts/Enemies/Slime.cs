using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Slime : Enemy
{

    public bool isRanged = false;
    public GameObject bullet;
    public float bulletForce = 10f;

    public AudioClip[] audios;
    public GameObject afterDieSound;

    public AudioSource audioSourceAttack;
    public AudioSource audioSourceWalk;

    private int shotsPerJump = 4;
    private Vector2[] shotPatterns;

    private float nextSound = 0f;
    private float timeSounds = 0.6f;

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
        if (activated)
        {
            if (health <= 0)
            {
                Instantiate(afterDieSound, transform.position, Quaternion.identity);
                die();
            }

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
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            bool inRange = Seek();
            if (Time.time > nextSound && inRange)
            {
                nextSound = Time.time + timeSounds;
                audioSourceWalk.clip = audios[1];
                audioSourceWalk.pitch = Random.Range(0.7f, 1.3f);
                audioSourceWalk.Play();

            }
        }
    }

    void Shoot()
    {
        audioSourceAttack.clip = audios[0];
        audioSourceAttack.Play();
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
