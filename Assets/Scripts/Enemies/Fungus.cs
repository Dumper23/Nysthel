using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fungus : Enemy
{
    private float angle = 0f;
    public GameObject bullet;
    public float bulletForce = 10f;
    public Sprite bulletSprite;
    public bool isRandomShooter = true;

    void Start()
    {
        changeAnimationState("Idle");
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        die();

        if((target.position - transform.position).magnitude <= range)
        {
            if (Time.time > nextShot)
            {
                nextShot = Time.time + attackRate;
                shoot();
            }
        }
        else
        {
            changeAnimationState("Idle");
        }
    }

    private void shoot()
    {
        
        float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
        float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

        Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
        Vector2 bulDir = (bulMoveVector - transform.position).normalized;
        
        GameObject bul = BulletPool.Instance.GetBullet();
        bul.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        
        bul.transform.position = firePoint.position;
        bul.transform.rotation = firePoint.rotation;

        if (isRandomShooter)
        {
            changeAnimationState("Attack");  
        }

        bul.SetActive(true);
        bul.GetComponent<BulletHellBullet>().SetMoveDirection(bulDir);

        angle += 10f;

    }
}
