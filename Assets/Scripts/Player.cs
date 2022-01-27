using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public  Rigidbody2D rb;
    public Animator anim;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float animationDelay = 0.4f;
    public float attackDelay = 0.2f;
    public float attackRate = 1f;
    public Camera cam;

    private Vector2 movement;
    private string currentState;
    private bool attacking = false;


    private void Update()
    {
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
        //input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!(movement.x == 0 && movement.y == 0))
        {
            firePoint.localPosition = Vector3.Normalize(new Vector3(Mathf.Clamp(movement.x, -0.1f, 0.1f), Mathf.Clamp(movement.y, -0.1f, 0.1f), 0))/5;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (movement.y <= 0)
            {
                changeAnimationState("Nysthel_Attack");
            }
            else
            {
                changeAnimationState("Nysthel_AttackUp");
            }

            attacking = true;
            Invoke("stopAttacking", animationDelay);
            Invoke("Shoot", attackDelay);
        }

        if (movement.y > 0)
        {
            if (!attacking)
            {
                changeAnimationState("Nysthel_walkUp");
            }
        }else if (movement.y < 0)
        {
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

    private void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + (movement * moveSpeed * Time.fixedDeltaTime));

    }

    void Shoot()
    {
        
        Vector2 lookDir = firePoint.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        


        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rBullet = bullet.GetComponent<Rigidbody2D>();
        rBullet.rotation = angle;
        rBullet.AddForce((firePoint.position - transform.position) * bulletForce, ForceMode2D.Impulse);
    
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
}
