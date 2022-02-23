using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int health = 10;
    public int maxGoldToGive = 1;
    public GameObject coin;
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
                GameObject g = Instantiate(coin, transform.position, transform.rotation);
                g.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * coinForce, ForceMode2D.Impulse);
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
