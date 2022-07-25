using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Ulrus : Enemy
{
    [Header("\n----------Melee Options----------\n")]
    public float meleeRange = 1f;
    public float dashSpeed = 1f;
    public float fireSpawnRate = 0.075f;
    public float dashRestTime = 0.75f;
    public float meleeStateDuration = 3f;
    public GameObject fire;
    public Transform fireSpawnPoint;
    public GameObject meleeCollider;

    private Vector3 oldTargetPos;
    private bool dashed = false;
    private bool meleeDone = false;
    private float initialDashSpeed;

    [Header("\n----------Teleport Options----------\n")]
    public BoundsInt teleportArea;
    public GameObject teleportIndication;
    public ParticleSystem teleportWarning;
    public Collider2D teleportCollider;
    public GameObject seekBullet;

    private Vector3 teleportPosition;
    private Collider2D selfCollider;
    private GameObject go;
    private GameObject go1;
    private GameObject go2;
    private GameObject go3;
    private GameObject go4;
    private GameObject go5;
    private GameObject go6;
    private GameObject go7;
    private GameObject go8;

    [Header("\n----------Ranged Attack Options----------\n")]
    public GameObject bullet;

    private float initialAttackRate;
    private bool isShooting = false;

    [Header("\n----------Area Attack Options----------\n")]
    public ParticleSystem areaAttackIndicator;
    public GameObject areaAttackCircle;
    public GameObject fireColumn;
    public GameObject fireColumnContinuous;
    public float areaAttackRange = 5f;

    private bool areaAttackIsPrepared = false;

    [Header("\n----------Laser Options----------\n")]
    public GameObject lasersFase1;
    public GameObject lasersFase2;
    public float laserTime = 20f;

    [Header("\n--------Audio options---------\n")]
    public AudioSource attackAS;
    public AudioSource damageAS;
    public AudioSource dashAS;
    public AudioClip[] audios;

    private static int DAMAGE_AUDIO = 0;
    private static int FIREBALL_AUDIO = 1;
    private static int AREA_AUDIO = 2;
    private static int TELEPORT_AUDIO = 3;
    private static int MELEE_AUDIO = 4;
    private static int DASH_AUDIO = 5;
    private static int LASER_AUDIO = 6;

    [Header("\n--------Other options---------\n")]
    public SpriteRenderer sprite;
    public GameObject portal;
    public GameObject blood;
    public SpriteRenderer penthagram;
    public GameObject fires;
    public float immuneTime = 1f;
    public GameObject shield;
    public GameObject deathSound;

    private bool patronDone = false;
    public int fase = 0;
    public string cState;
    private bool continuousColumnsSpawned = false;
    private string[] state;
    private float time = 0;
    private float attackTime = 0;
    private bool isActing = false;
    private float initialMoveSpeed;
    private string newState = "";



    private void Start()
    {
        shield.SetActive(false);
        fires.SetActive(false);
        penthagram.color = new Color(0, 0, 0, 0.5f);
        lasersFase1.SetActive(false);
        lasersFase2.SetActive(false);
        initialMoveSpeed = moveSpeed;
        initialAttackRate = attackRate;
        initialDashSpeed = dashSpeed;
        startHealth = health;
        areaAttackIndicator.Stop();
        areaAttackCircle.SetActive(false);
        selfCollider = GetComponent<CapsuleCollider2D>();
        teleportCollider.enabled = false;
        teleportIndication.SetActive(false);
        meleeCollider.SetActive(false);
        target = FindObjectOfType<Player>().transform;

        state = new string[13];
        state[0] = "rangedAttack";
        state[1] = "rangedAttack";
        state[2] = "melee";
        state[3] = "melee";
        state[4] = "walk";
        state[5] = "teleport"; 
        state[6] = "teleport"; 
        state[7] = "areaAttack";
        state[8] = "areaAttack";
        state[9] = "laser";
        state[10] = "laser";
        state[11] = "rangedAttack";
        state[12] = "laser";

        cState = "areaAttack";
    }

    // Update is called once per frame
    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            if (!immune)
            {
                shield.SetActive(false);
            }
            if(health <= 0)
            {
                DamageEnemy[] columns = FindObjectsOfType<DamageEnemy>();
                foreach (DamageEnemy column in columns)
                {
                    Destroy(column.gameObject);
                }
                Destroy(lasersFase1.gameObject);
                Destroy(lasersFase2.gameObject);
                Instantiate(portal,  transform.position, Quaternion.identity);
                Destroy(fires);
                Instantiate(deathSound);
                die();
            }

            if(health > (startHealth / 3) * 2.5f)
            {
                fase = 1;
                dashSpeed = initialDashSpeed / 2;
                penthagram.color = new Color(0,0,0,0.5f);
            }
            else if(health > (startHealth / 3) * 1.5f)
            {
                fase = 2;
                dashSpeed = (initialDashSpeed / 4) * 3;
                moveSpeed = initialMoveSpeed * 1.75f;
                penthagram.color = new Color(255, 255, 255, 0.5f);

            }
            else
            {
                fires.SetActive(true);
                fase = 3;
                dashSpeed = initialDashSpeed;
                moveSpeed = initialMoveSpeed * 2.25f;
            }

            if (!isShooting)
            {
                if (target.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            switch (cState)
            {
                case "walk":
                    moveSpeed = 3.1f;
                    Seek();
                    if (!isActing)
                    {
                        isActing = true;
                        changeAnimationState("walk");
                        Invoke("ChangeState", 2);
                    }
                    break;

                case "melee":

                    if (!isActing)
                    {
                        changeAnimationState("walk");
                        oldTargetPos = target.position + (Vector3)target.GetComponent<Player>().movement;
                        isActing = true;
                        Invoke("ChangeState", meleeStateDuration);
                    }
                    if ((transform.position - target.position).magnitude <= 1)
                    {
                        Invoke("changeProximity", 1f);
                    }
                    else
                    {
                        CancelInvoke("changeProximity");
                        dash();
                    }

                    break;

                case "teleport":
                    if (!isActing)
                    {
                        teleportWarning.Play();
                        Invoke("startTeleportAction", 1f);
                        isActing = true;
                    }
                    teleportIndication.transform.position = Vector3.Lerp(teleportIndication.transform.position, transform.position - new Vector3(0,1.66f,0), 0.01f);
                    break;

                case "laser":
                    if (fase > 1)
                    {
                        if (!isActing)
                        {
                            isActing = true;
                            changeAnimationState("laser");
                            attackAS.clip = audios[LASER_AUDIO];
                            attackAS.Play();
                            Invoke("activateLaser", 1f);
                            Invoke("ChangeState", 1.2f);
                        }
                    }
                    else
                    {
                        ChangeState();
                    }
                    break;

                case "areaAttack":
                    areaAttackIndicator.Play();

                    if (!isActing)
                    {
                        areaAttackCircle.SetActive(true);
                    }

                    if (areaAttackCircle.transform.localScale.x < 13)
                    {
                        areaAttackCircle.transform.localScale += new Vector3(0.07f, 0.07f);
                    }
                    else
                    {
                        if (!isActing)
                        {
                            isActing = true;
                            areaAttack();
                            Invoke("hideIndicator", 0.6f);
                        }
                    }
                    if (areaAttackIsPrepared)
                    {
                        attackAS.clip = audios[AREA_AUDIO];
                        attackAS.Play();
                        spawnColumns();
                    }

                    break;

                case "rangedAttack":
                    if (fase > 1)
                    {
                        if (!isActing)
                        {
                            isActing = true;
                            Invoke("ChangeState", 3f);
                        }
                        rangedAttack();
                    }
                    else
                    {
                        ChangeState();
                    }
                    break;
            }
        }
    }

    private void changeProximity()
    {
        ChangeState("teleport");
    }

    public void activate()
    {
        Invoke("setActivation", 0.5f);
        attackAS.clip = audios[TELEPORT_AUDIO];
        attackAS.Play();
    }

    private void setActivation()
    {
        activated = true;
    }

    #region DASH
    private void dash()
    {
        if ((transform.position - oldTargetPos).magnitude >= meleeRange)
        {
            if (!dashed)
            {
                dashAS.clip = audios[DASH_AUDIO];
                dashAS.Play();
                dashed = true;
            }
            transform.position = Vector3.Lerp(transform.position, oldTargetPos, (dashSpeed * Time.deltaTime) / 2);
            meleeCollider.SetActive(false);
            attackTime += Time.deltaTime;
            if (attackTime >= fireSpawnRate)
            {
            attackTime = 0;
            Instantiate(fire, fireSpawnPoint.position, Quaternion.identity);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position, (dashSpeed * Time.deltaTime) / 1.5f);
            Invoke("colliderActivation", 0.2f);
            changeAnimationState("melee");
            Invoke("updateOldTargetPos", dashRestTime);
        }
    }

    private void colliderActivation()
    {
        meleeCollider.SetActive(true);
        if (!meleeDone)
        {
            meleeDone = true;
            Invoke("resetMeleeAudio", 1f);
            attackAS.clip = audios[MELEE_AUDIO];
            attackAS.Play();
            dashed = false;
        }
    }

    private void resetMeleeAudio()
    {
        meleeDone = false;
    }

    private void updateOldTargetPos()
    {
        changeAnimationState("walk");
        oldTargetPos = target.position + (Vector3)target.GetComponent<Player>().movement*2;
    }

    #endregion

    #region TELEPORT

    private void startTeleportAction()
    {
        attackAS.clip = audios[TELEPORT_AUDIO];
        attackAS.Play();
        teleportCollider.enabled = true;
        Invoke("hideTeleportCollider", 0.35f);
        teleportPosition = new Vector2(Random.Range(teleportArea.xMin, teleportArea.xMax), Random.Range(teleportArea.yMin, teleportArea.yMax));
        changeAnimationState("teleport");
        Invoke("teleport", 0.7f);
    }

    private void teleport()
    {
        //Start teleport
        shield.SetActive(false);
        immune = true;
        #region Bullet Instantiation
        go = Instantiate(seekBullet, transform.position, Quaternion.identity);
        go.GetComponent<EnemyBullet>().damage = damage;
        go.GetComponent<EnemyBullet>().isSeeker = false;
        go.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(1, 0));
        go.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
        go.GetComponent<EnemyBullet>().speed = go.GetComponent<EnemyBullet>().speed * 2;

        go2 = Instantiate(seekBullet, transform.position, Quaternion.identity);
        go2.GetComponent<EnemyBullet>().damage = damage;
        go2.GetComponent<EnemyBullet>().isSeeker = false;
        go2.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(-1, 0));
        go2.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
        go2.GetComponent<EnemyBullet>().speed = go2.GetComponent<EnemyBullet>().speed * 2;

        if (fase >= 2)
        {
            go3 = Instantiate(seekBullet, transform.position, Quaternion.identity);
            go3.GetComponent<EnemyBullet>().damage = damage;
            go3.GetComponent<EnemyBullet>().isSeeker = false;
            go3.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(0, 1));
            go3.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
            go3.GetComponent<EnemyBullet>().speed = go3.GetComponent<EnemyBullet>().speed * 2;

            go4 = Instantiate(seekBullet, transform.position, Quaternion.identity);
            go4.GetComponent<EnemyBullet>().damage = damage;
            go4.GetComponent<EnemyBullet>().isSeeker = false;
            go4.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(0, -1));
            go4.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
            go4.GetComponent<EnemyBullet>().speed = go4.GetComponent<EnemyBullet>().speed * 2;

            if (fase >= 3)
            {
                go5 = Instantiate(seekBullet, transform.position, Quaternion.identity);
                go5.GetComponent<EnemyBullet>().damage = damage;
                go5.GetComponent<EnemyBullet>().isSeeker = false;
                go5.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(0.7f, 0.7f));
                go5.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
                go5.GetComponent<EnemyBullet>().speed = go5.GetComponent<EnemyBullet>().speed * 2;

                go6 = Instantiate(seekBullet, transform.position, Quaternion.identity);
                go6.GetComponent<EnemyBullet>().damage = damage;
                go6.GetComponent<EnemyBullet>().isSeeker = false;
                go6.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(0.7f, -0.7f));
                go6.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
                go6.GetComponent<EnemyBullet>().speed = go6.GetComponent<EnemyBullet>().speed * 2;

                go7 = Instantiate(seekBullet, transform.position, Quaternion.identity);
                go7.GetComponent<EnemyBullet>().damage = damage;
                go7.GetComponent<EnemyBullet>().isSeeker = false;
                go7.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(-0.7f, 0.7f));
                go7.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
                go7.GetComponent<EnemyBullet>().speed = go7.GetComponent<EnemyBullet>().speed * 2;


                go8 = Instantiate(seekBullet, transform.position, Quaternion.identity);
                go8.GetComponent<EnemyBullet>().damage = damage;
                go8.GetComponent<EnemyBullet>().isSeeker = false;
                go8.GetComponent<EnemyBullet>().setMoveDirection(new Vector2(-0.7f, -0.7f));
                go8.GetComponent<EnemyBullet>().target = FindObjectOfType<Player>().transform;
                go8.GetComponent<EnemyBullet>().speed = go8.GetComponent<EnemyBullet>().speed * 2;
            }
        }
        Invoke("makeFollow", 2f);
        #endregion

        teleportIndication.SetActive(true);
        selfCollider.enabled = false;
        sprite.enabled = false;
        transform.position = teleportPosition;
        Invoke("teleportArrival", 2f);
    }

    private void makeFollow()
    {
        if(go != null) go.GetComponent<EnemyBullet>().isSeeker = true;
        if (go2 != null) go2.GetComponent<EnemyBullet>().isSeeker = true;
        if (go3 != null) go3.GetComponent<EnemyBullet>().isSeeker = true;
        if (go4 != null) go4.GetComponent<EnemyBullet>().isSeeker = true;
        if (go5 != null) go5.GetComponent<EnemyBullet>().isSeeker = true;
        if (go6 != null) go6.GetComponent<EnemyBullet>().isSeeker = true;
        if (go7 != null) go7.GetComponent<EnemyBullet>().isSeeker = true;
        if (go8 != null) go8.GetComponent<EnemyBullet>().isSeeker = true;

        if (go != null) go.GetComponent<EnemyBullet>().speed = go.GetComponent<EnemyBullet>().speed / 2;
        if (go2 != null) go2.GetComponent<EnemyBullet>().speed = go2.GetComponent<EnemyBullet>().speed / 2;
        if (go3 != null) go3.GetComponent<EnemyBullet>().speed = go3.GetComponent<EnemyBullet>().speed / 2;
        if (go4 != null) go4.GetComponent<EnemyBullet>().speed = go4.GetComponent<EnemyBullet>().speed / 2;
        if (go5 != null) go5.GetComponent<EnemyBullet>().speed = go5.GetComponent<EnemyBullet>().speed / 2;
        if (go6 != null) go6.GetComponent<EnemyBullet>().speed = go6.GetComponent<EnemyBullet>().speed / 2;
        if (go7 != null) go7.GetComponent<EnemyBullet>().speed = go7.GetComponent<EnemyBullet>().speed / 2;
        if (go8 != null) go8.GetComponent<EnemyBullet>().speed = go8.GetComponent<EnemyBullet>().speed / 2;
    }

    private void teleportArrival()
    {
        //End teleport
        attackAS.clip = audios[TELEPORT_AUDIO];
        attackAS.Play();
        immune = false;
        teleportCollider.enabled = true;
        Invoke("hideTeleportCollider", 0.35f);
        teleportIndication.SetActive(false);
        selfCollider.enabled = true;
        sprite.enabled = true;
        changeAnimationState("teleportArrive");
        Invoke("ChangeState", 1);
    }

    private void hideTeleportCollider()
    {
        teleportCollider.enabled = false;
    }

    #endregion

    #region RANGED ATTACK
    private void rangedAttack()
    {
        if(fase == 1)
        {
            attackRate = initialAttackRate * 2;
        }else if (fase == 2)
        {
            attackRate = initialAttackRate * 1.5f;
        }
        else
        {
            attackRate = initialAttackRate;
        }

        time += Time.deltaTime;
        if(time >= attackRate)
        {
            attackAS.clip = audios[FIREBALL_AUDIO];
            attackAS.Play();
            isShooting = true;
            Invoke("shoot", 0.2f);
            Invoke("isShootingOff", 0.6f);
            time = 0;
            changeAnimationState("rangedAttack");
            Invoke("idleState", 0.55f);
        }
    }

    private void shoot()
    {
        oldTargetPos = target.position + (Vector3)target.GetComponent<Player>().movement * 2;
        GameObject go = Instantiate(bullet, firePoint.position, Quaternion.identity);
        go.GetComponent<EnemyBullet>().damage = damage;

        float angle2 = Mathf.Atan2((oldTargetPos - go.transform.position).normalized.y, (oldTargetPos - go.transform.position).normalized.x) * Mathf.Rad2Deg;
        go.transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle2, Vector3.forward);
        go.GetComponent<EnemyBullet>().setMoveDirection((oldTargetPos - go.transform.position).normalized);
    }

    private void isShootingOff()
    {
        isShooting = false;
    }

    private void idleState()
    {
        changeAnimationState("idle");
    }

    #endregion

    #region AREA ATTACK

    private void areaAttack()
    {
        changeAnimationState("areaAttack");
    }

    private void hideIndicator()
    {
        areaAttackIsPrepared = true;
        areaAttackCircle.SetActive(false);
        areaAttackCircle.transform.localScale = Vector3.zero;
    }

    private void spawnColumns()
    {
        Instantiate(fireColumn, transform.position + new Vector3(4.5f, 0), Quaternion.identity);
        Instantiate(fireColumn, transform.position + new Vector3(0, 4.5f), Quaternion.identity);
        Instantiate(fireColumn, transform.position + new Vector3(-4.5f, 0), Quaternion.identity);
        Instantiate(fireColumn, transform.position + new Vector3(0, -4.5f), Quaternion.identity);

        Instantiate(fireColumn, transform.position + new Vector3(3.2f, 3.2f), Quaternion.identity);
        Instantiate(fireColumn, transform.position + new Vector3(3.2f, -3.2f), Quaternion.identity);
        Instantiate(fireColumn, transform.position + new Vector3(-3.2f, 3.2f), Quaternion.identity);
        Instantiate(fireColumn, transform.position + new Vector3(-3.2f, -3.2f), Quaternion.identity);

        areaAttackIsPrepared = false;
        Invoke("spawnContinuous", 0.5f);
    }

    private void spawnContinuous()
    {
        if (!continuousColumnsSpawned && fase >= 2)
        {
            continuousColumnsSpawned = true;
            Instantiate(fireColumnContinuous, transform.position + new Vector3(4.5f, 0), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(1, 0));
            Instantiate(fireColumnContinuous, transform.position + new Vector3(0, 4.5f), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(0, 1));
            Instantiate(fireColumnContinuous, transform.position + new Vector3(-4.5f, 0), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(-1, 0));
            Instantiate(fireColumnContinuous, transform.position + new Vector3(0, -4.5f), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(0, -1));

            Instantiate(fireColumnContinuous, transform.position + new Vector3(3.2f, 3.2f), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(1f, 1f));
            Instantiate(fireColumnContinuous, transform.position + new Vector3(3.2f, -3.2f), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(1f, -1f));
            Instantiate(fireColumnContinuous, transform.position + new Vector3(-3.2f, 3.2f), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(-1f, 1f));
            Instantiate(fireColumnContinuous, transform.position + new Vector3(-3.2f, -3.2f), Quaternion.identity).GetComponent<DamageEnemy>().setDirection(new Vector3(-1f, -1f));
        }
        ChangeState();
    }

    #endregion

    #region LASER
    private void activateLaser()
    {
        lasersFase1.SetActive(false);
        lasersFase2.SetActive(false);
        lasersFase1.transform.position = transform.position;
        lasersFase2.transform.position = transform.position;
        if (fase <= 2)
        {
            lasersFase1.SetActive(true);
        }
        else
        {
            lasersFase2.SetActive(true);
        }
    }

    private void desactivateLaser()
    {
        lasersFase1.SetActive(false);
        lasersFase2.SetActive(false);
    }
    #endregion

    private void ChangeState()
    {
        meleeDone = false;
        dashed = false;
        changeAnimationState("");
        areaAttackIsPrepared = false;
        meleeCollider.SetActive(false);
        teleportCollider.enabled = false;
        teleportIndication.SetActive(false);
        areaAttackIndicator.Stop();

        if (!patronDone)
        {
            doPatron();
            cState = newState;
            patronDone = true;
        }
        else
        {
            cState = state[Random.Range(0, state.Length)];
            patronDone = false;
        }
        isActing = false;
        isShooting = false; 
        CancelInvoke();
    }

    private void ChangeState(string stat)
    {
        meleeDone = false;
        dashed = false;
        changeAnimationState("");
        areaAttackIsPrepared = false;
        meleeCollider.SetActive(false);
        teleportCollider.enabled = false;
        teleportIndication.SetActive(false);
        areaAttackIndicator.Stop();
        patronDone = false;
        isActing = false;
        isShooting = false;
        cState = stat;
        CancelInvoke();
    }

    private void doPatron()
    {
        switch (cState)
        {
            case "walk":
                newState = "areaAttack";
                break;

            case "melee":
                newState = "rangedAttack";
                break;

            case "teleport":
                newState = "melee";
                break;

            case "laser":
                newState = "melee";
                break;

            case "areaAttack":
                newState = "melee";
                break;

            case "rangedAttack":
                newState = "teleport";
                break;
        }
    }

    public override void takeDamage(int value)
    {
        base.takeDamage(value);
        if (!immune)
        {
            damageAS.clip = audios[DAMAGE_AUDIO];
            damageAS.Play();
            string[] an = new string[2];
            shield.SetActive(true);
            immune = true;
            an[0] = "blood";
            an[1] = "blood2";
            blood.GetComponent<Animator>().Play(an[Random.Range(0, an.Length)]);
        }
        Invoke("endImmune", immuneTime);
    }

    private void endImmune()
    {
        shield.SetActive(false);
        immune = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(teleportArea.center, teleportArea.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, areaAttackRange);
    }
}