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
        enemies = Physics2D.OverlapCircleAll(transform.position, 3.5f);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>() && !enemy.GetComponent<Enemy>().isFrozen)
            {
                Instantiate(iceHit, enemy.transform.position, Quaternion.identity, enemy.transform);
                Instantiate(ADSIce, transform.position, Quaternion.identity, enemy.transform);
                enemy.GetComponent<Enemy>().freeze();
            }
        }
    }
}