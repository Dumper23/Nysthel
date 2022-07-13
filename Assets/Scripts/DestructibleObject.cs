using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int health = 10;
    public int maxGoldToGive = 1;
    [Range(0, 2)]
    public int coinType = 0;
    public float coinForce = 20f;
    public GameObject soundEffect;
    public ParticleSystem destroyParticles;

    private int goldToGive;

    void Start()
    {
        int g = Random.Range(0, maxGoldToGive);
        if (g < 0) g = 0;
        goldToGive = g;
    }

    void Update()
    {
        if(health <= 0)
        {
            Instantiate(destroyParticles, transform.position, Quaternion.identity);
            for (int i = 0; i <= goldToGive; i++) {
                GameObject g = CoinManager.Instance.GetCoin(coinType);
                g.SetActive(true);
                g.transform.position = transform.position;
                Coin c = g.GetComponent<Coin>();
                c.playerInRange = false;
                c.startPosition = transform.position;
                c.target = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * coinForce + transform.position;
                c.coinForce = coinForce;
                c.isSet = true;
                
            }
            soundEffect.GetComponent<AudioSource>().pitch = Random.Range(0.55f, 1.55f);
            Instantiate(soundEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void damage(int damage)
    {
        health -= damage;
    }
}
