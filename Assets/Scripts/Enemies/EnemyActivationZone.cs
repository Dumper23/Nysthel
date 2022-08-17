using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class EnemyActivationZone : MonoBehaviour
{
    public Vector2 EnemyAreaSurface;
    public GameObject[] Barriers;
    public AudioClip endOfRoom;
    public AudioClip startOfRoom;
    public TextMeshPro enemyNumber;
    public bool isBoss = false;

    private bool activated = false;
    private bool finished = false;
    private bool hasBarriers = true;
    private Collider2D[] collisions;
    private List<Enemy> enemiesInside = new List<Enemy>();
    private Player p;

    private void Start()
    {
        if (enemyNumber != null)
        {
            enemyNumber.SetText("?");
        }
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
                if (enemy.GetComponent<Enemy>())
                {
                    enemy.GetComponent<Enemy>().enemyActivation(activated);
                    collisions[i] = enemy;
                    i++;
                }
                else if (enemy.GetComponent<StaticEye>())
                {
                    enemy.GetComponent<StaticEye>().activated = true;
                    collisions[i] = enemy;
                    i++;
                }
            }
        }
        if (enemyNumber != null)
        {
            enemyNumber.SetText(i.ToString());
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
            if (enemyNumber != null)
            {
                enemyNumber.SetText(enemyCount.ToString());
            }
            if (enemyCount == 0)
            {
                if (isBoss)
                {
                    Invoke("endZoom", 0.75f);
                    Camera.main.GetComponent<Animator>().Play("zoomIn");
                }
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
        else if (finished)
        {
            if (enemyNumber != null)
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
                if (enemyCount <= 0)
                {
                    enemyNumber.color = Color.green;
                    enemyNumber.SetText(enemyCount.ToString());
                }
                else
                {
                    enemyNumber.SetText(enemyCount.ToString());
                }
            }
        }
    }

    private void destroyObj()
    {
        Statistics.Instance.isInBoss = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!finished && !activated)
        {
            if (collision.CompareTag("Player") && !collision.GetComponent<Player>().inCombat)
            {
                if (isBoss)
                {
                    Statistics.Instance.isInBoss = true;
                    Camera.main.GetComponent<Animator>().Play("zoomOut");
                }
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
                        if (enemy.GetComponent<Enemy>())
                        {
                            enemy.GetComponent<Enemy>().enemyActivation(activated);
                            enemiesInside.Add(enemy.GetComponent<Enemy>());
                        }
                        else if (enemy.GetComponent<StaticEye>())
                        {
                            enemy.GetComponent<StaticEye>().activated = true;
                            enemiesInside.Add(enemy.GetComponent<Enemy>());
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
            if (collision.CompareTag("Player"))
            {
                p.inCombat = false;
                activated = false;
                if (isBoss && !finished)
                {
                    Statistics.Instance.isInBoss = false;
                    Camera.main.GetComponent<Animator>().Play("zoomIn");
                }
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
                        if (enemy.GetComponent<Enemy>())
                        {
                            enemy.GetComponent<Enemy>().enemyActivation(activated);
                            if (enemy.GetComponent<Bat>() && enemy.GetComponent<Bat>().intantiatedCopy != null && enemy.GetComponent<Bat>().intantiatedCopy.GetComponent<Bat>() != null)
                            {
                                Destroy(enemy.GetComponent<Bat>().intantiatedCopy.GetComponent<Bat>().gameObject);
                            }
                        }
                        else if (enemy.GetComponent<StaticEye>())
                        {
                            enemy.GetComponent<StaticEye>().activated = true;
                        }
                    }
                }
            }
        }
    }

    private void endZoom()
    {
        Statistics.Instance.isInBoss = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, EnemyAreaSurface);
    }
}