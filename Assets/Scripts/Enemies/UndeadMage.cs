using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMage : Enemy
{
    public GameObject undeadMinion;
    [Range(1, 4)]
    public int minionQuantity;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            die();
            Seek();

            if (target.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

            if (Time.time > nextShot && Vector3.Magnitude(target.position - transform.position) < range)
            {
                nextShot = Time.time + attackRate;
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        if (minionQuantity == 1)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
        }
        else if(minionQuantity == 2)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(0, -1), Quaternion.identity);
        }
        else if(minionQuantity == 3)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(0, -1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(1, 0), Quaternion.identity);
        }else if(minionQuantity == 4)
        {
            Instantiate(undeadMinion, transform.position + new Vector3(0, 1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(0, -1), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(1, 0), Quaternion.identity);
            Instantiate(undeadMinion, transform.position + new Vector3(-1, 0), Quaternion.identity);
        }
    }
}
