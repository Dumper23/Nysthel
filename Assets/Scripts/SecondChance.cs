using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SecondChance : MonoBehaviour
{
    public GameObject[] EnemiesW1;
    public GameObject[] EnemiesW2;

    public float spawnRadius;
    public TextMeshProUGUI counterText;
    public float timer = 60f;
    public float enemyRate = 1f;
    
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
                Instantiate(EnemiesW1[Random.Range(0, EnemiesW1.Length)], randPos, Quaternion.identity);
            }else if (SaveVariables.CURRENT_WORLD == 1)
            {
                Instantiate(EnemiesW2[Random.Range(0, EnemiesW2.Length)], randPos, Quaternion.identity);
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
