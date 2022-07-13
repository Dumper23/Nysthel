using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmeable : MonoBehaviour
{
    public GameObject destroyParticles;
    public GameObject wood;
    public int maxWood = 10;
    public int minWood = 1;
    public float woodForce = 10f;
    public int hitsToDestroy;
    public GameObject destroySound;
    public AudioSource aud;

    private bool dead = false;
    private Animator animator;
    private Vector3 pos;

    private void Start()
    {
        animator = GetComponent<Animator>();
        destroySound.GetComponent<AudioSource>().pitch = Random.Range(0.55f, 1.55f);
        destroySound.GetComponent<AudioSource>().volume = 0.3f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet" && !dead)
        {
            hitsToDestroy--;
            aud.Play();
            if (FindObjectOfType<Player>().transform.position.x >= transform.position.x)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !animator.GetCurrentAnimatorStateInfo(0).IsName("HitRight"))
                {
                    animator.Play("Hit");
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !animator.GetCurrentAnimatorStateInfo(0).IsName("HitRight"))
                {
                    animator.Play("HitRight");
                }
            }

            if (hitsToDestroy > 0)
            {
                Invoke("stopAnim", 0.3f);
            }
        }
    }

    private void stopAnim()
    {
        animator.Play("Idle");
    }

    private void Update()
    {
        if(hitsToDestroy <= 0 && !dead)
        {
            dead = true;
            if (FindObjectOfType<Player>().transform.position.x >= transform.position.x)
            {
                animator.StopPlayback();
                animator.Play("DieLeft");
                pos = transform.position + new Vector3(-2, 0, 0);
            }
            else
            {
                animator.StopPlayback();
                animator.Play("DieRight");
                pos = transform.position + new Vector3(2, 0, 0);
            }
            
            Invoke("destryObj", 0.85f);
        }
    }

    private void destryObj()
    {     
        Instantiate(destroyParticles, pos, Quaternion.identity);
        Instantiate(destroySound, pos, Quaternion.identity);
        for (int i = 0; i < Random.Range(minWood, maxWood); i++)
        {
            GameObject g = Instantiate(wood, pos, Quaternion.identity);
            Coin c = g.GetComponent<Coin>();
            c.playerInRange = false;
            c.startPosition = transform.position;
            c.target = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * woodForce + new Vector3(transform.position.x, transform.position.y, 0);
            c.coinForce = woodForce;
            c.isSet = true;
        }
        Destroy(gameObject);

    }
}
