using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int health = 10;
    public int maxGoldToGive = 1;

    private int goldToGive;

    void Start()
    {
        int g = Random.RandomRange(0, 2) - 1;
        if (g < 0) g = 0;
        goldToGive = g;
    }

    void Update()
    {
        if(health <= 0)
        {
            //instanciar coins al terra
            Destroy(gameObject);
        }
    }

    public void damage(int damage)
    {
        //Instantiate damage particles
        health -= damage;
    }
}
