using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public  Rigidbody2D rb;
    public Animator anim;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float animationDelay = 0.4f;
    public float attackRate = 1f;
    public Camera cam;
    public int gold;
    public float coinMagnetRange = 2f;
    public float coinMagnetSpeed = 1f;
    public int health = 50;
    public float immunityTime = 1f;
    public float dashRestoreTime = 3f;
    public float dashForce = 10f;
    public float dashTime = 1f;
    public float smoothFactor = 5f;

    private Vector2 movement;
    private string currentState;
    private bool attacking = false;
    private float nextFire = 0f;
    private bool immune = false;
    private bool dashing = false;
    private float nextDash = 0f;
    private Vector2 dashDirection;
    private Vector2 dashSpeed;

    private Vector2 lookDir;
    private Vector3 directionToShoot;
    private Vector3 positionToShoot;
    private float angle;


    private void Update()
    {
        //cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(transform.position.x, transform.position.y, cam.transform.position.z), smoothFactor/100);

        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);

        Collider2D[] coins = Physics2D.OverlapCircleAll(transform.position, coinMagnetRange);

        if (coins.Length > 0)
        {
            coinMagnet(coins);
        }

        die();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Time.time > nextDash && !dashing)
            {
                nextDash = Time.time + dashRestoreTime;
                dashing = true;
                dashDirection = movement;
                dashSpeed = dashDirection.normalized * dashForce;
            }
        }

        if (!dashing)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            playerMovement();

            if (!(movement.x == 0 && movement.y == 0))
            {
                firePoint.localPosition = new Vector3(Mathf.Clamp(movement.x, -0.6f, 0.6f), Mathf.Clamp(movement.y, -0.6f, 0.6f), 0);
            }

            if (Input.GetButtonDown("Jump"))
            {
                Shoot();
            }
        }
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
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Coin")
        {
            gold++;
            Destroy(collision.gameObject);
        }
    }

    void stopDash()
    {
        dashing = false;
    }

    void playerMovement()
    {
        if (movement.y > 0)
        {
            if (!attacking)
            {
                changeAnimationState("Nysthel_walkUp");
            }
        }
        else if (movement.y < 0)
        {
            if (!attacking)
            {
                changeAnimationState("Nysthel_walk");
            }
        }
        else
        {
            if (movement.x > 0)
            {
                //transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                GetComponent<SpriteRenderer>().flipX = false;
                if (!attacking)
                {
                    changeAnimationState("Nysthel_walk");
                }
            }
            else if (movement.x < 0)
            {
                //transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                GetComponent<SpriteRenderer>().flipX = true;
                if (!attacking)
                {
                    changeAnimationState("Nysthel_walk");
                }
            }
            else if (movement.y == 0)
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
        if (Time.time > nextFire)
        {
            nextFire = Time.time + attackRate;

            if (movement.y <= 0)
            {
                changeAnimationState("Nysthel_Attack");
            }
            else
            {
                changeAnimationState("Nysthel_AttackUp");
            }

            lookDir = firePoint.position - transform.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            directionToShoot = (firePoint.position - transform.position);
            attacking = true;
            Invoke("stopAttacking", animationDelay);

            positionToShoot = firePoint.position;
            GameObject bullet = Instantiate(bulletPrefab, positionToShoot, Quaternion.Euler(0, 0, angle));
            Rigidbody2D rBullet = bullet.GetComponent<Rigidbody2D>();
            rBullet.rotation = angle;
            rBullet.AddForce(directionToShoot * bulletForce, ForceMode2D.Impulse);
        }
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
            health -= value;
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
        if(health <= 0)
        {
            //Die, for now restart
            SceneManager.LoadScene(0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, coinMagnetRange);
    }
}
