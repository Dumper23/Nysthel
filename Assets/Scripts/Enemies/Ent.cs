using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Ent : Enemy
{
    public bool isRanged = false;
    public GameObject bullet;
    public AudioClip[] audios;
    public GameObject afterDieSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = FindObjectOfType<Player>().transform;
        changeAnimationState("Idle");
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    private void Update()
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
        }
    }

    private void FixedUpdate()
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
            if (!isRanged)
            {
                bool inRange = Seek();
                if (Time.time > nextShot && inRange && !isScared)
                {
                    nextShot = Time.time + 0.8f;
                    audioSource.clip = audios[0];
                    audioSource.Play();
                }
            }
            else
            {
                if (Vector3.Magnitude(target.position - transform.position) < range)
                {
                    if (Time.time > nextShot && !isScared)
                    {
                        audioSource.clip = audios[0];
                        audioSource.Play();
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
    }

    private void Shoot()
    {
        changeAnimationState("Idle");
        GameObject bul = Instantiate(bullet, firePoint.position, transform.rotation);
        bul.GetComponent<EnemyBullet>().damage = damage;
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