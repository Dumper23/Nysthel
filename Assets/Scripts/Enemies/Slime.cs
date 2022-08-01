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

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        changeAnimationState("Idle");
        shotPatterns = new Vector2[4];
        shotPatterns[0] = new Vector2(1, 0);
        shotPatterns[1] = new Vector2(-1, 0);
        shotPatterns[2] = new Vector2(0, 1);
        shotPatterns[3] = new Vector2(0, -1);
    }

    private void Update()
    {
        if (activated)
        {
            if (target != null && target.gameObject.GetComponent<Player>() && target.gameObject.GetComponent<Player>().scare)
            {
                isScared = true;
            }
            else
            {
                isScared = false;
            }
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
                if (Time.time > nextShot && Vector3.Magnitude(target.position - transform.position) < range && !isScared)
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

    private void Shoot()
    {
        audioSourceAttack.clip = audios[0];
        audioSourceAttack.Play();
        int j = 0;
        for (int i = 0; i < shotsPerJump; i++)
        {
            GameObject go = Instantiate(bullet, firePoint.position + new Vector3(shotPatterns[j].x, shotPatterns[j].y), transform.rotation);
            Rigidbody2D r = go.GetComponent<Rigidbody2D>();
            go.GetComponent<EnemyBullet>().damage = damage;
            if (j < shotPatterns.Length)
            {
                r.GetComponent<EnemyBullet>().setMoveDirection(shotPatterns[j].normalized);
                j++;
            }
            else
            {
                j = 0;
                r.GetComponent<EnemyBullet>().setMoveDirection(shotPatterns[j].normalized);
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