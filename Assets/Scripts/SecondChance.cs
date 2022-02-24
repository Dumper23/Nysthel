using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SecondChance : MonoBehaviour
{
    public GameObject[] Enemies;
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
            Instantiate(Enemies[Random.Range(0, Enemies.Length)], randPos, Quaternion.identity);
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
