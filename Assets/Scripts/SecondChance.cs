using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SecondChance : MonoBehaviour
{
    public GameObject[] EnemiesW1;
    public GameObject[] EnemiesW2;
    public GameObject[] EnemiesW3;
    public GameObject bulletW1;
    public GameObject bulletW2;
    public GameObject bulletW3;
    public BulletPool bp;

    public float spawnRadius;
    public TextMeshProUGUI counterText;
    public float timer = 60f;
    public float enemyRate = 1f;
    public GameObject loadingScreen;
    
    private int seconds;
    private bool spawned = false;

    private void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        seconds = (int)timer % 60;
        counterText.SetText("Survive for: " + seconds + "s");

        if(seconds <= 0)
        {
            loadingScreen.SetActive(true);
            SaveManager.Instance.SaveGame();
            SceneManager.LoadScene(SaveVariables.getCurrentWorld());
        }

        if (!spawned && seconds < 55)
        {
            Invoke("reSpawn", enemyRate);
            spawned = true;
            Vector2 randPos = Random.insideUnitCircle * spawnRadius;
            if (SaveVariables.CURRENT_WORLD == 0)
            {
                bp.poolBullet = bulletW1;
                Instantiate(EnemiesW1[Random.Range(0, EnemiesW1.Length)], randPos, Quaternion.identity);
            }else if (SaveVariables.CURRENT_WORLD == 1)
            {
                bp.poolBullet = bulletW2;
                Instantiate(EnemiesW2[Random.Range(0, EnemiesW2.Length)], randPos, Quaternion.identity);
            }
            else if (SaveVariables.CURRENT_WORLD == 2)
            {
                bp.poolBullet = bulletW3;
                Instantiate(EnemiesW3[Random.Range(0, EnemiesW3.Length)], randPos, Quaternion.identity);
            }
        }
        
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
