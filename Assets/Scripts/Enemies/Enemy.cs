using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Enemy : MonoBehaviour
{
    public int health = 20;
    public int damage = 10;
    public int maxGoldToGive = 5;
    public int minGoldToGive = 0;
    public float moveSpeed = 2f;
    
    public Rigidbody2D rb;
    public Animator anim;
    public Transform firePoint;
    public float attackRate = 1f;
    public float range = 2f;
    public GameObject coin;
    public float coinForce = 2f;
    public GameObject bloodStain;
    public GameObject bloodParticles;
    public bool immune = false;

    protected Transform target;
    protected int goldToGive;
    protected string currentState;
    protected float nextShot = 0f;
    protected int startHealth;
    public bool activated = false;

    private void Start()
    {
        startHealth = health;
    }

    public virtual void takeDamage(int value)
    {
        if (activated && !immune)
        {
            health -= value;
        }
    }

    public void enemyActivation(bool activate)
    {
        activated = activate;
    }

    protected void die()
    {
        if (health <= 0)
        {
            Statistics.Instance.enemiesKilled += 1;
            if (bloodStain != null)
            {
                Instantiate(bloodStain, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
            if(bloodParticles != null)
            {
                Instantiate(bloodParticles, transform.position, Quaternion.Euler(90, 0, 0));
            }
            int g = Random.Range(minGoldToGive, maxGoldToGive);
            if (g < 0) g = 0;
            goldToGive = g;
            if (g > 0)
            {
                for (int i = 0; i <= goldToGive; i++)
                {
                    GameObject go = Instantiate(coin, transform.position, Quaternion.identity);
                    go.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * coinForce, ForceMode2D.Impulse);

                }
            }
            Destroy(gameObject);
        }
    }

    protected bool Seek()
    {
        bool inRange = false;
        if (activated)
        {
            if (Vector3.Magnitude(target.position - transform.position) < range)
            {
                inRange = true;
                transform.Translate((target.position - transform.position).normalized * moveSpeed * Time.fixedDeltaTime);
                changeAnimationState("Walk");
            }
            else
            {
                inRange = false;
                changeAnimationState("Idle");
            }
        }
        return inRange;
    }
    protected void changeAnimationState(string newState)
    {
        //We avoid playing the same animation multiple times
        if (currentState == newState) return;

        //We play a determinated animation
        anim.Play(newState);

        currentState = newState;
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (collision.transform.GetComponent<Player>().isImmune())
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
            }
            else
            {
                collision.transform.GetComponent<Player>().takeDamage(damage);
            }
        }
    }
}
