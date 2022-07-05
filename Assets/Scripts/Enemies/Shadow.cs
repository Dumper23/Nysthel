using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Enemy
{
    public float timeHidden = 3f;
    public GameObject bulletSeek;
    public GameObject bullet;
    public GameObject eyeLight;
    public AudioClip[] audios;
    public GameObject deathSound;

    [Range(0f, 1f)]
    public float seekerProbability = 0.5f;

    private static int ATTACK_AUDIO = 0;
    private static int HIDE_AUDIO = 1;
    private static int UNHIDE_AUDIO = 2;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            if (health <= 0)
            {
                Instantiate(deathSound, transform.position, Quaternion.identity);
                die();
            }
            if (!immune)
            {
                Seek();   
            }
        }
    }

    private void Hide()
    {
        audioSource.clip = audios[HIDE_AUDIO];
        audioSource.Play();
        anim.Play("Hide");
        eyeLight.SetActive(false);
        Invoke("Shoot", 0.4f);
    }

    private void Shoot()
    {
        audioSource.clip = audios[ATTACK_AUDIO];
        audioSource.Play();
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
        audioSource.clip = audios[UNHIDE_AUDIO];
        audioSource.Play();
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
