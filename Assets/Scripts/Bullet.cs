using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0, 1)]
    public float range = 1f;
    public int damage = 10;

    private void Start()
    {
        Destroy(gameObject, range);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Destructible")
        {
            collision.transform.GetComponent<DestructibleObject>().damage(damage);
        }
        Destroy(gameObject);
    }

    
}
