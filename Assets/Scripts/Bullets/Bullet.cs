using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0, 1)]
    public float range = 1f;

    public float speed = 1f;
    public GameObject afterDestroySound;
    public GameObject destroyGameObject;
    public GameObject holyHit;
    public Transform centerOfSeeking;
    public bool isSeeker = false;
    public float seekRange = 2f;
    public LayerMask enemyLayer;
    public bool isTrueBullet = false;

    [Header("Electric Setings")]
    public float electricRange = 2f;

    public GameObject ADSElectric;
    public int electricDamage = 10;
    public GameObject electricHit;
    public GameObject electricRay;

    [Header("Fire Setings")]
    public int fireDamage = 10;

    public GameObject ADSFire;
    public GameObject fireHit;

    [Header("Earth Setings")]
    public GameObject earthHit;

    public GameObject ADSEarth;

    [Header("Earth Setings")]
    public GameObject iceHit;

    public GameObject ADSIce;

    private Transform player;

    private int damage = 10;

    private Vector2 moveDir;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlayerBullet"));
        Destroy(gameObject, range);
    }

    private void Update()
    {
        if (isSeeker)
        {
            GameObject closestEnemy = null;
            float closestDistance = 999f;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(centerOfSeeking.position, seekRange, enemyLayer);
            if (enemies.Length > 0)
            {
                foreach (Collider2D enemy in enemies)
                {
                    if (Mathf.Abs((enemy.transform.position - this.transform.position).magnitude) < closestDistance)
                    {
                        closestDistance = (enemy.transform.position - this.transform.position).magnitude;
                        closestEnemy = enemy.gameObject;
                    }
                }
                transform.Translate((closestEnemy.transform.position - transform.position) * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(moveDir * speed * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
    }

    public void setDirection(Vector2 dir)
    {
        moveDir = dir;
    }

    public void setRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void setDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "coinContainer")
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "skill" && collision.transform.name == "Acid(Clone)" && SaveVariables.FIRE_ORB == 2)
        {
            collision.GetComponent<AcidSkill>().incendiate();
        }

        if (collision.CompareTag("coinContainer"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
            GameObject p = Instantiate(destroyGameObject, transform.position, Quaternion.identity);
            if (player.GetComponent<Player>().holy)
            {
                Instantiate(holyHit, transform.position, Quaternion.identity);
            }

            float angle = Mathf.Atan2((player.position - transform.position).normalized.y, (player.position - transform.position).normalized.x) * Mathf.Rad2Deg;
            p.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f);
        }

        if (collision.transform.tag == "Enemy" || collision.transform.tag == "sparring")
        {
            Statistics.Instance.shake();
            if (SaveVariables.ELECTRIC_ORB == 2 && collision.transform.GetComponent<Enemy>() && !collision.transform.GetComponent<Enemy>().immuneToElementalEffects)
            {
                if (collision.transform.GetComponent<Enemy>())
                {
                    collision.transform.GetComponent<Enemy>().takeDamage(damage);
                }
                Collider2D[] hits;
                if (collision.transform.GetComponent<Enemy>().isInWater)
                {
                    hits = Physics2D.OverlapCircleAll(transform.position, electricRange * 3, enemyLayer);
                }
                else
                {
                    hits = Physics2D.OverlapCircleAll(transform.position, electricRange, enemyLayer);
                }
                int i = 0;
                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Enemy") || hit.CompareTag("sparring"))
                    {
                        Instantiate(electricHit, hit.transform.position, Quaternion.identity, hit.transform);
                        Instantiate(ADSElectric, transform.position, Quaternion.identity);
                        if (hits.Length > 1 && i > 0)
                        {
                            Vector3 midPoint = new Vector3(((hits[i - 1].transform.position.x + hits[i].transform.position.x) / 2), ((hits[i - 1].transform.position.y + hits[i].transform.position.y) / 2), 0);
                            GameObject r = Instantiate(electricRay, midPoint, Quaternion.identity);
                            float a = Mathf.Atan2((hits[i - 1].transform.position - hits[i].transform.position).normalized.y, (hits[i - 1].transform.position - hits[i].transform.position).normalized.x) * Mathf.Rad2Deg;
                            r.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(a, Vector3.forward), 10f);
                            if (hits[i].transform.GetComponent<Enemy>())
                            {
                                hits[i].transform.GetComponent<Enemy>().takeDamage(electricDamage);
                            }
                            if (hits[i - 1].transform.GetComponent<Enemy>())
                            {
                                hits[i - 1].transform.GetComponent<Enemy>().takeDamage(electricDamage);
                            }
                        }
                    }
                    i++;
                }
            }
            else if (SaveVariables.FIRE_ORB == 2)
            {
                if (collision.transform.GetComponent<Enemy>())
                {
                    if (!collision.transform.GetComponent<Enemy>().immuneToElementalEffects)
                    {
                        if (!collision.transform.GetComponent<Enemy>().isInFire)
                        {
                            Instantiate(ADSFire, transform.position, Quaternion.identity);
                            Instantiate(fireHit, collision.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, collision.transform);
                        }
                        collision.transform.GetComponent<Enemy>().fireEffect(fireDamage, 10);
                        collision.transform.GetComponent<Enemy>().takeDamage(damage);
                    }
                    else
                    {
                        collision.transform.GetComponent<Enemy>().takeDamage(damage);
                    }
                }
            }
            else if (SaveVariables.EARTH_ORB == 2)
            {
                if (collision.transform.GetComponent<Enemy>())
                {
                    if (!collision.transform.GetComponent<Enemy>().immuneToElementalEffects)
                    {
                        Instantiate(ADSEarth, transform.position, Quaternion.identity);
                        Instantiate(earthHit, collision.transform.position, Quaternion.identity);
                        collision.transform.GetComponent<Enemy>().takeDamage(damage / 2);
                        collision.transform.GetComponent<Enemy>().takeDamage(damage);
                    }
                    else
                    {
                        collision.transform.GetComponent<Enemy>().takeDamage(damage);
                    }
                }
            }
            else if (SaveVariables.ICE_ORB == 2)
            {
                if (collision.transform.GetComponent<Enemy>())
                {
                    if (!collision.transform.GetComponent<Enemy>().immuneToElementalEffects)
                    {
                        collision.transform.GetComponent<Enemy>().takeDamage(damage);
                        if (!collision.transform.GetComponent<Enemy>().isFrozen)
                        {
                            if (Random.Range(0, 1f) <= 0.35)
                            {
                                Instantiate(iceHit, collision.transform.position, Quaternion.identity, collision.transform);
                                collision.transform.GetComponent<Enemy>().freeze();
                                Instantiate(ADSIce, transform.position, Quaternion.identity);
                            }
                        }
                    }
                    else
                    {
                        collision.transform.GetComponent<Enemy>().takeDamage(damage);
                    }
                }
            }
            else
            {
                if (collision.transform.GetComponent<Enemy>())
                {
                    collision.transform.GetComponent<Enemy>().takeDamage(damage);
                }
            }
            GameObject p = Instantiate(destroyGameObject, transform.position, Quaternion.identity);
            if (player.GetComponent<Player>().holy)
            {
                Instantiate(holyHit, transform.position, Quaternion.identity);
            }

            float angle = Mathf.Atan2((player.position - transform.position).normalized.y, (player.position - transform.position).normalized.x) * Mathf.Rad2Deg;
            p.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f);
        }

        if ((collision.transform.tag == "Bullet" || collision.transform.tag == "BulletHellBullet"))
        {
            if (SaveVariables.HOLY_STATUE != 2)
            {
                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else
            {
                if (collision.transform.CompareTag("BulletHellBullet"))
                {
                    collision.gameObject.SetActive(false);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
                GameObject p = Instantiate(destroyGameObject, transform.position, Quaternion.identity);
                if (player.GetComponent<Player>().holy)
                {
                    Instantiate(holyHit, transform.position, Quaternion.identity);
                }

                float angle = Mathf.Atan2((player.position - transform.position).normalized.y, (player.position - transform.position).normalized.x) * Mathf.Rad2Deg;
                p.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f);
                Instantiate(afterDestroySound, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

        if (collision.transform.tag != "Bullet" && collision.transform.tag != "skill" && collision.transform.tag != "MineWall" && collision.transform.tag != "coinContainer" && collision.transform.tag != "BulletHellBullet" && collision.transform.tag != "Player" && collision.transform.tag != "EnemyZone" && collision.transform.tag != "Interactable" && collision.transform.tag != "SpawnPoint" && collision.transform.tag != "Shield" && collision.transform.tag != "PlayerBullet" && collision.transform.tag != "Collectable" && collision.transform.tag != "Coin" && collision.transform.tag != "Coin2" && collision.transform.tag != "Coin3" && collision.transform.tag != "Wood")
        {
            Instantiate(afterDestroySound, transform.position, Quaternion.identity);
            GameObject p = Instantiate(destroyGameObject, transform.position, Quaternion.identity);
            if (player.GetComponent<Player>().holy)
            {
                Instantiate(holyHit, transform.position, Quaternion.identity);
            }

            float angle = Mathf.Atan2((player.position - transform.position).normalized.y, (player.position - transform.position).normalized.x) * Mathf.Rad2Deg;
            p.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f);
            if (!isTrueBullet)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "MineWall")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnDrawGizmos()
    {
        if (centerOfSeeking != null)
        {
            Gizmos.DrawWireSphere(centerOfSeeking.position, seekRange);
        }
    }
}