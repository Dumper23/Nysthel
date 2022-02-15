using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Turtoise : Enemy
{
    public float dashForce = 5f;
    public float shotAngleIncrement = 10f;
    public GameObject miniTurtle;

    public GameObject bullet;
    public float bulletForce = 10f;
    public GameObject villagePortal;

    private float angle = 0f;
    private string[] state;
    private bool inAction = false;
    private string cState;
    private Vector2 moveDir;
    private float nextTurtle = 0f;

    private bool immune = false;
    private bool phase2 = false;
    private bool shot = false;


    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Decoration"), LayerMask.NameToLayer("Enemy"));
        startHealth = health;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = new string[8];
        state[0] = "dash";
        state[1] = "shield";
        state[2] = "idle";
        state[4] = "dash";
        state[5] = "dash";
        state[6] = "idle";
        state[7] = "idle";

        cState = state[2];
        moveDir = (target.position - transform.position).normalized * dashForce;
    }

    private void Update()
    {
        if (activated)
        {
            if (health <= Mathf.RoundToInt(startHealth / 2))
            {
                phase2 = true;
            }

            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (health <= 0)
            {
                Instantiate(villagePortal, transform.position, Quaternion.identity);
                die();
            }

            if (!inAction) {
                Invoke("ChangeState", Random.Range(2, 5));
                inAction = true;
            }

            if (!shot)
            {
                Invoke("Shoot", Random.Range(1, 4));
                shot = true;
            }

            switch (cState)
            {
                case "dash":
                    changeAnimationState("dash");
                    dash();
                    immune = true;
                    break;
                case "shield":
                    changeAnimationState("shield");
                    immune = true;
                    break;
                case "idle":
                    changeAnimationState("idle");
                    immune = false;
                    break;
            }
        }
    }

    private void ChangeState()
    {
        cState = state[Random.Range(0, state.Length-1)];
        moveDir = (target.position - transform.position).normalized * dashForce;
        inAction = false;
    }

    private void Shoot()
    {
        shot = false;
        angle = 0;
        while (angle <= 350)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.Instance.GetBullet();
            //bul.GetComponent<SpriteRenderer>().sprite = bulletSprite;

            bul.transform.position = firePoint.position;
            bul.transform.rotation = firePoint.rotation;

            bul.SetActive(true);
            bul.GetComponent<BulletHellBullet>().SetMoveDirection(bulDir);

            angle += shotAngleIncrement;
        }
    }

    private void dash()
    {
        transform.Translate(moveDir * Time.deltaTime);

        if (phase2)
        {
            if (Time.time > nextTurtle)
            {
                nextTurtle = Time.time + attackRate;
                Instantiate(miniTurtle, transform.position, Quaternion.identity);
            }
        }
    }
    
    public override void takeDamage(int value)
    {
        if (activated && !immune)
        {
            health -= value;
            cState = "shield";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("EnemyZone"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
        }
        else
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<Player>().takeDamage(damage);
            }
            else
            {
                moveDir = (target.position - transform.position).normalized * dashForce;
            }
        }
    }

}
