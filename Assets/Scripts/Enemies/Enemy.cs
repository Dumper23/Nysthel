using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int startHealth;
    private int fireDMG = 10;

    public Rigidbody2D rb;
    public Animator anim;
    public Transform firePoint;
    public float attackRate = 1f;
    public float range = 2f;
    public int coinType;
    public float coinForce = 2f;
    public GameObject bloodStain;
    public GameObject bloodParticles;
    public bool immune = false;
    public GameObject damageNumbers;
    public bool immuneToElementalEffects = false;

    protected Transform target;
    protected int goldToGive;
    protected string currentState;
    protected float nextShot = 0f;

    public bool activated = false;
    public bool isInFire = false;
    public bool isFrozen = false;
    public bool isInWater = false;
    public bool isInAcid = false;
    public bool isScared = false;

    private AcidSkill acid;

    private void Start()
    {
        startHealth = health;
        target = FindObjectOfType<Player>().transform;
    }

    public virtual void takeDamage(int value)
    {
        GameObject go = Instantiate(damageNumbers, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity) as GameObject;

        if (activated && !immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- " + value);
            }
            health -= value;
        }
        else if (immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Immune!");
            }
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
            Statistics.Instance.shake();

            if (bloodStain != null)
            {
                Instantiate(bloodStain, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
            if (bloodParticles != null)
            {
                Instantiate(bloodParticles, transform.position, Quaternion.Euler(90, 0, 0));
            }
            int g = Random.Range(minGoldToGive, maxGoldToGive);
            if (g < 0) g = 0;
            goldToGive = g;
            if (g > 0)
            {
                for (int i = 0; i < goldToGive; i++)
                {
                    GameObject go = CoinManager.Instance.GetCoin(coinType);
                    go.SetActive(true);
                    go.transform.position = transform.position;
                    Coin c = go.GetComponent<Coin>();
                    c.playerInRange = false;
                    c.isPreSet = true;
                    c.startPosition = transform.position;
                    c.target = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * coinForce + new Vector3(transform.position.x, transform.position.y, 0);
                    c.coinForce = coinForce;
                    c.isSet = true;
                    go.GetComponent<DestroyAfterTime>().isPool = true;
                }
            }
            Destroy(gameObject);
        }
    }

    public void freeze()
    {
        if (!isFrozen)
        {
            isFrozen = true;
            Invoke("startFreeze", 1f);
        }
    }

    private void startFreeze()
    {
        anim.speed = 0;
        target = transform;
        isFrozen = true;
        nextShot = 99999;
        Invoke("stopFrozen", 3.75f);
    }

    private void stopFrozen()
    {
        anim.speed = 1;
        isFrozen = false;
        target = FindObjectOfType<Player>().transform;
        nextShot = 0;
    }

    protected bool Seek()
    {
        bool inRange = false;
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            if (isScared)
            {
                if (Mathf.Abs((target.position - transform.position).magnitude) <= 15f)
                {
                    inRange = true;
                    transform.Translate((-target.position + transform.position).normalized * moveSpeed * Time.deltaTime);
                    changeAnimationState("Walk");
                }
                else
                {
                    inRange = false;
                    changeAnimationState("Idle");
                }
            }
            else
            {
                if (Mathf.Abs((target.position - transform.position).magnitude) <= range)
                {
                    inRange = true;
                    transform.Translate((target.position - transform.position).normalized * moveSpeed * Time.deltaTime);
                    changeAnimationState("Walk");
                }
                else
                {
                    inRange = false;
                    changeAnimationState("Idle");
                }
            }
        }
        return inRange;
    }

    public void changeAnimationState(string newState)
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (collision.transform.GetComponent<Player>().isImmune())
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
            }
            else
            {
                if (this.transform.tag == "sparring")
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

    public void fireEffect(int damage, int times)
    {
        if (!isInFire)
        {
            isInFire = true;
            fireDMG = damage;
            Invoke("endFire", 0.25f * times);
            for (int i = 0; i < times; i++)
            {
                Invoke("fireDamage", 0.25f * (i + 1));
            }
        }
    }

    private void endFire()
    {
        isInFire = false;
    }

    private void fireDamage()
    {
        GameObject go = Instantiate(damageNumbers, transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0), Quaternion.identity) as GameObject;

        if (activated && !immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- " + fireDMG);
            }
            health -= fireDMG;
        }
        else if (immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Immune!");
            }
        }
    }

    private void acidDamage()
    {
        if (acid.isInFire)
        {
            takeDamage(5);
        }
        else
        {
            takeDamage(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("skill") && collision.transform.name == "Water(Clone)")
        {
            isInWater = true;
        }

        if (collision.CompareTag("skill") && collision.transform.name == "Acid(Clone)")
        {
            if (!isInAcid)
            {
                acid = collision.GetComponent<AcidSkill>();
                InvokeRepeating("acidDamage", 0, 0.5f);
                isInAcid = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("skill") && collision.transform.name == "Water(Clone)")
        {
            isInWater = false;
        }

        if (collision.CompareTag("skill") && collision.transform.name == "Acid(Clone)")
        {
            isInAcid = false;
            CancelInvoke("acidDamage");
        }
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
                if (this.transform.tag == "sparring")
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
}