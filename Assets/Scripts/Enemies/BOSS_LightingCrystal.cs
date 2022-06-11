using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_LightingCrystal : Enemy
{
    public GameObject[] LaserEyes;
    public GameObject[] LaserAdvaices;
    public GameObject bat;
    public GameObject miner;

    public float pickAxeSpeed = 10f;
    public int pickAxeDamage = 25;
    public float timeToChangeAttack = 5f;
    public float laserAdviceTime = 0.3f;
    public float pickaxeDispersion = 0.2f;
    public GameObject villagePortal;
    public GameObject levelCompletedUi;

    private Rotating rotation;
    private bool rotate = false;
    private int attackType = 0;
    private bool changingAttack = false;
    private float time = 0;
    private float attackRateFase = 0;
    private List<GameObject> batsSpawned = new List<GameObject>();

    void Start()
    {
        startHealth = health;
        attackRateFase = attackRate / 3.5f;
        target = FindObjectOfType<Player>().transform;
        attackType = Random.Range(1, 4);
        foreach(GameObject g in LaserEyes)
        {
            g.SetActive(false);
        }
        foreach(GameObject g in LaserAdvaices)
        {
            g.SetActive(false);
        }
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            if (batsSpawned.Count > 0)
            {
                for (int i = 0; i < batsSpawned.Count; i++)
                {
                    if (batsSpawned[i] == null)
                    {
                        batsSpawned.RemoveAt(i);
                    }
                }
            }
            if (health <= 0)
            {
                Instantiate(villagePortal, transform.position, Quaternion.identity);
                Instantiate(levelCompletedUi, target);
                die();
            }

            switch (attackType)
            {
                case 1: //Lasers + PickAxes
                    time += Time.deltaTime;
                    if (health <= startHealth / 2)
                    {
                        attackRate = attackRateFase;
                    }

                    if (time >= attackRate)
                    {
                        time = 0;
                        GameObject bul = BulletPool.Instance.GetBullet();
                        bul.GetComponent<BulletHellBullet>().damage = pickAxeDamage;
                        bul.GetComponent<BulletHellBullet>().speed = pickAxeSpeed;
                        bul.gameObject.SetActive(true);
                        bul.transform.position = firePoint.position;
                        bul.transform.rotation = firePoint.rotation;
                        bul.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        bul.GetComponent<BulletHellBullet>().SetMoveDirection(((target.position - transform.position).normalized + new Vector3(Random.Range(-pickaxeDispersion, pickaxeDispersion), Random.Range(-pickaxeDispersion, pickaxeDispersion), 0)).normalized);
                    }
                    if (!changingAttack)
                    {
                        if (health >= startHealth / 2)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                LaserAdvaices[i].SetActive(true);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < LaserAdvaices.Length; i++)
                            {
                                LaserAdvaices[i].SetActive(true);
                            }
                        }
                        Invoke("lasersActivated", laserAdviceTime);
                        changingAttack = true;
                        Invoke("changeAttack", timeToChangeAttack);
                    }
                    break;

                case 2: //PickAxes throw
                    time += Time.deltaTime;
                    if (health <= startHealth / 2)
                    {
                        attackRate = attackRateFase;
                    }

                    if (time >= attackRate / 2)
                    {
                        time = 0;
                        GameObject bul = BulletPool.Instance.GetBullet();
                        bul.GetComponent<BulletHellBullet>().damage = pickAxeDamage;
                        bul.GetComponent<BulletHellBullet>().speed = pickAxeSpeed;
                        bul.gameObject.SetActive(true);
                        bul.transform.position = firePoint.position;
                        bul.transform.rotation = firePoint.rotation;
                        bul.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        bul.GetComponent<BulletHellBullet>().SetMoveDirection(((target.position - transform.position).normalized + new Vector3(Random.Range(-pickaxeDispersion, pickaxeDispersion), Random.Range(-pickaxeDispersion, pickaxeDispersion), 0).normalized).normalized);
                    }
                    if (!changingAttack)
                    {
                        changingAttack = true;
                        Invoke("changeAttack", timeToChangeAttack);
                    }
                    break;

                case 3: //Bat Spawn && Miner
                    time += Time.deltaTime;
                    if(time >= attackRate * 2)
                    {
                        time = 0;
                        if (health >= startHealth / 2)
                        {
                            if (batsSpawned.Count < 2)
                            {
                                GameObject go = Instantiate(bat, transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0), Quaternion.identity);
                                go.GetComponent<Enemy>().activated = true;
                                go.GetComponent<Enemy>().maxGoldToGive = 0;
                                go.GetComponent<Enemy>().minGoldToGive = 0;
                                go.GetComponent<Bat>().copyMaxGold = 0;
                                go.GetComponent<Bat>().copyMinGold = 0;
                                batsSpawned.Add(go);
                            }
                        }
                        else
                        {
                            if (batsSpawned.Count < 2)
                            {
                                if (Random.Range(0f, 1f) <= 0.3f)
                                {
                                    GameObject go = Instantiate(miner, transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0), Quaternion.identity);
                                    go.GetComponent<Enemy>().activated = true;
                                    go.GetComponent<Enemy>().maxGoldToGive = 0;
                                    go.GetComponent<Enemy>().minGoldToGive = 0;
                                    go.GetComponent<Enemy>().damage = 10;
                                    batsSpawned.Add(go);
                                }
                                else
                                {
                                    GameObject go = Instantiate(bat, transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0), Quaternion.identity);
                                    go.GetComponent<Enemy>().activated = true;
                                    go.GetComponent<Enemy>().maxGoldToGive = 0;
                                    go.GetComponent<Enemy>().minGoldToGive = 0;
                                    go.GetComponent<Bat>().copyMaxGold = 0;
                                    go.GetComponent<Bat>().copyMinGold = 0;
                                    go.GetComponent<Bat>().damage = 20;
                                    batsSpawned.Add(go);
                                }
                            }
                        }
                    }
                    if (!changingAttack)
                    {
                        changingAttack = true;
                        Invoke("changeAttack", timeToChangeAttack/3);
                    }
                    break;
            }
        }
    }

    private void lasersActivated()
    {
        if (health >= startHealth / 2)
        {
            for (int i = 0; i < 4; i++)
            {
                LaserEyes[i].SetActive(true);
            }
        }
        else
        {
            foreach (GameObject g in LaserEyes)
            {
                g.SetActive(true);
            }
        }

        foreach (GameObject g in LaserAdvaices)
        {
            g.SetActive(false);
        }
    }

    private void changeAttack()
    {
        CancelInvoke();
        changingAttack = false;
        int oldAttack = attackType;
        attackType = Random.Range(1, 4);
        if(oldAttack == attackType)
        {
            attackType = Random.Range(1, 4);
        }
        foreach (GameObject g in LaserEyes)
        {
            g.SetActive(false);
        }
    }
}
