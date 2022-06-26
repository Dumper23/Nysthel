using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Ulrus : Enemy
{
    public SpriteRenderer sprite;
    public float meleeRange = 1f;

    private string[] state;
    private string cState;
    private float time = 0;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        state = new string[7];
        state[0] = "melee";
        state[1] = "rangedAttack";
        state[2] = "areaAttack";
        state[3] = "teleport";
        state[4] = "walk";
        state[5] = "idle";
        state[6] = "laser";

        cState = state[Random.Range(0, state.Length)];
    }

    // Update is called once per frame
    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            if (target.transform.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

            time += Time.deltaTime;
            if (time >= attackRate)
            {
                time = 0;
                ChangeState();
            }
            switch (cState)
            {
                case "walk":
                    moveSpeed = 3.1f;
                    Seek();
                    changeAnimationState("walk");
                    break;

                case "idle":
                    changeAnimationState("idle");
                    break;

                case "melee":
                    moveSpeed = 6.5f;
                    if ((target.position - transform.position).magnitude > meleeRange)
                    {
                        changeAnimationState("walk");
                        Seek();
                    }
                    else
                    {
                        //attack
                        changeAnimationState("melee");
                    }
                    break;

                case "teleport":
                    break;

                case "laser":
                    break;

                case "areaAttack":
                    break;

                case "rangedAttack":
                    break;
            }
        }
    }

    private void ChangeState()
    {
        cState = state[Random.Range(0, state.Length)];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}