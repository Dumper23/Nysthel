using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_AncientWarrior : Enemy
{
    public float rangedAttackDuration = 2f;
    public float undeadAttackDuration = 2f;
    public float spikesAttackDuration = 1f;
    public float dashDuration = 1.5f;
    public float undeadRate = 1.5f;
    public GameObject[] spawnPoints;
    public GameObject[] bulletSpawnPoints;
    public GameObject villagePortal;
    public GameObject levelCompletedUi;

    public GameObject spikes;
    public GameObject spikeLocation;
    public GameObject bulletPrefab;
    public GameObject undeadPrefab;

    private string[] state;
    private bool spikesSpawned = false;
    private bool spikesCanSpawn = false;
    private Vector3 spikesPlace;
    private bool inAction = false;
    private string cState;
    private float nextUndead = 1.5f;
    private int originalHealth;
    private GameObject spikesLocationObject;
    private bool spiking = false;
    private bool undeadSpawning = false;
    private bool dashing = false;
    private Vector3 prevPlayerPos;
    private bool secondFase = false;
    private bool changeDone = false;

    private SpriteRenderer s;

    void Start()
    {
        originalHealth = health;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Decoration"), LayerMask.NameToLayer("Enemy"));
        target = FindObjectOfType<Player>().transform;

        state = new string[9];
        state[0] = "idle";
        state[1] = "idle";
        state[2] = "spikes";
        state[3] = "spikes";
        state[4] = "shot";
        state[5] = "shot";
        state[6] = "undead";
        state[7] = "dash";
        state[8] = "dash";

        s = GetComponent<SpriteRenderer>();
        cState = "idle";
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            if (!undeadSpawning || dashing)
            {
                Seek();
            }

            if (health <= 0)
            {
                Instantiate(villagePortal, transform.position, Quaternion.identity);
                Instantiate(levelCompletedUi);
                die();
            }

            if (health <= originalHealth / 1.5f)
            {
                s.color = new Color(20, 0, 0);
                moveSpeed = 0.25f;
                switch (cState)
                {
                    case "idle":
                        immune = false;
                        shooting();
                        break;

                    case "dash":
                        immune = false;
                        dash();
                        shooting();
                        if (!dashing)
                        {
                            prevPlayerPos = target.position;
                        }
                        if (!inAction)
                        {
                            Invoke("ChangeState", rangedAttackDuration);
                            inAction = true;
                        }
                        break;

                    case "spikes":
                        immune = false;
                        shooting();
                        spikeAttack();
                        break;

                    case "shot":
                        immune = false;
                        shooting();
                        undead();
                        if (!inAction)
                        {
                            Invoke("ChangeState", rangedAttackDuration);
                            inAction = true;
                        }
                        break;
                    case "undead":
                        immune = false;
                        undead();
                        shooting();
                        break;
                }
            }
            else
            {
                moveSpeed = 0.05f;
                switch (cState)
                {
                    case "idle":
                        immune = false;
                        idle();
                        break;

                    case "dash":
                        immune = false;
                        if (!dashing)
                        {
                            prevPlayerPos = target.position;
                        }
                        dash();

                        break;

                    case "spikes":
                        immune = false;
                        spikeAttack();
                        break;

                    case "shot":
                        immune = false;
                        shooting();
                        if (!inAction)
                        {
                            Invoke("ChangeState", rangedAttackDuration);
                            inAction = true;
                        }
                        break;
                    case "undead":
                        immune = false;
                        undead();
                        if (!inAction)
                        {
                            Invoke("ChangeState", undeadAttackDuration);
                            inAction = true;
                        }
                        break;
                }
            }
        }
    }

    private void ChangeState()
    {
        cState = state[Random.Range(0, state.Length - 1)];
        inAction = false;
        spikesCanSpawn = false;
        spikesSpawned = false;
        spiking = false;
        undeadSpawning = false;
        dashing = false;
     

        switch (cState)
        {
            case "idle":
                changeAnimationState("Idle");
                break;

            case "dash":
                changeAnimationState("Idle");
                break;

            case "spikes":
                changeAnimationState("SpikeAttack");
                break;

            case "shot":
                changeAnimationState("DeathSoulAttack");
                break;
            case "undead":
                changeAnimationState("2ndFase");
                break;
        }
        
    }

    private void undead()
    {
        undeadSpawning = true;
        if (Time.time > nextUndead)
        {
            nextUndead = Time.time + undeadRate;
            Instantiate(undeadPrefab, spawnPoints[Random.Range(0, spawnPoints.Length-1)].transform.position, Quaternion.identity);
        }        
    }

    private void shooting() { 
        if (Time.time > nextShot)
        {
            if (health <= originalHealth / 2)
            {
                nextShot = Time.time + attackRate/2;
                Instantiate(bulletPrefab, bulletSpawnPoints[Random.Range(0, bulletSpawnPoints.Length)].transform.position, Quaternion.identity);
            }
            else
            {
                nextShot = Time.time + attackRate;
                Instantiate(bulletPrefab, bulletSpawnPoints[Random.Range(0, bulletSpawnPoints.Length)].transform.position, Quaternion.identity);
            }
        }
        
    }

    private void idle()
    {
        if (!inAction)
        {
            Invoke("ChangeState", 2f);
            inAction = true;
        }
    }

    private void spikeAttack()
    {
        if (!spiking)
        {
            if (spikesCanSpawn && spikesLocationObject != null)
            {
                spikesLocationObject.transform.position = target.position;
                spikesPlace = target.position;
            }

            if (!spikesCanSpawn)
            {
                spikesLocationObject = Instantiate(spikeLocation, target.position, Quaternion.identity) as GameObject;
                spikesCanSpawn = true;
            }
            else if (!spikesSpawned)
            {
                Invoke("spawnSpikes", 2f);
                spikesSpawned = true;
            }
        }
    }

    private void spawnSpikes()
    {
        Instantiate(spikes, spikesPlace, Quaternion.identity);
        Invoke("ChangeState", spikesAttackDuration);
        spiking = true;
    }

    private void dash()
    {
        dashing = true;
        if (dashing)
        {
            transform.Translate((prevPlayerPos - transform.position).normalized * moveSpeed * 0.5f * 10 * Time.fixedDeltaTime);
            if (!inAction)
            {
                Invoke("ChangeState", dashDuration);
                inAction = true;
            }
        }
    }
}
