  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Fungus : Enemy
{
    private float angle = 0f;
    public float angleStep = 10f;
    public GameObject bullet;
    public float bulletForce = 10f;
    public Sprite bulletSprite;
    public bool isRandomShooter = true;

    public AudioClip[] audios;
    public GameObject afterDieSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        changeAnimationState("Idle");
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (activated)
        {

            if (health <= 0)
            {
                Instantiate(afterDieSound, transform.position, Quaternion.identity);
                die();
            }

            if ((target.position - transform.position).magnitude <= range)
            {
                if (Time.time > nextShot)
                {
                    nextShot = Time.time + attackRate;
                    audioSource.clip = audios[0];
                    audioSource.Play();
                    shoot();
                }
            }
            else
            {
                changeAnimationState("Idle");
            }
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
        bul.GetComponent<BulletHellBullet>().damage = damage;
        
        bul.transform.position = firePoint.position;
        bul.transform.rotation = firePoint.rotation;

        if (isRandomShooter)
        {
            changeAnimationState("Attack");  
        }

        bul.SetActive(true);
        bul.GetComponent<BulletHellBullet>().SetMoveDirection(bulDir);

        angle += angleStep;

    }
}
