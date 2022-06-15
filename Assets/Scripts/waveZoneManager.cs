using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class waveZoneManager : MonoBehaviour
{

    public List<Transform> spawnPoints = new List<Transform>();
    public float timeToSpawn = 0.5f;
    public int enemyLimit = 20;
    public TextMeshProUGUI timer;
    public Transform prizeSpawnPoint;
    public List<GameObject> prizes = new List<GameObject>();
   

    private AudioSource audioSource;
    private int currentEnemies = 0;
    private float timeSpawning = 0;
    private float tempTime = 0;
    private bool prizeSpawned = false;
    private float timeToPrize = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!spawnPoints.Contains(transform.GetChild(i).transform))
            {
                spawnPoints.Add(transform.GetChild(i).transform);
            }
        }
    }

    void prizeSpawnedToggle()
    {
        prizeSpawned = false;
    }

    void Update()
    {
        tempTime += Time.deltaTime;
        timeToPrize += Time.deltaTime;
        if (timeToPrize >= 60 && !prizeSpawned)
        {
            timeToPrize = 0;
            audioSource.Play();
            prizeSpawned = true;
            Invoke("prizeSpawnedToggle", 0.2f);
            Instantiate(prizes[Random.Range(0, prizes.Count)], prizeSpawnPoint.position, Quaternion.identity); ;
        }
        timer.SetText(Mathf.FloorToInt(tempTime / 60) + "m " + Mathf.RoundToInt(tempTime - (Mathf.FloorToInt(tempTime / 60) * 60)) + "s ");
        currentEnemies = BulletPool.Instance.getActiveBulletCount();
        timeSpawning += Time.deltaTime;
        if(timeSpawning >= timeToSpawn && currentEnemies < enemyLimit)
        {
            timeSpawning = 0;
            GameObject enemy = BulletPool.Instance.GetBullet();
            enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            enemy.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!spawnPoints.Contains(transform.GetChild(i).transform))
            {
                spawnPoints.Add(transform.GetChild(i).transform);
            }
        }
        
        foreach(Transform point in spawnPoints)
        {
            Gizmos.DrawSphere(point.position, 0.5f);
        }
    }
}
