using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health = 20;
    public int damage = 10;
    public int maxGoldToGive = 5;
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public Animator anim;
    public Transform firePoint;
    public float attackRate = 1f;
    public float range = 2f;
    public GameObject coin;
    public float coinForce = 2f;

    protected int goldToGive;
    protected string currentState;

    public void takeDamage(int value)
    {
        health -= value;
    }

    protected void die()
    {
        if (health <= 0)
        {
            int g = Random.Range(0, maxGoldToGive);
            if (g < 0) g = 0;
            goldToGive = g;
            for (int i = 0; i <= goldToGive; i++)
            {
                GameObject go = Instantiate(coin, transform.position, transform.rotation);
                go.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * coinForce, ForceMode2D.Impulse);

            }
            Destroy(gameObject);
        }
    }

    protected void changeAnimationState(string newState)
    {
        //We avoid playing the same animation multiple times
        if (currentState == newState) return;

        //We play a determinated animation
        anim.Play(newState);

        currentState = newState;
    }
}
