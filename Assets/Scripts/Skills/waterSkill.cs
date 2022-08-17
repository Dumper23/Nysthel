using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterSkill : MonoBehaviour
{
    public bool isFrozen = false;
    public GameObject ice;

    private Collider2D[] enemies;
    private GameObject freezeEffect;
    private GameObject freezeSound;

    private void Update()
    {
        if (isFrozen && freezeEffect != null && freezeSound != null)
        {
            froze(freezeEffect, freezeSound);
        }
    }

    public void froze(GameObject iceHit, GameObject ADSIce)
    {
        ice.SetActive(true);
        freezeEffect = iceHit;
        freezeSound = ADSIce;
        isFrozen = true;
        enemies = Physics2D.OverlapBoxAll(new Vector3(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y + GetComponent<BoxCollider2D>().offset.y, 0), GetComponent<BoxCollider2D>().size * 1.4f, 0);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>() && !enemy.GetComponent<Enemy>().isFrozen && !enemy.GetComponent<Enemy>().immuneToElementalEffects)
            {
                Instantiate(iceHit, enemy.transform.position, Quaternion.identity, enemy.transform);
                Instantiate(ADSIce, transform.position, Quaternion.identity, enemy.transform);
                enemy.GetComponent<Enemy>().freeze();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 3.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y + GetComponent<BoxCollider2D>().offset.y, 0), GetComponent<BoxCollider2D>().size * 1.4f);
    }
}