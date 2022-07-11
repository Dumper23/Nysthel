using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject[] explotions;
    public int damage = 45;
    public float minTimeToDetonate;
    public float maxTimeToDetonate;
    public float range = 1.2f;
    public GameObject bombDeath;

    private void Start()
    {
        Invoke("detonate", Random.Range(minTimeToDetonate, maxTimeToDetonate));
    }

    private void detonate()
    {
        Instantiate(explotions[Random.Range(0, explotions.Length)], transform.position, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Player>().takeDamage(damage);
            }
        }
        Instantiate(bombDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}