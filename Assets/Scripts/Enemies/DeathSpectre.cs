using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpectre : Enemy
{
    public float meleeAttackRange = 2f;
    public GameObject bullet;
    public float meleeAttackRate = 1f;
    public float meleeAttackDelay = 0.35f;
    public float treshold = 0.3f;
    public GameObject meleeAttackEffect;
    public bool isRanged = true;
    public CircleCollider2D cCollider;
    public AudioClip[] audios;
    public GameObject deadSound;

    private static int ATTACK_AUDIO = 0;
    private static int MELEE_AUDIO = 1;

    private AudioSource audioSource;
    private float xScale = 1, yScale = 1;
    private float xOffset = 0.39f;
    private bool isAttacking = false;
    private bool hasSounded = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {


            if ((target.position - transform.position).magnitude <= range)
            {
                if (target.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(xScale, yScale, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-xScale, yScale, 1);
                }

                if ((target.position - transform.position).magnitude > meleeAttackRange)    //Ranged
                {
                    if (isRanged)
                    {
                        if (Time.time > nextShot)
                        {
                            nextShot = Time.time + attackRate;
                            isAttacking = true;
                            anim.Play("rangedAttack");
                            Invoke("shoot", 0.75f);
                            cCollider.offset = new Vector2(0, 0);
                            xScale = 1;
                            yScale = 1;
                        }
                        else
                        {
                            hasSounded = false;
                        }
                    }
                    else
                    {
                        Seek();
                    }
                }
                else //Melee
                {
                    if (Time.time > nextShot)
                    {
                        nextShot = Time.time + meleeAttackRate;
                        isAttacking = true;
                        anim.Play("meleeAttack");
                        meleeAttackEffect.SetActive(true);
                        xScale = 2;
                        yScale = 2;
                        Invoke("meleeAttack", meleeAttackDelay);
                        Invoke("cancelAttack", 0.6f);
                    }
                    else
                    {
                        hasSounded = false;
                    }

                    if (!isAttacking)
                    {
                        xScale = 1;
                        yScale = 1;
                        cCollider.offset = new Vector2(0, 0);
                    }
                }

                if (!isAttacking)
                {
                    anim.Play("Walk");
                    xScale = 1;
                    yScale = 1;
                    cCollider.offset = new Vector2(0, 0);
                }
            }
            else
            {
                anim.Play("Idle");
                xScale = 1;
                yScale = 1;
                isAttacking = false;
            }


            if (!isAttacking)
            {
                cCollider.offset = new Vector2(0, 0);
                Seek();
            }

            if (health <= 0)
            {
                Instantiate(deadSound, transform.position, Quaternion.identity);
                die();
            }

        }
    }

    private void cancelAttack()
    {
        isAttacking = false;
        meleeAttackEffect.SetActive(false);
        xScale = 1;
        yScale = 1;
    }

    private void shoot()
    {
        EnemyBullet b = (Instantiate(bullet, firePoint.position, Quaternion.identity) as GameObject).GetComponent<EnemyBullet>();
        b.timeToWait = 0.5f;
        b = (Instantiate(bullet, firePoint.position, Quaternion.identity) as GameObject).GetComponent<EnemyBullet>();
        b.timeToWait = 1f;
        b = (Instantiate(bullet, firePoint.position, Quaternion.identity) as GameObject).GetComponent<EnemyBullet>();
        b.timeToWait = 2f;
        isAttacking = false;
        Invoke("sound", 0.5f);
        Invoke("sound", 1f);
        Invoke("sound", 2f);
    }

    private void sound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.clip = audios[ATTACK_AUDIO];
        audioSource.Play();
    }

    private void meleeAttack()
    {
        if (!hasSounded)
        {
            audioSource.clip = audios[MELEE_AUDIO];
            audioSource.Play();
            hasSounded = true;
        }

        if (new Vector2((target.position.x - transform.position.x), target.position.y - transform.position.y).magnitude < meleeAttackRange)
        {
            if (target.position.x > transform.position.x)
            {
                cCollider.offset = new Vector2((target.position.x - transform.position.x), target.position.y - transform.position.y) / (meleeAttackRange / 2);
            }
            else
            {
                cCollider.offset = new Vector2((target.position.x - transform.position.x) * -1, target.position.y - transform.position.y) / (meleeAttackRange / 2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
