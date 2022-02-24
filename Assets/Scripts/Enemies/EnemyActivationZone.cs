using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class EnemyActivationZone : MonoBehaviour
{
    public Vector2 EnemyAreaSurface;
    public GameObject[] Barriers;
    public AudioClip endOfRoom;
    public AudioClip startOfRoom;

    private bool activated = false;
    private bool finished = false;
    private bool hasBarriers = true;
    private Collider2D[] collisions;
    private Player p;


    private void Start()
    {
        if (Barriers != null)
        {
            foreach (GameObject barrier in Barriers)
            {
                barrier.SetActive(false);
            }
        }
        else
        {
            hasBarriers = false;
        }
        GetComponent<AudioSource>().loop = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<BoxCollider2D>().size = EnemyAreaSurface;
        Invoke("DetectEnemies", 2f);
        collisions = new Collider2D[999];
               
    }

    private void DetectEnemies()
    {
        Collider2D[] temp = Physics2D.OverlapBoxAll(transform.position, EnemyAreaSurface, 90);
        int i = 0;
        foreach (Collider2D enemy in temp)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().enemyActivation(activated);
                collisions[i] = enemy;
                i++;
            }
        }
    }

    private void Update()
    {
        if (activated && !finished)
        {
            Collider2D[] temp = Physics2D.OverlapBoxAll(transform.position, EnemyAreaSurface, 0);
            int enemyCount = 0;
            foreach (Collider2D enemy in temp)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemyCount++;
                }
            }
            if(enemyCount == 0)
            {
                finished = true;
                p.inCombat = false;
                //Avis que ja s'ha acabat la sala
                GetComponent<AudioSource>().clip = endOfRoom;
                GetComponent<AudioSource>().Play();
                if (hasBarriers)
                {
                    foreach (GameObject barrier in Barriers)
                    {
                        barrier.SetActive(false);
                    }
                }
                Invoke("destroyObj", 1.5f);   
            }
        }
    }

    private void destroyObj()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!finished && !activated)
        {
            if (collision.CompareTag("Player") && !collision.GetComponent<Player>().inCombat)
            {
                p = collision.GetComponent<Player>();
                p.inCombat = true;
                activated = true;
                GetComponent<AudioSource>().clip = startOfRoom;
                GetComponent<AudioSource>().Play();
                if (hasBarriers)
                {
                    foreach (GameObject barrier in Barriers)
                    {
                        barrier.SetActive(true);

                    }
                }

                foreach (Collider2D enemy in collisions)
                {
                    if (enemy != null)
                    {
                        if (enemy.CompareTag("Enemy"))
                        {
                            enemy.GetComponent<Enemy>().enemyActivation(activated);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!finished)
        {
            if (collision.CompareTag("Player") && (collision.transform.position - transform.position).magnitude > 35)
            {
                p.inCombat = false;
                activated = false;
                if (hasBarriers)
                {
                    foreach (GameObject barrier in Barriers)
                    {
                        barrier.SetActive(false);

                    }
                }

                foreach (Collider2D enemy in collisions)
                {
                    if (enemy != null)
                    {
                        if (enemy.CompareTag("Enemy"))
                        {
                            enemy.GetComponent<Enemy>().enemyActivation(activated);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, EnemyAreaSurface);
    }
}