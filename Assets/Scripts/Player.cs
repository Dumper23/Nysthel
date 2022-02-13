using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
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

    private Vector2 lookDir;
    private Vector3 directionToShoot;
    private Vector3 positionToShoot;
    private float angle;

    private Inventory inventory;
    [SerializeField]
    private UIInventory uiInventory;

    private void Awake()
    {
        gold = PlayerPrefs.GetInt("gold");
        goldText.text = gold.ToString();
        currentHealth = maxHealth;
        inventory = new Inventory(UseItem);
        uiInventory.setInventory(inventory);
        uiInventory.setPlayer(this);
        GameStateManager.Instance.OnGameStateChange += OnGameStateChanged;
        healthBar.setMaxHealth(maxHealth);
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

            aimPos.x = Input.GetAxisRaw("HorizontalAim");
            aimPos.y = Input.GetAxisRaw("VerticalAim");

            Vector2 tempAim = aimPos;

            aimPos = GetConstrainedPosition(Vector2.zero, tempAim); 
            

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

            if (Input.GetButtonDown("Attack"))
            {
                Shoot();
            }
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
        }
        else
        {
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
            goldText.text = gold.ToString();
            SaveVariables.PLAYER_GOLD = gold;
            Destroy(collision.gameObject);
        }
        ItemWorld iw = collision.transform.GetComponent<ItemWorld>();
        if (iw != null)
        {
            inventory.addItem(iw.getItem());
            iw.destroySelf();
        }
    }

    void playerMovement(Vector2 dir)
    {
        if (dir.y > 0)
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
            if (dir.x > 0)
            {
                //transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                GetComponent<SpriteRenderer>().flipX = false;
                if (!attacking)
                {
                    changeAnimationState("Nysthel_walk");
                }
            }
            else if (dir.x < 0)
            {
                //transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                GetComponent<SpriteRenderer>().flipX = true;
                if (!attacking)
                {
                    changeAnimationState("Nysthel_walk");
                }
            }
            else if (dir.y == 0)
            {
                if (!attacking)
                {
                    changeAnimationState("Nysthel_idle");
                }
            }
        }
    }

    

    void Shoot()
    {
        //Cambiar la forma de disparar i fer un script per a que la bala vagi a una velocitat constant (tipo la bala bullethellbullet)
        if (Time.time > nextFire)
        {
            nextFire = Time.time + attackRate;

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
        Invoke("notImmunity", immunityTime);
    }

    void notImmunity()
    {
        immune = false;
    }

    void die()
    {
        if(currentHealth <= 0)
        {
            //Die, for now just go to the village
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
}
