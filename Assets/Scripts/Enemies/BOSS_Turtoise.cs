using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BOSS_Turtoise : Enemy
{
    public float dashForce = 5f;
    public float shotAngleIncrement = 10f;
    public GameObject miniTurtle;
    public GameObject deathSound;
    public GameObject bullet;
    public float bulletForce = 10f;
    public GameObject villagePortal;
    public AudioSource audioSource;
    public AudioSource screamSource;
    public AudioClip scream;
    public GameObject levelCompletedUi;
    public AudioSource AttacksSource;
    public AudioClip[] audios;

    private static int SHOOT_AUDIO = 0;
    private static int DASH_AUDIO = 1;
    private static int COLLISION_AUDIO = 2;

    private float angle = 0f;
    private string[] state;
    private bool inAction = false;
    private string cState;
    private Vector2 moveDir;
    private float nextTurtle = 0f;
    private bool canDash = false;

    private bool phase2 = false;
    private bool shot = false;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Decoration"), LayerMask.NameToLayer("Enemy"));
        startHealth = health;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = new string[8];
        state[0] = "dash";
        state[1] = "shield";
        state[2] = "idle";
        state[4] = "dash";
        state[5] = "dash";
        state[6] = "idle";
        state[7] = "idle";

        cState = state[2];
        moveDir = (target.position - transform.position).normalized * dashForce;
    }

    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            if (!audioSource.isPlaying) audioSource.Play();

            if (health <= Mathf.RoundToInt(startHealth / 2))
            {
                phase2 = true;
            }

            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (health <= 0)
            {
                Instantiate(villagePortal, transform.position, Quaternion.identity);
                Instantiate(levelCompletedUi, target);
                Instantiate(deathSound);
                die();
            }

            if (!inAction)
            {
                Invoke("ChangeState", Random.Range(2, 5));
                inAction = true;
            }

            if (!shot)
            {
                Invoke("Shoot", Random.Range(1, 4));
                shot = true;
            }

            switch (cState)
            {
                case "dash":
                    changeAnimationState("shield");
                    Invoke("enableDash", 0.75f);
                    if (canDash)
                    {
                        changeAnimationState("dash");
                        dash();
                    }
                    else
                    {
                        AttacksSource.pitch = 1.5f;
                        AttacksSource.clip = audios[DASH_AUDIO];
                        AttacksSource.Play();
                    }
                    immune = true;
                    break;

                case "shield":
                    changeAnimationState("shield");
                    immune = true;
                    break;

                case "idle":
                    changeAnimationState("idle");
                    immune = false;
                    break;

                default:
                    changeAnimationState("idle");
                    immune = false;
                    break;
            }
        }
    }

    private void enableDash()
    {
        canDash = true;
    }

    private void ChangeState()
    {
        canDash = false;
        cState = state[Random.Range(0, state.Length - 1)];
        moveDir = (target.position - transform.position).normalized * dashForce;
        inAction = false;
    }

    private void Shoot()
    {
        AttacksSource.clip = audios[SHOOT_AUDIO];
        AttacksSource.Play();
        shot = false;
        angle = 0;
        while (angle <= 350)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.Instance.GetBullet();
            //bul.GetComponent<SpriteRenderer>().sprite = bulletSprite;

            bul.transform.position = firePoint.position;
            bul.transform.rotation = firePoint.rotation;

            bul.SetActive(true);
            bul.GetComponent<BulletHellBullet>().SetMoveDirection(bulDir);

            angle += shotAngleIncrement;
        }
    }

    private void dash()
    {
        transform.Translate(moveDir * Time.deltaTime);

        if (phase2)
        {
            if (Time.time > nextTurtle)
            {
                nextTurtle = Time.time + attackRate;
                Instantiate(miniTurtle, transform.position, Quaternion.identity);
            }
        }
    }

    public override void takeDamage(int value)
    {
        GameObject go = Instantiate(damageNumbers, transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0), Quaternion.identity) as GameObject;

        if (activated && !immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- " + value);
            }
            Instantiate(bloodParticles, transform.position, Quaternion.Euler(90, 0, 0));
            screamSource.clip = scream;
            screamSource.Play();
            health -= value;
            cState = "shield";
        }

        if (immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Immune");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("EnemyZone"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
        }
        else
        {
            Statistics.Instance.shake();
            AttacksSource.clip = audios[COLLISION_AUDIO];
            AttacksSource.Play();
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<Player>().takeDamage(damage);
                cState = "shield";
            }
            else
            {
                moveDir = (target.position - transform.position).normalized * dashForce;
            }
        }
    }
}