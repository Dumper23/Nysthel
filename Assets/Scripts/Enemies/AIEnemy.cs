using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : Enemy
{
    public float stoppingRange = 1f;
    public GameObject weapon;
    public float lanceRotationSpeed = 0.02f;
    public bool isRanged = false;
    public bool isBomber = false;
    public bool isPriest = false;
    public GameObject arrow;
    public GameObject bomb;
    public float angleSpread = 20;
    public int waypointCount = 4;
    public int walkRadius = 10;
    public List<Transform> waypoints = new List<Transform>();
    public AudioClip[] audios;
    public GameObject deathAudio;

    private static int SPEAR_AUDIO = 0;
    private static int BOW_AUDIO = 1;
    private static int SPELL_AUDIO = 2;
    private AudioSource audioSource;
    private NavMeshAgent Agent;
    private float time = 0;
    private bool attacking = false;
    private int currentWaypoint = 0;
    private float minAttackRate;
    private float maxAttackRate;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        time = attackRate;
        if (!isBomber)
        {
            target = FindObjectOfType<Player>().transform;
        }
        else
        {
            minAttackRate = attackRate / 2;
            maxAttackRate = attackRate + attackRate / 2;
            for (int i = 0; i <= waypointCount; i++)
            {
                bool isCorrect = false;
                NavMeshHit hit = new NavMeshHit();
                while (!isCorrect)
                {
                    Vector3 posToSpawnWayponit = (Random.insideUnitSphere * walkRadius) + transform.position;
                    if (NavMesh.SamplePosition(posToSpawnWayponit, out hit, walkRadius, NavMesh.AllAreas))
                    {
                        isCorrect = true;
                    }
                }
                GameObject w = new GameObject();
                w.transform.position = hit.position;
                w.name = "waypoint" + waypoints.Count;
                waypoints.Add(w.transform);
            }
        }
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            if (!attacking)
            {
                if (!isRanged && !isBomber)
                {
                    float angle = Mathf.Atan2((target.position - weapon.transform.position).normalized.y, (target.position - weapon.transform.position).normalized.x) * Mathf.Rad2Deg;
                    weapon.transform.rotation = Quaternion.Lerp(weapon.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), lanceRotationSpeed);
                }
            }
            if (!isBomber && (transform.position - target.position).magnitude <= range)
            {
                if (isRanged)
                {
                    float angle = Mathf.Atan2((target.position - weapon.transform.position).normalized.y, (target.position - weapon.transform.position).normalized.x) * Mathf.Rad2Deg;
                    weapon.transform.rotation = Quaternion.Lerp(weapon.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), lanceRotationSpeed);
                }

                if ((target.position - transform.position).magnitude <= range)
                {
                    Agent.SetDestination(target.position);
                }

                if (Mathf.Abs((target.position - transform.position).magnitude) <= stoppingRange + 0.2f)
                {
                    //Stop
                    time += Time.deltaTime;
                    if (time >= attackRate)
                    {
                        //Attack
                        time = 0;
                        attacking = true;
                        if (!isRanged)
                        {
                            anim.Play("attack");
                            audioSource.pitch = Random.Range(0.85f, 1.15f);
                            audioSource.clip = audios[SPEAR_AUDIO];
                            audioSource.Play();
                            Invoke("endAttack", 0.8f);
                        }
                        else if (isRanged)
                        {

                            GameObject go2 = Instantiate(arrow, firePoint.position, Quaternion.identity);
                            go2.GetComponent<EnemyBullet>().damage = damage;
                            if (!isPriest)
                            {
                                audioSource.pitch = Random.Range(0.85f, 1.15f);
                                audioSource.clip = audios[BOW_AUDIO];
                                audioSource.Play();
                                float angle2 = Mathf.Atan2((target.position - go2.transform.position).normalized.y, (target.position - go2.transform.position).normalized.x) * Mathf.Rad2Deg;
                                go2.transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle2 + 225, Vector3.forward);
                                go2.GetComponent<EnemyBullet>().setMoveDirection((target.position - go2.transform.position).normalized);
                            }
                            else
                            {
                                audioSource.pitch = Random.Range(0.85f, 1.15f);
                                audioSource.clip = audios[SPELL_AUDIO];
                                audioSource.Play();
                            }
                            Invoke("endAttack", 0.8f);
                        }
                    }

                    Agent.velocity = Vector2.zero;
                    Agent.isStopped = true;
                }
                else
                {
                    time = attackRate;
                    Agent.isStopped = false;
                }
            }

            if (isBomber)
            {
                time += Time.deltaTime;
                if (time >= Random.Range(minAttackRate, maxAttackRate))
                {
                    //Bomber drop bombs
                    time = 0;
                    Instantiate(bomb, transform.position, Quaternion.identity);
                }
                if (currentWaypoint > waypoints.Count - 1)
                {
                    currentWaypoint = 0;
                    foreach (Transform waypoint in waypoints)
                    {
                        Destroy(waypoint.gameObject);
                    }
                    waypoints.Clear();
                    for (int i = 0; i <= waypointCount; i++)
                    {
                        bool isCorrect = false;
                        NavMeshHit hit = new NavMeshHit();
                        while (!isCorrect)
                        {
                            Vector3 posToSpawnWayponit = (Random.insideUnitSphere * walkRadius) + transform.position;
                            if (NavMesh.SamplePosition(posToSpawnWayponit, out hit, walkRadius, NavMesh.AllAreas))
                            {
                                isCorrect = true;
                            }
                        }
                        GameObject w = new GameObject();
                        w.tag = "waypoint";
                        w.transform.position = hit.position;
                        w.name = "waypoint" + waypoints.Count;
                        waypoints.Add(w.transform);
                    }
                }
                Agent.SetDestination(waypoints[currentWaypoint].transform.position);

                if ((transform.position - waypoints[currentWaypoint].transform.position).magnitude <= Agent.stoppingDistance)
                {
                    currentWaypoint++;
                }
            }
            if(health <= 0)
            {
                Instantiate(deathAudio, transform.position, Quaternion.identity);
                die();
            }
           
        }
    }

    private void endAttack()
    {
        attacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, stoppingRange);
        Gizmos.DrawWireSphere(transform.position, range);
        if (isBomber)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, walkRadius);
            foreach (GameObject waypoint in GameObject.FindGameObjectsWithTag("waypoint"))
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(waypoint.transform.position, 1f);
            }
        }
    }
}