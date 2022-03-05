using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingManager : MonoBehaviour
{
    public GameObject[] resourcesToSpawn;
    public Transform[] points;
    public int respawnTime = 4;
    public GameObject[] Enemies;
    public float spawnRadius;
    public float minEnemyRate = 10f;
    public float maxEnemyRate = 30f;

    private float enemyRate = 1f;
    private bool spawned = true;
    private GameObject[] instantiated;

    void Start()
    {
        instantiated = new GameObject[points.Length];
        reload();
        enemyRate = Random.Range(minEnemyRate, maxEnemyRate);
        Invoke("reSpawn", enemyRate);
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
            Vector2 randPos = Random.insideUnitCircle * spawnRadius;
            Instantiate(Enemies[Random.Range(0, Enemies.Length)], randPos, Quaternion.identity);
        }
    }

    IEnumerator respawn(int pos)
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
