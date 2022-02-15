using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour, IShopCustomer
{
    public float moveSpeed = 5f;
    public  Rigidbody2D rb;
    public Animator anim;
    public int damage = 10;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float animationDelay = 0.4f;
    public float attackRate = 1f;
    public Camera cam;
    public int gold;
    public float coinMagnetRange = 2f;
    public float coinMagnetSpeed = 1f;
    public int maxHealth = 50;
    public float immunityTime = 1f;
    public float dashRestoreTime = 3f;
    public float dashForce = 10f;
    public float dashTime = 1f;
    public float smoothFactor = 5f;
    public HealthBar healthBar;
    public TextMeshProUGUI goldText;
    public GameObject shield;
    public GameObject crossHair;
    public ParticleSystem walkParticles;
    public ParticleSystem dashParticles;
    public List<AudioClip> audios;
    public List<AudioSource> audioSource;
    public bool usingController = true;

    private const int ATTACK_AUDIO = 0;
    private const int DASH_AUDIO = 1;
    private const int FOOTSTEP_AUDIO = 2;
    private const int PICKUP_AUDIO = 3;

    private int currentHealth;
    private Vector2 movement;
    private Vector2 aimPos;
    private string currentState;
    private bool attacking = false;
    private float nextFire = 0f;
    private bool immune = false;
    private bool dashing = false;
    private float nextDash = 0f;
    private Vector2 dashDirection;
    private Vector2 dashSpeed;
    private bool shielded = false;
    private bool basicaxe = true;
    private bool multiaxe = false;
    private bool doubleaxe = false;

    public bool pathRenderer = false;
    public LineRenderer ruteFollowed;
    private int posIndex = 0;
    private float nextPoint = 0;
    public float timeBetweenPointsInRute = 2f;

    private Vector2 lookDir;
    private Vector3 directionToShoot;
    private Vector3 positionToShoot;
    private float angle;

    private Vector3 lastUpdatePos = Vector3.zero;
    private Vector3 dist;
    private float curentSpeed;

    private Inventory inventory;
    [SerializeField]
    private UIInventory uiInventory;

    private void Awake()
    {
        //Unic LoadGame Que hi ha d'haver ja que carrega totes les variables no només les del jugador
        SaveManager.Instance.loadGame();

        inventory = new Inventory(UseItem);
        uiInventory.setInventory(inventory);
        uiInventory.setPlayer(this);
        GameStateManager.Instance.OnGameStateChange += OnGameStateChanged;
    }

    private void Start()
    {

        gold = SaveVariables.PLAYER_GOLD;

        if (SaveVariables.PLAYER_ATTACK > 0) damage = SaveVariables.PLAYER_ATTACK;

        if (SaveVariables.PLAYER_LIFE > 0) maxHealth = SaveVariables.PLAYER_LIFE;

        if (SaveVariables.PLAYER_SPEED > 0) moveSpeed = SaveVariables.PLAYER_SPEED;

        if(SaveVariables.PLAYER_ATTACK_SPEED > 0) attackRate = SaveVariables.PLAYER_ATTACK_SPEED;
       
        if(SaveVariables.PLAYER_RANGE > 0) coinMagnetRange = SaveVariables.PLAYER_RANGE;

        if(SaveVariables.PLAYER_DASH_RECOVERY > 0) dashRestoreTime = SaveVariables.PLAYER_DASH_RECOVERY;

        if(SaveVariables.PLAYER_DASH_RANGE > 0) dashForce = SaveVariables.PLAYER_DASH_RANGE;
        


        healthBar.setMaxHealth(maxHealth);
        currentHealth = maxHealth;
        goldText.text = gold.ToString();
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

        if (immune)
        {
            if (!shielded)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
            }
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
        }

        healthBar.setHealth(currentHealth);

        if (aimPos.magnitude != 0)
        {
            crossHair.transform.position = transform.position + new Vector3(aimPos.x, aimPos.y);
        }

        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);

        Collider2D[] coins = Physics2D.OverlapCircleAll(transform.position, coinMagnetRange);

        if (coins.Length > 0)
        {
            coinMagnet(coins);
        }

        die();

        
        if ((Input.GetAxisRaw("Dash") != 0 || Input.GetKey(KeyCode.LeftShift)) && !shielded)
        {
            if (Time.time > nextDash && !dashing)
            {
                audioSource[1].clip = audios[1];
                audioSource[1].Play();
                nextDash = Time.time + dashRestoreTime;
                dashing = true;
                immune = true;
                dashDirection = movement;
                dashSpeed = dashDirection.normalized * dashForce;
            }
        }

        if (!dashing)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (usingController)
            {
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

            if(movement.magnitude == 0)
            {
                walkParticles.Play();
                audioSource[2].clip = audios[2];
                audioSource[2].Play();
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
                Shoot();
            }
        }
    }

    private bool checkMovement()
    {
        dist = transform.position - lastUpdatePos;
        curentSpeed = dist.magnitude / Time.deltaTime;
        lastUpdatePos = transform.position;

        if(curentSpeed > 0.1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    Vector2 GetConstrainedPosition(Vector2 midPoint, Vector2 endPoint)
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
        //Movement
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
            gold++;
            updateGold();
            SaveVariables.PLAYER_GOLD = gold;
            Destroy(collision.gameObject);
        }
        ItemWorld iw = collision.transform.GetComponent<ItemWorld>();
        if (iw != null)
        {
            audioSource[0].clip = audios[PICKUP_AUDIO];
            audioSource[0].Play();
            inventory.addItem(iw.getItem());
            iw.destroySelf();
        }
    }

    public bool isImmune()
    {
        return immune;
    }

    void playerMovement(Vector2 dir)
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
        }else if (!attacking)
        {
            changeAnimationState("Nysthel_idle");
        }
    }

    

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + attackRate;
            //Ficar un so per a cada arma de moment un per totes
            audioSource[0].clip = audios[0];
            audioSource[0].Play();

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
                lookDir =  firePoint.position - transform.position;
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
                    bullet.setDamage(damage);
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
                bullet.setDamage(damage);
            }
            else if (basicaxe)
            {
                directionToShoot = (firePoint.position - transform.position).normalized;
                attacking = true;
                Invoke("stopAttacking", animationDelay);
                

                Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.setDirection(directionToShoot);
                bullet.setDamage(damage);
            }
        }
    }

    private void generateSecondBullet()
    {
        audioSource[0].Play();
        directionToShoot = (firePoint.position - transform.position).normalized;
        attacking = true;
        Invoke("stopAttacking", animationDelay);

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.setDirection(directionToShoot);
    }

    void coinMagnet(Collider2D[] coins)
    {
        foreach(Collider2D coin in coins)
        {
            if (coin.tag == "Coin")
            {
                coin.transform.Translate((transform.position - coin.transform.position).normalized * coinMagnetSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void stopAttacking()
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
            currentHealth -= value;
            immunity();
        }
    }

    void immunity()
    {
        immune = true;
        shield.SetActive(true);
        Invoke("notImmunity", immunityTime);
    }

    void notImmunity()
    {
        shield.SetActive(false);
        immune = false;
    }

    void die()
    {
        if(currentHealth <= 0)
        {
            gold -= Mathf.RoundToInt(0.6f * gold);
            SaveVariables.PLAYER_GOLD = gold;
            SaveManager.Instance.SaveGame();
            SceneManager.LoadScene("Village");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, coinMagnetRange);
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
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.smallPotion, amount = 1 });
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
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.bigPotion, amount = 1 });
                    }
                    break;
                case Item.ItemType.shieldPotion:
                    shield.SetActive(true);
                    immune = true;
                    Invoke("endShield", 5f);
                    Invoke("notImmunity", 5f);
                    shielded = true;
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.shieldPotion, amount = 1 });
                    break;
            }
            healthBar.setHealth(currentHealth);
        }
        else
        {
            switch (item.itemType)
            {
                case Item.ItemType.multiAxe:
                    if (doubleaxe)doubleaxe = false;
                    if (basicaxe) basicaxe = false;
                    multiaxe = true;
                    break;
                case Item.ItemType.doubleAxe:
                    if (multiaxe) multiaxe = false;
                    if (basicaxe) basicaxe = false;
                    doubleaxe = true;
                    break;
                case Item.ItemType.basicAxe:
                    if (multiaxe) multiaxe = false;
                    if (doubleaxe) doubleaxe = false;
                    basicaxe = true;
                    break;
            }
        }
    }

    #endregion

    private void endShield()
    {
        shield.SetActive(false);
        shielded = false;
    }

    public void BoughtItem(ShopItem.ItemType itemType)
    {
        ShopItem.AddLevel(itemType);

        switch (itemType)
        {
            case ShopItem.ItemType.LifeUpgrade:
                maxHealth += 10;
                SaveVariables.PLAYER_LIFE = maxHealth;
                SaveVariables.LIFE_LEVEL = ShopItem.GetCurrentLevel(itemType);
                healthBar.setMaxHealth(maxHealth);
                currentHealth = maxHealth;
                break;

            case ShopItem.ItemType.AttackUpgrade:
                damage += 5;
                SaveVariables.PLAYER_ATTACK = damage;
                SaveVariables.ATTACK_LEVEL = ShopItem.GetCurrentLevel(itemType);
                break;

            case ShopItem.ItemType.SpeedUpgrade:
                moveSpeed += 1;
                SaveVariables.PLAYER_SPEED = moveSpeed;
                SaveVariables.SPEED_LEVEL = ShopItem.GetCurrentLevel(itemType);
                break;

            case ShopItem.ItemType.AttackSpeedUpgrade:
                attackRate -= 0.05f;
                SaveVariables.PLAYER_ATTACK_SPEED = attackRate;
                SaveVariables.ATTACK_SPEED_LEVEL = ShopItem.GetCurrentLevel(itemType);
                break;

            case ShopItem.ItemType.RangeUpgrade:
                coinMagnetRange += 0.55f;
                SaveVariables.PLAYER_RANGE = coinMagnetRange;
                SaveVariables.RANGE_LEVEL = ShopItem.GetCurrentLevel(itemType);
                break;

            case ShopItem.ItemType.DashRecoveryUpgrade:
                dashRestoreTime -= 0.5f;
                SaveVariables.PLAYER_DASH_RECOVERY = dashRestoreTime;
                SaveVariables.DASH_RECOVERY_LEVEL = ShopItem.GetCurrentLevel(itemType);
                break;

            case ShopItem.ItemType.DashRangeUpgrade:
                dashForce += 2f;
                SaveVariables.PLAYER_DASH_RANGE = dashForce;
                SaveVariables.DASH_RANGE_LEVEL = ShopItem.GetCurrentLevel(itemType);
                break;
        }
    }

    private void updateGold()
    {
        goldText.text = gold.ToString();
    }

    public bool TrySpendGoldAmount(int goldAmount)
    {
        if(gold >= goldAmount)
        {
            gold -= goldAmount;
            updateGold();
            SaveVariables.PLAYER_GOLD = gold;
            return true;
        }
        else
        {
            //Error, no enough gold
            return false;
        }
    }

    public float[] GetStatistics()
    {
        float[] s = new float[7];
        s[0] = damage;
        s[1] = maxHealth;
        s[2] = moveSpeed;
        s[3] = (1/attackRate);
        s[4] = dashRestoreTime;
        s[5] = dashForce;
        s[6] = coinMagnetRange;
        return s;    
    }
}
