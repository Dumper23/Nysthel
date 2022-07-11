using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bat : Enemy
{
    public GameObject batCopy;
    public int copyMaxGold = 5;
    public int copyMinGold = 2;
    public GameObject deathSound;
    public bool isSecondChance = false;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.7f, 1f);
        target = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay && (target.position - transform.position).magnitude <= range)
        {
            transform.Translate((target.position - transform.position).normalized * moveSpeed * Time.deltaTime);
            if (health <= 0)
            {
                Instantiate(deathSound, transform.position, Quaternion.identity);
                die();
            }
        }
    }
    
    override
    public void takeDamage(int value)
    {
        GameObject go = Instantiate(damageNumbers, transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0), Quaternion.identity) as GameObject;

        if (activated && !immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- " + value);
            }
            if(health - value > 0)
            {
                audioSource.Play();
                Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
                GameObject copy = Instantiate(batCopy, transform.position + spawnPos, Quaternion.identity);
                transform.position = transform.position + spawnPos;
                copy.GetComponent<Bat>().startHealth = 10;
                copy.GetComponent<Bat>().health = 10;
                copy.GetComponent<Bat>().activated = true;
                copy.GetComponent<Bat>().immune = false;
                if (isSecondChance)
                {
                    copy.GetComponent<Bat>().maxGoldToGive = 0;
                    copy.GetComponent<Bat>().minGoldToGive = 0;
                }
                else
                {
                    copy.GetComponent<Bat>().maxGoldToGive = copyMaxGold;
                    copy.GetComponent<Bat>().minGoldToGive = copyMinGold;
                }
                copy.GetComponent<Bat>().moveSpeed = moveSpeed * 2f;
                copy.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0, 0.4f);

            }
            health -= value;
        }
        else if (immune)
        {
            if (go != null)
            {
                go.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- 0");
            }
        }
    }
}
