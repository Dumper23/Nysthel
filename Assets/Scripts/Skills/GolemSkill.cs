using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSkill : MonoBehaviour
{
    public float moveSpeed;
    public float radious = 6;
    public float attackRate = 2f;
    public GameObject RockSpikes;
    public float timeToDie = 30f;

    private Player player;
    private int damageAttackBasic = 1;
    private int damageAttackComplex = 2;
    private float nextAttack = 0;
    private Collider2D[] enemies;
    private float closestDistance = 999999;
    private GameObject closestEnemy;
    private string currentState = "idle";
    private Animator anim;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        Invoke("die", timeToDie);
    }

    private void Update()
    {
        closestDistance = 999999;
        damageAttackBasic = (player.totalDamage) / 3;
        damageAttackComplex = player.totalDamage;
        enemies = Physics2D.OverlapCircleAll(transform.position, radious);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>())
            {
                if ((enemy.gameObject.transform.position - transform.position).magnitude < closestDistance)
                {
                    closestDistance = (enemy.gameObject.transform.position - transform.position).magnitude;
                    closestEnemy = enemy.gameObject;
                }
            }
        }
        if (closestEnemy != null)
        {
            if (closestEnemy.transform.position.x >= transform.position.x)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1.5f, 1.5f, 1);
            }

            if ((transform.position - closestEnemy.transform.position).magnitude > 1.5f)
            {
                transform.Translate((closestEnemy.transform.position - transform.position).normalized * Time.deltaTime * moveSpeed);
            }
            else
            {
                if (Time.time > nextAttack)
                {
                    attack();
                    nextAttack = Time.time + attackRate;
                }
            }
        }
        else
        {
            changeAnimationState("idle");
            if (player.transform.position.x >= transform.position.x)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1.5f, 1.5f, 1);
            }
            if ((transform.position - player.transform.position).magnitude > 1.5f)
            {
                transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime * moveSpeed);
            }
        }
    }

    private void attack()
    {
        if (SaveVariables.EARTH_ORB == 2)
        {
            if (Random.Range(0, 1f) > 0.25)
            {
                changeAnimationState("Attack2");
                Invoke("spikes", 0.5f);
            }
            else
            {
                changeAnimationState("Attack");
                Invoke("basic", 0.2f);
                Invoke("basic", 0.55f);
            }
        }
        else
        {
            changeAnimationState("Attack");
            Invoke("basic", 0.2f);
            Invoke("basic", 0.55f);
        }
    }

    private void basic()
    {
        audioSource.Play();
        closestEnemy.GetComponent<Enemy>().takeDamage(damageAttackBasic);
        //Sonido golpes Roca
    }

    private void spikes()
    {
        audioSource.Play();
        Instantiate(RockSpikes, closestEnemy.transform.position, Quaternion.identity);
        closestEnemy.GetComponent<Enemy>().takeDamage(damageAttackComplex);
        //Sonido golpes Roca
    }

    private void die()
    {
        attackRate = 99999;
        nextAttack = 99999;
        changeAnimationState("Death");
        Destroy(gameObject, 1.5f);
    }

    public void changeAnimationState(string newState)
    {
        currentState = "";
        //We avoid playing the same animation multiple times
        if (currentState == newState) return;
        //We play a determinated animation
        anim.Play(newState);

        currentState = newState;
    }
}