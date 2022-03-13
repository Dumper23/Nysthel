using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField]
    private GameObject poolBullet;
    private bool notEnoughBullets = true;

    private List<GameObject> bullets;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bullets = new List<GameObject>();
    }

    public GameObject GetBullet()
    {

        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
        }

        if (notEnoughBullets)
        {
            GameObject bul = Instantiate(poolBullet, gameObject.transform);
            bul.SetActive(false);
            bullets.Add(bul);
            return bul;
        }

        return null;
    }



}
