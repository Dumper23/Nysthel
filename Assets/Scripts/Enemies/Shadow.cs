using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Enemy
{
    public float timeHidden = 3f;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<Player>().transform;
        anim.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            if (target.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
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
        Invoke("Shoot", 0.4f);
    }

    private void Shoot()
    {

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
        anim.Play("Idle");
    }

}
