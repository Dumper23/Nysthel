using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingManager : MonoBehaviour
{
    public GameObject[] resourcesToSpawn;
    public GameObject bulletW3;
    public Transform[] points;
    public int respawnTime = 4;
    public GameObject[] EnemiesW1;
    public GameObject[] EnemiesW2;
    public GameObject[] EnemiesW3;
    public GameObject[] EnemiesW4;
    public float spawnRadius;
    public float minEnemyRate = 10f;
    public float maxEnemyRate = 30f;

    private float enemyRate = 1f;
    private bool spawned = true;
    private GameObject[] instantiated;

    private bool reduced500 = false, reduced1000 = false, reduced2000 = false;

    private void Start()
    {
        instantiated = new GameObject[points.Length];
        reload();
        enemyRate = Random.Range(minEnemyRate, maxEnemyRate);
        Invoke("reSpawn", enemyRate);

        if (SaveVariables.MAX_WORLD == 3)
        {
            BulletPool.Instance.poolBullet = bulletW3;
        }
    }

    public void reload()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (instantiated[i] != null)
            {
                Destroy(instantiated[i]);
            }

            instantiated[i] = Instantiate(
            resourcesToSpawn[Random.Range(0, resourcesToSpawn.Length)],
            points[i].transform.position,
            Quaternion.identity) as GameObject;
        }
    }

    private void Update()
    {
        if (FindObjectOfType<Player>().wood > 500 & !reduced500)
        {
            enemyRate -= 2;
            reduced500 = true;
        }

        if (FindObjectOfType<Player>().wood > 1000 & !reduced1000)
        {
            enemyRate -= 2;
            reduced1000 = true;
        }

        if (FindObjectOfType<Player>().wood > 2000 & !reduced2000)
        {
            enemyRate -= 2;
            reduced2000 = true;
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (instantiated[i] == null)
            {
                StartCoroutine("respawn", i);
            }
        }

        if (!spawned)
        {
            Invoke("reSpawn", enemyRate);
            spawned = true;
            Player aux = FindObjectOfType<Player>();
            Vector3 randPos = Random.insideUnitCircle * spawnRadius;
            while ((randPos - aux.transform.position).magnitude < 3)
            {
                randPos = Random.insideUnitCircle * spawnRadius;
            }

            if (SaveVariables.MAX_WORLD == 0 || SaveVariables.MAX_WORLD == 1)
            {
                Instantiate(EnemiesW1[Random.Range(0, EnemiesW1.Length)], randPos, Quaternion.identity);
            }
            else if (SaveVariables.MAX_WORLD == 2)
            {
                Instantiate(EnemiesW2[Random.Range(0, EnemiesW2.Length)], randPos, Quaternion.identity);
            }
            else if (SaveVariables.MAX_WORLD == 3)
            {
                Instantiate(EnemiesW3[Random.Range(0, EnemiesW3.Length)], randPos, Quaternion.identity);
            }
            else if (SaveVariables.MAX_WORLD > 3)
            {
                Instantiate(EnemiesW4[Random.Range(0, EnemiesW4.Length)], randPos, Quaternion.identity);
            }
        }
    }

    private IEnumerator respawn(int pos)
    {
        yield return new WaitForSeconds(respawnTime);

        instantiated[pos] = Instantiate(
            resourcesToSpawn[Random.Range(0, resourcesToSpawn.Length)],
            points[pos].transform.position,
            Quaternion.identity) as GameObject;
        StopAllCoroutines();
    }

    private void reSpawn()
    {
        spawned = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, spawnRadius);
    }
}