using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Enemy
{
    public float movementTime = 2f;
    public float shootTime = 2f;
    public float movingProbability = 0.25f;
    public float animationTime = 0.3f;
    public float bulletSpeed = 5f;
    public GameObject pointOfLight;
    public ParticleSystem groundParticles;
    public AudioClip[] audios;
    public GameObject deathSound;

    private static int ATTACK_AUDIO = 0;
    private static int HIDE_AUDIO = 1;
    private static int UNHIDE_AUDIO = 2;
    private AudioSource audioSource;
    private float time = 0;
    private float timeMoving = 2f;
    private bool move = false;
    private bool isMoving = false;
    private bool isShooting = false;
    private Vector2 moveDir;
    private bool startedShooting = false, startedMoving = false;

    private bool lastIsMoving = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        groundParticles.Stop();
        target = FindObjectOfType<Player>().transform;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("Enemy"));
        if (Random.Range(0f, 1f) >= movingProbability)
        {
            move = false;
        }
        else
        {
            move = true;
        }
    }

    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
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
                Instantiate(deathSound, transform.position, Quaternion.identity);
                die();
            }
            if ((target.position - transform.position).magnitude <= range)
            {
                if (move)
                {
                    pointOfLight.SetActive(false);
                    if (!isMoving)
                    {
                        groundParticles.Play();
                        if (!lastIsMoving)
                        {
                            audioSource.pitch = Random.Range(0.75f, 1.15f);
                            audioSource.clip = audios[HIDE_AUDIO];
                            audioSource.Play();
                            anim.Play("gettingIn");
                        }

                        if (!isScared)
                        {
                            moveDir = (target.position - transform.position).normalized;
                        }
                        else
                        {
                            moveDir = (-target.position + transform.position).normalized;
                        }
                        Invoke("stopMoving", timeMoving);
                        Invoke("startMoving", animationTime);
                    }
                    if (startedMoving && !isFrozen)
                    {
                        immune = true;
                        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
                    }
                    isMoving = true;
                }
                else
                {
                    pointOfLight.SetActive(true);
                    rb.velocity = Vector2.zero;
                    if (!isShooting && !isFrozen)
                    {
                        groundParticles.Stop();
                        if (lastIsMoving)
                        {
                            anim.Play("gettingOut");
                        }
                        Invoke("stopShooting", shootTime);
                        Invoke("startShooting", animationTime);
                        isShooting = true;
                    }
                    if (startedShooting)
                    {
                        time += Time.deltaTime;
                        if (time >= attackRate && !isScared)
                        {
                            time = 0;
                            shoot();
                        }
                    }
                }
            }
        }
    }

    private void startShooting()
    {
        startedShooting = true;
    }

    private void startMoving()
    {
        startedMoving = true;
    }

    private void shoot()
    {
        audioSource.pitch = Random.Range(0.75f, 1.15f);
        audioSource.clip = audios[ATTACK_AUDIO];
        audioSource.Play();
        GameObject bul = BulletPool.Instance.GetBullet();
        bul.GetComponent<BulletHellBullet>().damage = damage;
        bul.GetComponent<BulletHellBullet>().speed = bulletSpeed;
        bul.gameObject.SetActive(true);
        bul.transform.position = firePoint.position;
        bul.transform.rotation = firePoint.rotation;
        bul.GetComponent<BulletHellBullet>().SetMoveDirection((target.position - transform.position).normalized);
    }

    private void stopShooting()
    {
        isShooting = false;
        isMoving = false;
        startedShooting = false;
        lastIsMoving = false;
        if (Random.Range(0f, 1f) >= movingProbability)
        {
            move = false;
        }
        else
        {
            move = true;
        }
    }

    private void stopMoving()
    {
        audioSource.pitch = Random.Range(0.75f, 1.15f);
        audioSource.clip = audios[UNHIDE_AUDIO];
        audioSource.Play();
        immune = false;
        isShooting = false;
        isMoving = false;
        startedShooting = false;
        lastIsMoving = true;
        if (Random.Range(0f, 1f) >= movingProbability)
        {
            move = false;
        }
        else
        {
            move = true;
        }
    }
}