using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour, IShopCustomer
{
    [Header("--------------Movement--------------")]
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public float dashRestoreTime = 3f;
    public float dashForce = 10f;
    public float dashTime = 1f;
    public ParticleSystem walkParticles;
    public ParticleSystem dashParticles;
    public ParticleSystem pickUpParticles;
    public GameObject dashSmoke;

    public Vector2 movement;
    private Vector2 aimPos;
    private bool dashing = false;
    private float nextDash = 0f;
    private Vector2 dashDirection;
    private Vector2 dashSpeed;
    private Vector3 lastUpdatePos = Vector3.zero;
    private Vector3 dist;
    private float curentSpeed;

    [Header("--------------Combat--------------")]
    public int damage = 10;

    public int defense = 0;
    private int originalDamage;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float animationDelay = 0.4f;
    public float attackRate = 0.9f;
    public float immunityTime = 1f;
    public GameObject crossHair;
    public bool inCombat;
    public ParticleSystem damageParticles;
    public GameObject bloodDecal;

    private bool attacking = false;
    private float nextFire = 0f;
    private bool immune = false;
    private Vector2 lookDir;
    private Vector3 directionToShoot;

    [Header("--------------weapon config--------------")]
    public float bloodyAxeRateIncrement = 0.2f;

    public float doubleAxeRateIncrement = 0.44f;
    public float seekAxeRateIncrement = 0.3f;
    public float multiAxeRateIncrement = 1f;
    public float basicAxeRateIncrement = 0.15f;
    public float battleAxeRateIncrement = 0.15f;    //Falta setejar
    public float nysthelAxeRateIncrement = 0.15f;   //Falta setejar
    public float trueAxeRateIncrement = 0.15f;      //Falta setejar

    public float timeToShotBattle = 1f;
    public GameObject battleCircleLoad;
    public GameObject battleCircleLoadMaximum;

    private int bulletsToShoot = 0;
    private bool battlePressing = false;

    //Damage
    public int bloodyAxeDamageIncrement = 15;

    public int seekAxeDamageIncrement = 10;
    public int battleAxeDamageIncrement = 10;
    public int nysthelAxeDamageIncrement = 10;
    public int trueAxeDamageIncrement = 10;
    public int doubleAxeDamageIncrement = 5;

    //Bullet Config
    public GameObject bloodyBulletPrefab;

    public GameObject seekBulletPrefab;
    public GameObject battleBulletPrefab;
    public GameObject nysthelBulletPrefab;
    public GameObject trueBulletPrefab;

    [Header("--------------Player Stats--------------")]
    public int maxHealth = 50;

    public int gold;
    public int wood;
    public int woodMultiplier = 1;
    public int goldMultiplier = 1;
    public float coinMagnetRange = 2f;
    public float coinMagnetSpeed = 1f;
    public float goldPotionDuration = 30f;
    public float shieldDuration = 5f;
    public float timePotionDuration = 3f;
    public float secondChanceProbability = 0.1f;

    private int currentHealth;

    [Header("--------------Audio--------------")]
    public List<AudioClip> audios;

    public List<AudioSource> audioSource;
    public GameObject coinSound;

    private const int ATTACK_AUDIO = 0;
    private const int DASH_AUDIO = 1;
    private const int FOOTSTEP_AUDIO = 2;
    private const int PICKUP_AUDIO = 3;
    private const int DAMAGE_AUDIO = 4;
    private const int NEGATIVE_AUDIO = 5;

    [Header("--------------UI And other settings--------------")]
    public TextMeshProUGUI goldText;

    public TextMeshProUGUI BlackSmithGoldText;
    public TextMeshProUGUI woodText;

    [SerializeField]
    private UIInventory uiInventory;

    public HealthBar healthBar;
    public GameObject goldMultiplierUI;
    public GameObject shield;
    public GameObject goldRush;
    public TextMeshProUGUI counterText;
    public Animator anim;
    public Camera cam;
    public float smoothFactor = 5f;
    public bool timeSlowed = false;
    public bool inShop = false;
    public GameObject customCursor;
    public TextMeshProUGUI dpsDebugger;
    public GameObject feedBackScreenPanel;
    public bool dpsDebug = false;
    public bool usingController = false;

    private string currentState;
    private Inventory inventory;
    private bool shielded = false;
    private bool goldRushed = false;
    private bool basicaxe = true;
    private bool multiaxe = false;
    private bool doubleaxe = false;
    private bool bloodyaxe = false;
    private bool seekaxe = false;
    private bool battleaxe = false;
    private bool nysthelaxe = false;
    private bool trueaxe = false;
    private float timer = 0.0f;
    private int seconds = 0;
    public bool hasSecondChance;
    public bool isDead = false;
    private bool goldStatueApplied = false;
    private bool damageStatueApplied = false;
    private float timeToStep = 0.2f;
    private float timePassedStep = 0;

    [Header("--------------Path Renderer--------------")]
    public bool pathRenderer = false;

    public LineRenderer ruteFollowed;
    public float timeBetweenPointsInRute = 2f;

    private int posIndex = 0;
    private float nextPoint = 0;
    private float battleTimePressed = 0;

    private void Awake()
    {
        inventory = new Inventory(UseItem);
        uiInventory.setInventory(inventory);
        uiInventory.setPlayer(this);
        GameStateManager.Instance.OnGameStateChange += OnGameStateChanged;
    }

    private void setSound()
    {
        audioSource[PICKUP_AUDIO].volume = 0.7f;
    }

    private void Start()
    {
        audioSource[PICKUP_AUDIO].volume = 0;
        Invoke("setSound", 0.5f);
        battleCircleLoad.SetActive(false);
        battleCircleLoadMaximum.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Time.timeScale = 1f;
        goldStatueApplied = false;
        //Unic LoadGame Que hi ha d'haver ja que carrega totes les variables no només les del jugador
        SaveManager.Instance.loadGame();

        loadPlayerVariables();
        healthBar.setMaxHealth(maxHealth);
        currentHealth = maxHealth;
        updateGold();
        woodText.SetText(wood.ToString());
        hasSecondChance = false;
        originalDamage = 10 + SaveVariables.ATTACK_LEVEL * 5;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChange -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    private void Update()
    {
        statueStats();
        if (usingController)
        {
            Cursor.visible = false;
            customCursor.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            customCursor.SetActive(true);
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            customCursor.transform.position = pos;
        }
        //cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(transform.position.x, transform.position.y, cam.transform.position.z), smoothFactor/100);
        if (pathRenderer)
        {
            if (Time.time > nextPoint)
            {
                nextPoint = Time.time + timeBetweenPointsInRute;
                ruteFollowed.positionCount = 9999;
                for (int i = posIndex; i < ruteFollowed.positionCount; i++)
                {
                    ruteFollowed.SetPosition(i, transform.position);
                }

                posIndex++;
            }
        }

        #region counter

        timer -= Time.deltaTime;
        seconds = (int)timer % 60;
        counterText.SetText(seconds + "s");

        #endregion counter

        //If we are shielded or immune we ignore the collisions with enemies so we can go through them
        if (immune)
        {
            if (!shielded)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
            }
            else
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
            }
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
        }

        if (aimPos.magnitude != 0)
        {
            crossHair.transform.position = transform.position + new Vector3(aimPos.x, aimPos.y);
        }

        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);

        //We check collision with coins and collect them
        Collider2D[] coins = Physics2D.OverlapCircleAll(transform.position, coinMagnetRange);

        if (coins.Length > 0)
        {
            coinMagnet(coins);
        }

        die();

        #region Dash

        if ((Input.GetAxisRaw("Dash") != 0 || Input.GetKey(KeyCode.LeftShift)) && !shielded)
        {
            if (Time.time > nextDash && !dashing && movement != Vector2.zero)
            {
                Statistics.Instance.dashesDone += 1;
                audioSource[DASH_AUDIO].clip = audios[1];
                audioSource[DASH_AUDIO].Play();
                nextDash = Time.time + dashRestoreTime;
                dashing = true;
                immune = true;
                dashDirection = movement;
                dashSpeed = dashDirection.normalized * dashForce;
                if (movement.x > 0)
                {
                    dashSmoke.GetComponent<SpriteRenderer>().flipX = false;
                    Instantiate(dashSmoke, transform.position, Quaternion.identity);
                }
                else
                {
                    dashSmoke.GetComponent<SpriteRenderer>().flipX = true;
                    Instantiate(dashSmoke, transform.position, Quaternion.identity);
                }
            }
        }

        #endregion Dash

        #region Movement and Attack

        if (!dashing)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (usingController)
            {
                crossHair.SetActive(true);
                aimPos.x = Input.GetAxisRaw("HorizontalAim");
                aimPos.y = Input.GetAxisRaw("VerticalAim");
                Vector2 tempAim = aimPos;
                aimPos = GetConstrainedPosition(Vector2.zero, tempAim);
            }
            else
            {
                crossHair.SetActive(false);
                Vector3 mPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                aimPos.x = mPos.x;
                aimPos.y = mPos.y;
            }

            if (movement.x != 0 || movement.y != 0)
            {
            }
            else
            {
                walkParticles.Play();
            }

            if (movement.magnitude != 0)
            {
                timePassedStep += Time.deltaTime;
                if (timePassedStep > timeToStep)
                {
                    timePassedStep = 0;

                    audioSource[FOOTSTEP_AUDIO].loop = false;
                    audioSource[FOOTSTEP_AUDIO].clip = audios[2];
                    audioSource[FOOTSTEP_AUDIO].Play();
                }
            }

            if (aimPos.magnitude <= 0)
            {
                playerMovement(movement);
            }
            else
            {
                playerMovement(aimPos);
            }

            if (!(aimPos.x == 0 && aimPos.y == 0))
            {
                firePoint.localPosition = new Vector3(Mathf.Clamp(aimPos.x, -0.6f, 0.6f), Mathf.Clamp(aimPos.y, -0.6f, 0.6f), 0);
            }

            if (Input.GetButton("Attack"))
            {
                if (battleaxe)
                {
                    battleCircleLoad.SetActive(true);
                    battleCircleLoadMaximum.SetActive(true);
                    battleCircleLoad.transform.localScale = new Vector3(1, 1, 1);
                    battlePressing = true;
                    battleTimePressed += Time.deltaTime;

                    if (battleTimePressed > 0.5f && battleTimePressed < 1f)
                    {
                        battleCircleLoad.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        bulletsToShoot = 2;
                    }
                    else if (battleTimePressed > 1f && battleTimePressed < 1.5f)
                    {
                        battleCircleLoad.transform.localScale = new Vector3(2f, 2f, 2f);
                        bulletsToShoot = 3;
                    }
                    else if (battleTimePressed > 1.5f && battleTimePressed < 3f)
                    {
                        battleCircleLoad.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                        bulletsToShoot = 4;
                    }
                    else if (battleTimePressed > 3f)
                    {
                        battleCircleLoad.transform.localScale = new Vector3(3f, 3f, 3f);
                        bulletsToShoot = 7;
                    }
                    else
                    {
                        bulletsToShoot = 1;
                    }
                }
                else
                {
                    Shoot();
                }
            }
            else
            {
                if (battleaxe && battleTimePressed > 0)
                {
                    battleCircleLoad.transform.localScale = new Vector3(1f, 1f, 1f);
                    battleCircleLoad.SetActive(false);
                    battleCircleLoadMaximum.SetActive(false);
                    Shoot();
                    bulletsToShoot = 0;
                    battleTimePressed = 0;
                }
            }
        }

        #endregion Movement and Attack
    }

    //We are checking if the player is moving or not
    private bool checkMovement()
    {
        dist = transform.position - lastUpdatePos;
        curentSpeed = dist.magnitude / Time.deltaTime;
        lastUpdatePos = transform.position;

        if (curentSpeed > 0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //We constraint the position of an object to create a circular constrained movement
    private Vector2 GetConstrainedPosition(Vector2 midPoint, Vector2 endPoint)
    {
        //get the length of the line
        float dist = Vector2.Distance(midPoint, endPoint);

        //Check for max length
        if (Vector2.Distance(midPoint, endPoint) > 1)
        {
            //get the direction of the line between mid point and end point
            Vector2 dirVector = endPoint - midPoint;

            //normalize
            dirVector.Normalize();

            //return the clamped position
            return (dirVector * 1) + midPoint;
        }
        //return the original position
        return endPoint;
    }

    private void FixedUpdate()
    {
        //Movement And Dash
        if (!dashing)
        {
            rb.MovePosition(rb.position + (movement.normalized * moveSpeed * Time.fixedDeltaTime));
            dashParticles.Stop();
        }
        else
        {
            dashParticles.Play();
            rb.MovePosition(rb.position + (dashSpeed * Time.fixedDeltaTime));
            dashSpeed *= 0.9f;
            if (dashSpeed.magnitude < moveSpeed)
            {
                dashing = false;
                immune = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Coin")
        {
            gold += (1 * goldMultiplier);
            Statistics.Instance.goldCollected += (1 * goldMultiplier);
            updateGold();
            SaveVariables.PLAYER_GOLD = gold;
            coinSound.name = "CoinSounding";
            Instantiate(coinSound, transform);
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Coin2")
        {
            gold += (3 * goldMultiplier);
            Statistics.Instance.goldCollected += (3 * goldMultiplier);
            updateGold();
            SaveVariables.PLAYER_GOLD = gold;
            coinSound.name = "CoinSounding";
            Instantiate(coinSound, transform);
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Coin3")
        {
            gold += (5 * goldMultiplier);
            Statistics.Instance.goldCollected += (5 * goldMultiplier);
            updateGold();
            SaveVariables.PLAYER_GOLD = gold;
            coinSound.name = "CoinSounding";
            Instantiate(coinSound, transform);
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Wood")
        {
            wood += (1 * woodMultiplier);
            SaveVariables.PLAYER_WOOD += (1 * woodMultiplier);
            woodText.SetText(wood.ToString());
            Destroy(collision.gameObject);
            audioSource[PICKUP_AUDIO].clip = audios[PICKUP_AUDIO];
            audioSource[PICKUP_AUDIO].Play();
        }

        ItemWorld iw = collision.transform.GetComponent<ItemWorld>();
        if (iw != null)
        {
            pickUpParticles.gameObject.transform.position = collision.transform.position;
            if (iw.getItem().itemType == Item.ItemType.smallPotion || iw.getItem().itemType == Item.ItemType.bigPotion)
            {
                pickUpParticles.startColor = Color.red;
            }
            else if (iw.getItem().itemType == Item.ItemType.shieldPotion)
            {
                pickUpParticles.startColor = Color.cyan;
            }
            else if (iw.getItem().itemType == Item.ItemType.goldPotion)
            {
                pickUpParticles.startColor = Color.yellow;
            }
            else if (iw.getItem().itemType == Item.ItemType.timePotion)
            {
                pickUpParticles.startColor = new Color(0, 0, 0);
            }
            else if (iw.getItem().itemType == Item.ItemType.teleportPotion)
            {
                pickUpParticles.startColor = Color.magenta;
            }
            else
            {
                pickUpParticles.startColor = new Color(255, 255, 255);
            }

            pickUpParticles.Play();
            audioSource[ATTACK_AUDIO].clip = audios[PICKUP_AUDIO];
            audioSource[ATTACK_AUDIO].Play();
            inventory.addItem(iw.getItem());

            switch (iw.getItem().itemType)
            {
                case Item.ItemType.smallPotion:
                    SaveVariables.INV_SMALL_POTION += iw.getItem().amount;
                    break;

                case Item.ItemType.bigPotion:
                    SaveVariables.INV_BIG_POTION += iw.getItem().amount;
                    break;

                case Item.ItemType.shieldPotion:
                    SaveVariables.INV_SHIELD_POTION += iw.getItem().amount;
                    break;

                case Item.ItemType.goldPotion:
                    SaveVariables.INV_GOLD_POTION += iw.getItem().amount;
                    break;

                case Item.ItemType.timePotion:
                    SaveVariables.INV_TIME_POTION += iw.getItem().amount;
                    break;

                case Item.ItemType.teleportPotion:
                    SaveVariables.INV_TELEPORT_POTION += iw.getItem().amount;
                    break;

                case Item.ItemType.basicAxe:
                    SaveVariables.INV_BASIC_AXE = iw.getItem().amount;
                    break;

                case Item.ItemType.seekAxe:
                    SaveVariables.INV_SEEK_AXE = iw.getItem().amount;
                    break;

                case Item.ItemType.bloodAxe:
                    SaveVariables.INV_BLOOD_AXE = iw.getItem().amount;
                    break;

                case Item.ItemType.multiAxe:
                    SaveVariables.INV_MULTIAXE = iw.getItem().amount;
                    break;

                case Item.ItemType.doubleAxe:
                    SaveVariables.INV_DOUBLE_AXE = iw.getItem().amount;
                    break;

                case Item.ItemType.battleAxe:
                    SaveVariables.INV_BATTLE_AXE = iw.getItem().amount;
                    break;

                case Item.ItemType.nysthelAxe:
                    SaveVariables.INV_NYSTHEL_AXE = iw.getItem().amount;
                    break;

                case Item.ItemType.trueAxe:
                    SaveVariables.INV_TRUE_AXE = iw.getItem().amount;
                    break;

                case Item.ItemType.shield:
                    SaveVariables.PLAYER_DEFENSE = iw.getItem().amount;
                    break;
            }

            iw.destroySelf();
        }
    }

    public bool isImmune()
    {
        return immune;
    }

    private void playerMovement(Vector2 dir)
    {
        if (checkMovement())
        {
            if (dir.x >= 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            if (dir.y > 0 && Mathf.Abs(dir.y) >= Mathf.Abs(dir.x))
            {
                if (!attacking)
                {
                    changeAnimationState("Nysthel_walkUp");
                }
            }
            else if (dir.y < 0)
            {
                if (!attacking)
                {
                    changeAnimationState("Nysthel_walk");
                }
            }
            else
            {
                changeAnimationState("Nysthel_walk");
            }
        }
        else if (!attacking)
        {
            changeAnimationState("Nysthel_idle");
        }
    }

    private void Shoot()
    {
        int tempDamage = 0;
        int DPSdamage = 0;
        if (Time.time > nextFire)
        {
            Statistics.Instance.attacksDone += 1;

            //Diferent time attack settup
            if (bloodyaxe)
            {
                tempDamage = damage + bloodyAxeDamageIncrement;
                DPSdamage = tempDamage;
                nextFire = Time.time + attackRate + bloodyAxeRateIncrement;
            }
            else if (multiaxe)
            {
                tempDamage = damage;
                DPSdamage = tempDamage * 4;
                nextFire = Time.time + attackRate + multiAxeRateIncrement;
            }
            else if (doubleaxe)
            {
                tempDamage = damage;
                DPSdamage = tempDamage * 2;
                nextFire = Time.time + attackRate + doubleAxeRateIncrement;
            }
            else if (seekaxe)
            {
                tempDamage = damage + seekAxeDamageIncrement;
                DPSdamage = tempDamage;
                nextFire = Time.time + attackRate + seekAxeRateIncrement;
            }
            else if (battleaxe)
            {
                tempDamage = damage + battleAxeDamageIncrement;
                DPSdamage = tempDamage;
                nextFire = Time.time + attackRate + battleAxeRateIncrement;
            }
            else if (nysthelaxe)
            {
                tempDamage = damage + nysthelAxeDamageIncrement;
                DPSdamage = tempDamage;
                nextFire = Time.time + attackRate + nysthelAxeRateIncrement;
            }
            else if (trueaxe)
            {
                tempDamage = damage + trueAxeDamageIncrement;
                DPSdamage = tempDamage;
                nextFire = Time.time + attackRate + trueAxeRateIncrement;
            }
            else
            {
                tempDamage = damage;
                DPSdamage = tempDamage;
                nextFire = Time.time + attackRate + basicAxeRateIncrement;
            }

            if (dpsDebug)
            {
                dpsDebugger.gameObject.SetActive(true);
                dpsDebugger.SetText("Max DPS: " + Mathf.RoundToInt(DPSdamage * (1 / (nextFire - Time.time))));
                //Debug.Log("Max DPS: " + tempDamage * (1 / (nextFire - Time.time)));
            }
            else
            {
                dpsDebugger.gameObject.SetActive(false);
            }
            //Ficar un so per a cada arma de moment un per totes
            audioSource[ATTACK_AUDIO].clip = audios[0];
            audioSource[ATTACK_AUDIO].Play();

            if (aimPos.y <= 0)
            {
                changeAnimationState("Nysthel_Attack");
            }
            else
            {
                changeAnimationState("Nysthel_AttackUp");
            }

            if (multiaxe)
            {
                lookDir = firePoint.position - transform.position;
                float angleSpread = 45;
                int numberOfProjectiles = 3;
                float facingRotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                float startRotation = facingRotation + angleSpread / 2f;
                float angleIncrease = angleSpread / ((float)numberOfProjectiles - 1);

                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    float tempRot = startRotation - angleIncrease * i;
                    Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                    bullet.setDirection(new Vector2(Mathf.Cos(tempRot * Mathf.Deg2Rad), Mathf.Sin(tempRot * Mathf.Deg2Rad)));
                    bullet.setDamage(tempDamage);
                }
            }
            else if (doubleaxe)
            {
                Invoke("generateSecondBullet", 0.2f);
                directionToShoot = (firePoint.position - transform.position).normalized;
                attacking = true;
                Invoke("stopAttacking", animationDelay);

                Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.setDirection(directionToShoot);
                bullet.setDamage(tempDamage);
            }
            else if (basicaxe)
            {
                directionToShoot = (firePoint.position - transform.position).normalized;
                attacking = true;
                Invoke("stopAttacking", animationDelay);

                Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.setDirection(directionToShoot);
                bullet.setDamage(tempDamage);
            }
            else if (bloodyaxe)
            {
                directionToShoot = (firePoint.position - transform.position).normalized;
                attacking = true;
                Invoke("stopAttacking", animationDelay);

                Bullet bullet = Instantiate(bloodyBulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.setDirection(directionToShoot);
                bullet.setDamage(tempDamage);
            }
            else if (seekaxe)
            {
                directionToShoot = (firePoint.position - transform.position).normalized;
                attacking = true;
                Invoke("stopAttacking", animationDelay);

                Bullet bullet = Instantiate(seekBulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.setDirection(directionToShoot);
                bullet.setDamage(tempDamage);
                bullet.isSeeker = true;
            }
            else if (battleaxe)
            {
                for (int i = 0; i < bulletsToShoot; i++)
                {
                    Invoke("battleBullets", 0.25f * i);
                }
                bulletsToShoot = 0;
            }
            else if (nysthelaxe)
            {
                directionToShoot = (firePoint.position - transform.position).normalized;
                attacking = true;
                Invoke("stopAttacking", animationDelay);

                Bullet bullet = Instantiate(nysthelBulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.setDirection(directionToShoot);
                bullet.setDamage(tempDamage);
            }
            else if (trueaxe)
            {
                directionToShoot = (firePoint.position - transform.position).normalized;
                attacking = true;
                Invoke("stopAttacking", animationDelay);

                Bullet bullet = Instantiate(trueBulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.setDirection(directionToShoot);
                bullet.setDamage(tempDamage);
            }
        }
    }

    private void battleBullets()
    {
        directionToShoot = (firePoint.position - transform.position).normalized;
        attacking = true;
        Invoke("stopAttacking", animationDelay);
        Bullet bullet = Instantiate(battleBulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.setDirection(directionToShoot);
        bullet.setDamage(damage + battleAxeDamageIncrement);
        bullet.isSeeker = true;
    }

    private void generateSecondBullet()
    {
        audioSource[ATTACK_AUDIO].Play();
        directionToShoot = (firePoint.position - transform.position).normalized;
        attacking = true;
        Invoke("stopAttacking", animationDelay);

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.setDamage(damage + doubleAxeDamageIncrement);
        bullet.setDirection(directionToShoot);
    }

    private void coinMagnet(Collider2D[] coins)
    {
        foreach (Collider2D coin in coins)
        {
            if (coin.tag == "Coin" || coin.tag == "Wood" || coin.tag == "Coin2" || coin.tag == "Coin3")
            {
                coin.transform.Translate((transform.position - coin.transform.position).normalized * coinMagnetSpeed * Time.deltaTime);
            }
        }
    }

    private void stopAttacking()
    {
        attacking = false;
    }

    private void changeAnimationState(string newState)
    {
        //We avoid playing the same animation multiple times
        if (currentState == newState) return;

        //We play a determinated animation
        anim.Play(newState);

        currentState = newState;
    }

    public void takeDamage(int value)
    {
        if (!immune)
        {
            feedBackScreenPanel.GetComponent<Animator>().Play("DamageAnimation");
            currentHealth -= Mathf.RoundToInt(value - ((defense / 100f) * value));
            immune = true;
            shield.SetActive(true);
            //shielded = true;
            audioSource[3].clip = audios[DAMAGE_AUDIO];
            audioSource[3].pitch = Random.Range(0.75f, 1.25f);
            audioSource[3].Play();
            damageParticles.Play();
            Instantiate(bloodDecal, transform.position, Quaternion.identity);
            Invoke("notImmunity", immunityTime);
        }
        healthBar.setHealth(currentHealth);
    }

    private void notImmunity()
    {
        shield.SetActive(false);
        immune = false;
        shielded = false;
    }

    private void die()
    {
        if (currentHealth <= 0)
        {
            if (Random.Range(0f, 1f) <= secondChanceProbability)
            {
                hasSecondChance = true;
            }

            if (hasSecondChance && SceneManager.GetActiveScene().name != "WoodFarm" && SceneManager.GetActiveScene().name != "GoldRush")
            {
                saveInventory();
                SaveManager.Instance.SaveGame();
                SceneManager.LoadScene("SecondChanceChallenge");
            }
            else
            {
                if (SceneManager.GetActiveScene().name != "WoodFarm" && SceneManager.GetActiveScene().name != "GoldRush")
                {
                    SaveVariables.clearInventory();
                    gold -= Mathf.RoundToInt(0.6f * gold);
                    SaveVariables.PLAYER_GOLD = gold;
                }
                SaveManager.Instance.SaveGame();
                isDead = true;
                Statistics.Instance.showStatistics();
                Time.timeScale = 0f;
                GameStateManager.Instance.SetState(GameState.Paused);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, coinMagnetRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + (Vector3)movement*2, 1f);
    }

    private void statueStats()
    {
        if (SaveVariables.DAMAGE_STATUE == 2 && !damageStatueApplied)
        {
            damage = originalDamage + (Mathf.RoundToInt((originalDamage) / 2));
            damageStatueApplied = true;
        }
        else if (SaveVariables.DAMAGE_STATUE == 1 && damageStatueApplied)
        {
            damage = originalDamage;
            damageStatueApplied = false;
        }

        if (SaveVariables.EMMYR_STATUE == 2)
        {
            //Simplement al final del joc checkejar si esta a 1 i habilitar un final o un altre.
        }
        if (SaveVariables.GOLD_STATUE == 2 && !goldStatueApplied)
        {
            goldMultiplier = goldMultiplier + 1;
            goldMultiplierUI.GetComponent<TextMeshProUGUI>().SetText("x" + goldMultiplier);
            goldMultiplierUI.SetActive(true);
            goldStatueApplied = true;
        }
        else if (goldStatueApplied && SaveVariables.GOLD_STATUE == 1)
        {
            goldMultiplierUI.SetActive(false);
            goldMultiplier = goldMultiplier - 1;
            goldStatueApplied = false;
        }

        if (SaveVariables.HOLY_STATUE == 2)
        {
            //On the start room create more probability for objects
        }

        if (SaveVariables.CHANCE_STATUE == 2)
        {
            secondChanceProbability = 0.3f;
        }
        else if (SaveVariables.CHANCE_STATUE == 1)
        {
            secondChanceProbability = 0.1f;
        }
    }

    #region Item Usage

    public void UseItem(Item item)
    {
        if (item.isStackable())
        {
            switch (item.itemType)
            {
                //Functionality for each item
                case Item.ItemType.smallPotion:
                    if (currentHealth < maxHealth)
                    {
                        if (currentHealth + 10 > maxHealth)
                        {
                            currentHealth = maxHealth;
                        }
                        else
                        {
                            currentHealth += 10;
                        }
                        audioSource[PICKUP_AUDIO].clip = audios[7];
                        audioSource[PICKUP_AUDIO].Play();
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.smallPotion, amount = 1 });
                        SaveVariables.INV_SMALL_POTION--;
                    }
                    else
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[6];
                        audioSource[PICKUP_AUDIO].Play();
                    }
                    break;

                case Item.ItemType.bigPotion:
                    if (currentHealth < maxHealth)
                    {
                        if (currentHealth + 20 > maxHealth)
                        {
                            currentHealth = maxHealth;
                        }
                        else
                        {
                            currentHealth += 20;
                        }
                        audioSource[PICKUP_AUDIO].clip = audios[7];
                        audioSource[PICKUP_AUDIO].Play();
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.bigPotion, amount = 1 });
                        SaveVariables.INV_BIG_POTION--;
                    }
                    else
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[6];
                        audioSource[PICKUP_AUDIO].Play();
                    }
                    break;

                case Item.ItemType.shieldPotion:
                    if (!shielded && !goldRushed && !timeSlowed)
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[7];
                        audioSource[PICKUP_AUDIO].Play();
                        shield.SetActive(true);
                        immune = true;
                        Invoke("endShield", shieldDuration);
                        Invoke("notImmunity", shieldDuration);
                        shielded = true;
                        seconds = (int)shieldDuration;
                        timer = (int)shieldDuration;
                        counterText.gameObject.SetActive(true);
                        counterText.GetComponent<TextMeshProUGUI>().color = Color.cyan;
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.shieldPotion, amount = 1 });
                        SaveVariables.INV_SHIELD_POTION--;
                    }
                    else
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[6];
                        audioSource[PICKUP_AUDIO].Play();
                    }
                    break;

                case Item.ItemType.goldPotion:
                    if (!goldRushed && !shielded && !timeSlowed)
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[7];
                        audioSource[PICKUP_AUDIO].Play();
                        goldMultiplier = goldMultiplier + 1;
                        goldMultiplierUI.GetComponent<TextMeshProUGUI>().SetText("x" + goldMultiplier);
                        goldMultiplierUI.SetActive(true);
                        goldRush.SetActive(true);
                        goldRushed = true;
                        seconds = (int)goldPotionDuration;
                        timer = (int)goldPotionDuration;
                        counterText.gameObject.SetActive(true);
                        counterText.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                        Invoke("endGoldPotion", goldPotionDuration);
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.goldPotion, amount = 1 });
                        SaveVariables.INV_GOLD_POTION--;
                    }
                    else
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[6];
                        audioSource[PICKUP_AUDIO].Play();
                    }
                    break;

                case Item.ItemType.teleportPotion:
                    GameObject tpPoint = GameObject.FindGameObjectWithTag("Respawn");
                    if (tpPoint != null)
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[7];
                        audioSource[PICKUP_AUDIO].Play();
                        transform.position = tpPoint.transform.position;
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.teleportPotion, amount = 1 });
                        SaveVariables.INV_TELEPORT_POTION--;
                    }
                    else
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[6];
                        audioSource[PICKUP_AUDIO].Play();
                    }
                    break;

                case Item.ItemType.timePotion:
                    if (!timeSlowed && !shielded && !goldRushed)
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[7];
                        audioSource[PICKUP_AUDIO].Play();
                        timeSlowed = true;
                        seconds = (int)timePotionDuration;
                        timer = (int)timePotionDuration;

                        counterText.gameObject.SetActive(true);
                        counterText.GetComponent<TextMeshProUGUI>().color = Color.black;
                        Invoke("endTimePotion", timePotionDuration);
                        moveSpeed = moveSpeed * 2;
                        attackRate = attackRate / 2;
                        Time.timeScale = 0.5f;

                        inventory.RemoveItem(new Item { itemType = Item.ItemType.timePotion, amount = 1 });
                        SaveVariables.INV_TIME_POTION--;
                    }
                    else
                    {
                        audioSource[PICKUP_AUDIO].clip = audios[6];
                        audioSource[PICKUP_AUDIO].Play();
                    }
                    break;
            }
            healthBar.setHealth(currentHealth);
        }
        else
        {
            audioSource[PICKUP_AUDIO].clip = audios[7];
            audioSource[PICKUP_AUDIO].Play();
            switch (item.itemType)
            {
                case Item.ItemType.multiAxe:
                    if (doubleaxe) doubleaxe = false;
                    if (basicaxe) basicaxe = false;
                    if (bloodyaxe) bloodyaxe = false;
                    if (seekaxe) seekaxe = false;
                    if (battleaxe) battleaxe = false;
                    if (nysthelaxe) nysthelaxe = false;
                    if (trueaxe) trueaxe = false;
                    multiaxe = true;
                    if (SaveVariables.INV_BASIC_AXE == 2) SaveVariables.INV_BASIC_AXE = 1;
                    if (SaveVariables.INV_BLOOD_AXE == 2) SaveVariables.INV_BLOOD_AXE = 1;
                    if (SaveVariables.INV_SEEK_AXE == 2) SaveVariables.INV_SEEK_AXE = 1;
                    if (SaveVariables.INV_DOUBLE_AXE == 2) SaveVariables.INV_DOUBLE_AXE = 1;
                    if (SaveVariables.INV_BATTLE_AXE == 2) SaveVariables.INV_BATTLE_AXE = 1;
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) SaveVariables.INV_NYSTHEL_AXE = 1;
                    if (SaveVariables.INV_TRUE_AXE == 2) SaveVariables.INV_TRUE_AXE = 1;
                    SaveVariables.INV_MULTIAXE = 2;
                    break;

                case Item.ItemType.doubleAxe:
                    if (multiaxe) multiaxe = false;
                    if (basicaxe) basicaxe = false;
                    if (bloodyaxe) bloodyaxe = false;
                    if (seekaxe) seekaxe = false;
                    if (battleaxe) battleaxe = false;
                    if (nysthelaxe) nysthelaxe = false;
                    if (trueaxe) trueaxe = false;
                    doubleaxe = true;
                    if (SaveVariables.INV_BASIC_AXE == 2) SaveVariables.INV_BASIC_AXE = 1;
                    if (SaveVariables.INV_BLOOD_AXE == 2) SaveVariables.INV_BLOOD_AXE = 1;
                    if (SaveVariables.INV_SEEK_AXE == 2) SaveVariables.INV_SEEK_AXE = 1;
                    if (SaveVariables.INV_MULTIAXE == 2) SaveVariables.INV_MULTIAXE = 1;
                    if (SaveVariables.INV_BATTLE_AXE == 2) SaveVariables.INV_BATTLE_AXE = 1;
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) SaveVariables.INV_NYSTHEL_AXE = 1;
                    if (SaveVariables.INV_TRUE_AXE == 2) SaveVariables.INV_TRUE_AXE = 1;
                    SaveVariables.INV_DOUBLE_AXE = 2;
                    break;

                case Item.ItemType.basicAxe:
                    if (multiaxe) multiaxe = false;
                    if (doubleaxe) doubleaxe = false;
                    if (bloodyaxe) bloodyaxe = false;
                    if (seekaxe) seekaxe = false;
                    if (battleaxe) battleaxe = false;
                    if (nysthelaxe) nysthelaxe = false;
                    if (trueaxe) trueaxe = false;
                    basicaxe = true;
                    if (SaveVariables.INV_DOUBLE_AXE == 2) SaveVariables.INV_DOUBLE_AXE = 1;
                    if (SaveVariables.INV_BLOOD_AXE == 2) SaveVariables.INV_BLOOD_AXE = 1;
                    if (SaveVariables.INV_SEEK_AXE == 2) SaveVariables.INV_SEEK_AXE = 1;
                    if (SaveVariables.INV_MULTIAXE == 2) SaveVariables.INV_MULTIAXE = 1;
                    if (SaveVariables.INV_BATTLE_AXE == 2) SaveVariables.INV_BATTLE_AXE = 1;
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) SaveVariables.INV_NYSTHEL_AXE = 1;
                    if (SaveVariables.INV_TRUE_AXE == 2) SaveVariables.INV_TRUE_AXE = 1;
                    SaveVariables.INV_BASIC_AXE = 2;
                    break;

                case Item.ItemType.bloodAxe:
                    if (multiaxe) multiaxe = false;
                    if (doubleaxe) doubleaxe = false;
                    if (basicaxe) basicaxe = false;
                    if (seekaxe) seekaxe = false;
                    if (battleaxe) battleaxe = false;
                    if (nysthelaxe) nysthelaxe = false;
                    if (trueaxe) trueaxe = false;
                    bloodyaxe = true;
                    if (SaveVariables.INV_DOUBLE_AXE == 2) SaveVariables.INV_DOUBLE_AXE = 1;
                    if (SaveVariables.INV_BASIC_AXE == 2) SaveVariables.INV_BASIC_AXE = 1;
                    if (SaveVariables.INV_SEEK_AXE == 2) SaveVariables.INV_SEEK_AXE = 1;
                    if (SaveVariables.INV_MULTIAXE == 2) SaveVariables.INV_MULTIAXE = 1;
                    if (SaveVariables.INV_BATTLE_AXE == 2) SaveVariables.INV_BATTLE_AXE = 1;
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) SaveVariables.INV_NYSTHEL_AXE = 1;
                    if (SaveVariables.INV_TRUE_AXE == 2) SaveVariables.INV_TRUE_AXE = 1;
                    SaveVariables.INV_BLOOD_AXE = 2;
                    break;

                case Item.ItemType.seekAxe:
                    if (multiaxe) multiaxe = false;
                    if (doubleaxe) doubleaxe = false;
                    if (basicaxe) basicaxe = false;
                    if (bloodyaxe) bloodyaxe = false;
                    if (battleaxe) battleaxe = false;
                    if (nysthelaxe) nysthelaxe = false;
                    if (trueaxe) trueaxe = false;
                    seekaxe = true;
                    if (SaveVariables.INV_DOUBLE_AXE == 2) SaveVariables.INV_DOUBLE_AXE = 1;
                    if (SaveVariables.INV_BASIC_AXE == 2) SaveVariables.INV_BASIC_AXE = 1;
                    if (SaveVariables.INV_BLOOD_AXE == 2) SaveVariables.INV_BLOOD_AXE = 1;
                    if (SaveVariables.INV_MULTIAXE == 2) SaveVariables.INV_MULTIAXE = 1;
                    if (SaveVariables.INV_BATTLE_AXE == 2) SaveVariables.INV_BATTLE_AXE = 1;
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) SaveVariables.INV_NYSTHEL_AXE = 1;
                    if (SaveVariables.INV_TRUE_AXE == 2) SaveVariables.INV_TRUE_AXE = 1;
                    SaveVariables.INV_SEEK_AXE = 2;
                    break;

                case Item.ItemType.battleAxe:
                    if (multiaxe) multiaxe = false;
                    if (doubleaxe) doubleaxe = false;
                    if (basicaxe) basicaxe = false;
                    if (bloodyaxe) bloodyaxe = false;
                    if (seekaxe) seekaxe = false;
                    if (nysthelaxe) nysthelaxe = false;
                    if (trueaxe) trueaxe = false;
                    battleaxe = true;
                    if (SaveVariables.INV_DOUBLE_AXE == 2) SaveVariables.INV_DOUBLE_AXE = 1;
                    if (SaveVariables.INV_BASIC_AXE == 2) SaveVariables.INV_BASIC_AXE = 1;
                    if (SaveVariables.INV_BLOOD_AXE == 2) SaveVariables.INV_BLOOD_AXE = 1;
                    if (SaveVariables.INV_MULTIAXE == 2) SaveVariables.INV_MULTIAXE = 1;
                    if (SaveVariables.INV_SEEK_AXE == 2) SaveVariables.INV_SEEK_AXE = 1;
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) SaveVariables.INV_NYSTHEL_AXE = 1;
                    if (SaveVariables.INV_TRUE_AXE == 2) SaveVariables.INV_TRUE_AXE = 1;
                    SaveVariables.INV_BATTLE_AXE = 2;
                    break;

                case Item.ItemType.nysthelAxe:
                    if (multiaxe) multiaxe = false;
                    if (doubleaxe) doubleaxe = false;
                    if (basicaxe) basicaxe = false;
                    if (bloodyaxe) bloodyaxe = false;
                    if (battleaxe) battleaxe = false;
                    if (seekaxe) seekaxe = false;
                    if (trueaxe) trueaxe = false;
                    nysthelaxe = true;
                    if (SaveVariables.INV_DOUBLE_AXE == 2) SaveVariables.INV_DOUBLE_AXE = 1;
                    if (SaveVariables.INV_BASIC_AXE == 2) SaveVariables.INV_BASIC_AXE = 1;
                    if (SaveVariables.INV_BLOOD_AXE == 2) SaveVariables.INV_BLOOD_AXE = 1;
                    if (SaveVariables.INV_MULTIAXE == 2) SaveVariables.INV_MULTIAXE = 1;
                    if (SaveVariables.INV_BATTLE_AXE == 2) SaveVariables.INV_BATTLE_AXE = 1;
                    if (SaveVariables.INV_SEEK_AXE == 2) SaveVariables.INV_SEEK_AXE = 1;
                    if (SaveVariables.INV_TRUE_AXE == 2) SaveVariables.INV_TRUE_AXE = 1;
                    SaveVariables.INV_NYSTHEL_AXE = 2;
                    break;

                case Item.ItemType.trueAxe:
                    if (multiaxe) multiaxe = false;
                    if (doubleaxe) doubleaxe = false;
                    if (basicaxe) basicaxe = false;
                    if (bloodyaxe) bloodyaxe = false;
                    if (battleaxe) battleaxe = false;
                    if (nysthelaxe) nysthelaxe = false;
                    if (seekaxe) seekaxe = false;
                    trueaxe = true;
                    if (SaveVariables.INV_DOUBLE_AXE == 2) SaveVariables.INV_DOUBLE_AXE = 1;
                    if (SaveVariables.INV_BASIC_AXE == 2) SaveVariables.INV_BASIC_AXE = 1;
                    if (SaveVariables.INV_BLOOD_AXE == 2) SaveVariables.INV_BLOOD_AXE = 1;
                    if (SaveVariables.INV_MULTIAXE == 2) SaveVariables.INV_MULTIAXE = 1;
                    if (SaveVariables.INV_BATTLE_AXE == 2) SaveVariables.INV_BATTLE_AXE = 1;
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) SaveVariables.INV_NYSTHEL_AXE = 1;
                    if (SaveVariables.INV_SEEK_AXE == 2) SaveVariables.INV_SEEK_AXE = 1;
                    SaveVariables.INV_TRUE_AXE = 2;
                    break;
            }
        }
    }

    private void endTimePotion()
    {
        timeSlowed = false;
        counterText.gameObject.SetActive(false);
        moveSpeed = moveSpeed / 2;
        attackRate = attackRate * 2;
        Time.timeScale = 1f;
    }

    private void endGoldPotion()
    {
        goldMultiplier = 1;
        goldRushed = false;
        goldRush.SetActive(false);
        goldMultiplierUI.SetActive(false);
        counterText.gameObject.SetActive(false);
    }

    #endregion Item Usage

    private void endShield()
    {
        shield.SetActive(false);
        shielded = false;
        counterText.gameObject.SetActive(false);
    }

    //Player Upgrades
    public int BoughtItem(ShopItem.ItemType itemType)
    {
        int index = 0;
        switch (itemType)
        {
            case ShopItem.ItemType.LifeUpgrade:
                maxHealth += (int)ShopItem.GetImprovementQuantity(ShopItem.ItemType.LifeUpgrade);
                SaveVariables.PLAYER_LIFE = maxHealth;
                SaveVariables.LIFE_LEVEL = ShopItem.GetCurrentLevel(itemType);
                healthBar.setMaxHealth(maxHealth);
                currentHealth = maxHealth;
                index = 0;
                break;

            case ShopItem.ItemType.AttackUpgrade:
                damage += (int)ShopItem.GetImprovementQuantity(ShopItem.ItemType.AttackUpgrade);
                SaveVariables.PLAYER_ATTACK = damage;
                SaveVariables.ATTACK_LEVEL = ShopItem.GetCurrentLevel(itemType);
                originalDamage = damage;
                index = 1;
                break;

            case ShopItem.ItemType.SpeedUpgrade:
                moveSpeed += ShopItem.GetImprovementQuantity(ShopItem.ItemType.SpeedUpgrade);
                SaveVariables.PLAYER_SPEED = moveSpeed;
                SaveVariables.SPEED_LEVEL = ShopItem.GetCurrentLevel(itemType);
                index = 2;
                break;

            case ShopItem.ItemType.AttackSpeedUpgrade:
                attackRate -= ShopItem.GetImprovementQuantity(ShopItem.ItemType.AttackSpeedUpgrade);
                SaveVariables.PLAYER_ATTACK_SPEED = attackRate;
                SaveVariables.ATTACK_SPEED_LEVEL = ShopItem.GetCurrentLevel(itemType);
                index = 3;
                break;

            case ShopItem.ItemType.RangeUpgrade:
                coinMagnetRange += ShopItem.GetImprovementQuantity(ShopItem.ItemType.RangeUpgrade);
                SaveVariables.PLAYER_RANGE = coinMagnetRange;
                SaveVariables.RANGE_LEVEL = ShopItem.GetCurrentLevel(itemType);
                index = 4;
                break;

            case ShopItem.ItemType.DashRecoveryUpgrade:
                dashRestoreTime -= ShopItem.GetImprovementQuantity(ShopItem.ItemType.DashRecoveryUpgrade);
                SaveVariables.PLAYER_DASH_RECOVERY = dashRestoreTime;
                SaveVariables.DASH_RECOVERY_LEVEL = ShopItem.GetCurrentLevel(itemType);
                index = 5;
                break;

            case ShopItem.ItemType.DashRangeUpgrade:
                dashForce += ShopItem.GetImprovementQuantity(ShopItem.ItemType.DashRangeUpgrade);
                SaveVariables.PLAYER_DASH_RANGE = dashForce;
                SaveVariables.DASH_RANGE_LEVEL = ShopItem.GetCurrentLevel(itemType);
                index = 6;
                break;
        }
        return index;
    }

    public void updateGold()
    {
        goldText.text = gold.ToString();
        woodText.text = wood.ToString();
        if (BlackSmithGoldText != null)
        {
            BlackSmithGoldText.text = gold.ToString();
        }
    }

    public bool TrySpendGoldAmount(int goldAmount)
    {
        if (gold >= goldAmount)
        {
            audioSource[PICKUP_AUDIO].clip = audios[7];
            audioSource[PICKUP_AUDIO].Play();
            gold -= goldAmount;
            updateGold();
            SaveVariables.PLAYER_GOLD = gold;
            return true;
        }
        else
        {
            //Error, no enough gold
            audioSource[PICKUP_AUDIO].clip = audios[6];
            audioSource[PICKUP_AUDIO].Play();
            return false;
        }
    }

    public float[] GetStatistics()
    {
        float[] s = new float[7];
        s[0] = damage;
        s[1] = maxHealth;
        s[2] = moveSpeed;
        s[3] = (1 / attackRate);
        s[4] = dashRestoreTime;
        s[5] = dashForce;
        s[6] = coinMagnetRange;
        return s;
    }

    public void saveInventory()
    {
        foreach (Item item in inventory.getItemList())
        {
            switch (item.itemType)
            {
                case Item.ItemType.smallPotion:
                    SaveVariables.INV_SMALL_POTION = item.amount;
                    break;

                case Item.ItemType.bigPotion:
                    SaveVariables.INV_BIG_POTION = item.amount;
                    break;

                case Item.ItemType.shieldPotion:
                    SaveVariables.INV_SHIELD_POTION = item.amount;
                    break;

                case Item.ItemType.goldPotion:
                    SaveVariables.INV_GOLD_POTION = item.amount;
                    break;

                case Item.ItemType.teleportPotion:
                    SaveVariables.INV_TELEPORT_POTION = item.amount;
                    break;

                case Item.ItemType.timePotion:
                    SaveVariables.INV_TIME_POTION = item.amount;
                    break;
            }
        }
    }

    public void playPositiveAction()
    {
        audioSource[PICKUP_AUDIO].clip = audios[7];
        audioSource[PICKUP_AUDIO].Play();
    }

    public void playNegativeAction()
    {
        audioSource[PICKUP_AUDIO].clip = audios[6];
        audioSource[PICKUP_AUDIO].Play();
    }

    public void loadPlayerVariables()
    {
        //Player Stats
        if (SaveVariables.PLAYER_USING_CONTROLLER == 0)
        {
            usingController = false;
        }
        else
        {
            usingController = true;
        }

        gold = SaveVariables.PLAYER_GOLD;
        wood = SaveVariables.PLAYER_WOOD;

        if (SaveVariables.PLAYER_ATTACK > 0) damage = SaveVariables.PLAYER_ATTACK;

        if (SaveVariables.PLAYER_DEFENSE > 0) defense = SaveVariables.PLAYER_DEFENSE;

        if (SaveVariables.PLAYER_LIFE > 0) maxHealth = SaveVariables.PLAYER_LIFE;

        if (SaveVariables.PLAYER_SPEED > 0) moveSpeed = SaveVariables.PLAYER_SPEED;

        if (SaveVariables.PLAYER_ATTACK_SPEED > 0) attackRate = SaveVariables.PLAYER_ATTACK_SPEED;

        if (SaveVariables.PLAYER_RANGE > 0) coinMagnetRange = SaveVariables.PLAYER_RANGE;

        if (SaveVariables.PLAYER_DASH_RECOVERY > 0) dashRestoreTime = SaveVariables.PLAYER_DASH_RECOVERY;

        if (SaveVariables.PLAYER_DASH_RANGE > 0) dashForce = SaveVariables.PLAYER_DASH_RANGE;

        //Inventory items

        System.Array a = System.Enum.GetValues(typeof(Item.ItemType));

        for (int i = 0; i < a.Length; i++)
        {
            Item it;
            switch (a.GetValue(i))
            {
                case Item.ItemType.smallPotion:
                    if (SaveVariables.INV_SMALL_POTION > 0) inventory.addItem(new Item { itemType = Item.ItemType.smallPotion, amount = SaveVariables.INV_SMALL_POTION });
                    break;

                case Item.ItemType.bigPotion:
                    if (SaveVariables.INV_BIG_POTION > 0) inventory.addItem(new Item { itemType = Item.ItemType.bigPotion, amount = SaveVariables.INV_BIG_POTION });
                    break;

                case Item.ItemType.shieldPotion:
                    if (SaveVariables.INV_SHIELD_POTION > 0) inventory.addItem(new Item { itemType = Item.ItemType.shieldPotion, amount = SaveVariables.INV_SHIELD_POTION });
                    break;

                case Item.ItemType.goldPotion:
                    if (SaveVariables.INV_GOLD_POTION > 0) inventory.addItem(new Item { itemType = Item.ItemType.goldPotion, amount = SaveVariables.INV_GOLD_POTION });
                    break;

                case Item.ItemType.teleportPotion:
                    if (SaveVariables.INV_TELEPORT_POTION > 0) inventory.addItem(new Item { itemType = Item.ItemType.teleportPotion, amount = SaveVariables.INV_TELEPORT_POTION });
                    break;

                case Item.ItemType.timePotion:
                    if (SaveVariables.INV_TIME_POTION > 0) inventory.addItem(new Item { itemType = Item.ItemType.timePotion, amount = SaveVariables.INV_TIME_POTION });
                    break;

                case Item.ItemType.basicAxe:
                    it = new Item { itemType = Item.ItemType.basicAxe, amount = SaveVariables.INV_BASIC_AXE };
                    if (SaveVariables.INV_BASIC_AXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_BASIC_AXE == 2) UseItem(it);
                    break;

                case Item.ItemType.doubleAxe:
                    it = new Item { itemType = Item.ItemType.doubleAxe, amount = SaveVariables.INV_DOUBLE_AXE };
                    if (SaveVariables.INV_DOUBLE_AXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_DOUBLE_AXE == 2) UseItem(it);
                    break;

                case Item.ItemType.multiAxe:
                    it = new Item { itemType = Item.ItemType.multiAxe, amount = SaveVariables.INV_MULTIAXE };
                    if (SaveVariables.INV_MULTIAXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_MULTIAXE == 2) UseItem(it);
                    break;

                case Item.ItemType.bloodAxe:
                    it = new Item { itemType = Item.ItemType.bloodAxe, amount = SaveVariables.INV_BLOOD_AXE };
                    if (SaveVariables.INV_BLOOD_AXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_BLOOD_AXE == 2) UseItem(it);
                    break;

                case Item.ItemType.seekAxe:
                    it = new Item { itemType = Item.ItemType.seekAxe, amount = SaveVariables.INV_SEEK_AXE };
                    if (SaveVariables.INV_SEEK_AXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_SEEK_AXE == 2) UseItem(it);
                    break;

                case Item.ItemType.battleAxe:
                    it = new Item { itemType = Item.ItemType.battleAxe, amount = SaveVariables.INV_BATTLE_AXE };
                    if (SaveVariables.INV_BATTLE_AXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_BATTLE_AXE == 2) UseItem(it);
                    break;

                case Item.ItemType.nysthelAxe:
                    it = new Item { itemType = Item.ItemType.nysthelAxe, amount = SaveVariables.INV_NYSTHEL_AXE };
                    if (SaveVariables.INV_NYSTHEL_AXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_NYSTHEL_AXE == 2) UseItem(it);
                    break;

                case Item.ItemType.trueAxe:
                    it = new Item { itemType = Item.ItemType.trueAxe, amount = SaveVariables.INV_TRUE_AXE };
                    if (SaveVariables.INV_TRUE_AXE > 0) inventory.addItem(it);
                    if (SaveVariables.INV_TRUE_AXE == 2) UseItem(it);
                    break;
            }
        }
    }

    public int BoughtItem(ItemShopItem.ItemType itemType)
    {
        int index = 0;
        switch (itemType)
        {
            case ItemShopItem.ItemType.smallHealthPotion:
                inventory.addItem(new Item { itemType = Item.ItemType.smallPotion, amount = 1 });
                index = 0;
                break;

            case ItemShopItem.ItemType.bigHealthPotion:
                inventory.addItem(new Item { itemType = Item.ItemType.bigPotion, amount = 1 });
                index = 1;
                break;

            case ItemShopItem.ItemType.shieldPotion:
                inventory.addItem(new Item { itemType = Item.ItemType.shieldPotion, amount = 1 });
                index = 2;
                break;

            case ItemShopItem.ItemType.goldPotion:
                inventory.addItem(new Item { itemType = Item.ItemType.goldPotion, amount = 1 });
                index = 3;
                break;

            case ItemShopItem.ItemType.teleportPotion:
                inventory.addItem(new Item { itemType = Item.ItemType.teleportPotion, amount = 1 });
                index = 4;
                break;

            case ItemShopItem.ItemType.timePotion:
                inventory.addItem(new Item { itemType = Item.ItemType.timePotion, amount = 1 });
                index = 5;
                break;

            case ItemShopItem.ItemType.doubleAxe:
                inventory.addItem(new Item { itemType = Item.ItemType.doubleAxe, amount = 1 });
                index = 6;
                break;

            case ItemShopItem.ItemType.bloodAxe:
                inventory.addItem(new Item { itemType = Item.ItemType.bloodAxe, amount = 1 });
                index = 7;
                break;

            case ItemShopItem.ItemType.seekAxe:
                inventory.addItem(new Item { itemType = Item.ItemType.seekAxe, amount = 1 });
                index = 8;
                break;

            case ItemShopItem.ItemType.battleAxe:
                inventory.addItem(new Item { itemType = Item.ItemType.battleAxe, amount = 1 });
                index = 9;
                break;

            case ItemShopItem.ItemType.nysthelAxe:
                inventory.addItem(new Item { itemType = Item.ItemType.nysthelAxe, amount = 1 });
                index = 10;
                break;

            case ItemShopItem.ItemType.trueAxe:
                inventory.addItem(new Item { itemType = Item.ItemType.trueAxe, amount = 1 });
                index = 11;
                break;

            case ItemShopItem.ItemType.shield:
                defense++;
                SaveVariables.PLAYER_DEFENSE++;
                index = 12;
                break;
        }
        return index;
    }
}