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

    private float time = 0;
    private float timeMoving = 2f;
    private bool move = false;
    private bool isMoving = false;
    private bool isShooting = false;
    private Vector2 moveDir;
    private bool startedShooting = false, startedMoving = false;

    private bool lastIsMoving = false;

    void Start()
    {
        groundParticles.Stop();
        target = FindObjectOfType<Player>().transform;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("Enemy"));
        if (Random.Range(0f, 1f) >= movingProbability) {
            move = false;
        }
        else
        {
            move = true;
        }
    }

    void Update()
    {
        if(activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            die();
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
                            anim.Play("gettingIn");
                        }
                        moveDir = (target.position - transform.position).normalized;
                        Invoke("stopMoving", timeMoving);
                        Invoke("startMoving", animationTime);
                    }
                    if (startedMoving)
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
                    if (!isShooting)
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
                        if (time >= attackRate)
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
