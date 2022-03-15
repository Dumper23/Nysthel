using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Enemy
{
    public float timeHidden = 3f;
    public GameObject bulletSeek;
    public GameObject bullet;
    public GameObject eyeLight;

    [Range(0f, 1f)]
    public float seekerProbability = 0.5f;


    void Start()
    {
        target = FindObjectOfType<Player>().transform;
        anim.Play("Idle");
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            if (target.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (Time.time > nextShot && Vector3.Magnitude(target.position - transform.position) < range && !immune)
            {
                nextShot = Time.time + attackRate;
                Hide();
            }

            die();
            if (!immune)
            {
                Seek();   
            }
        }
    }

    private void Hide()
    {
        anim.Play("Hide");
        eyeLight.SetActive(false);
        Invoke("Shoot", 0.4f);
    }

    private void Shoot()
    {
        
        if (Random.Range(0f, 1f) > seekerProbability) {
            EnemyBullet b = (Instantiate(bulletSeek, transform.position - new Vector3(0, -0.2f, 0), Quaternion.identity) as GameObject).GetComponent<EnemyBullet>();
            b.damage = 15;
            b.speed = 5;
            b.isSeeker = true;
        }
        else
        {
            EnemyBullet b = (Instantiate(bullet, transform.position - new Vector3(0, -0.2f, 0), Quaternion.identity) as GameObject).GetComponent<EnemyBullet>();
            b.damage = 25;
            b.speed = 8;
            b.isSeeker = false;
            b.setMoveDirection((target.position - transform.position).normalized);
        }

        anim.Play("Hiden");
        immune = true;
        Invoke("Unhide", timeHidden);
    }

    private void Unhide()
    {
        anim.Play("Unhide");
        immune = false;
        Invoke("idleAnim", 0.4f);
    }

    private void idleAnim()
    {
        eyeLight.SetActive(true);
        anim.Play("Idle");
    }

}
