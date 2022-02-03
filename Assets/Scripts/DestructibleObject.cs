using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int health = 10;
    public int maxGoldToGive = 1;
    public GameObject coin;
    public float coinForce = 20f;

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
            for (int i = 0; i <= goldToGive; i++) {
                GameObject g = Instantiate(coin, transform.position, transform.rotation);
                g.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * coinForce, ForceMode2D.Impulse);

            }
            Destroy(gameObject);
        }
    }

    public void damage(int damage)
    {
        //Instantiate damage particles
        health -= damage;
    }
}