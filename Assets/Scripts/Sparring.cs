using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparring : Enemy
{
    void Start()
    {
        activated = true;
        damage = 0;
        immune = false;
        health = 999999999;
    }

    private void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            if (FindObjectOfType<Player>().transform.position.x >= transform.position.x)
            {
                anim.Play("damageLeft");
            }
            else
            {
                anim.Play("damageRight");
            }
        }
    }

}
