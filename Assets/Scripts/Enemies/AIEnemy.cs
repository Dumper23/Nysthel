using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : Enemy
{
    public float stoppingRange = 1f;
    public GameObject lance;
    public float lanceRotationSpeed = 0.02f;
    public bool isRanged = false;
    public GameObject arrow;
    public float angleSpread = 20;

    private NavMeshAgent Agent;
    private float time = 0;
    private bool attacking = false;

    // Start is called before the first frame update
    private void Start()
    {
        time = attackRate;
        target = FindObjectOfType<Player>().transform;
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            if (!attacking)
            {
                float angle = Mathf.Atan2((target.position - lance.transform.position).normalized.y, (target.position - lance.transform.position).normalized.x) * Mathf.Rad2Deg;
                lance.transform.rotation = Quaternion.Lerp(lance.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), lanceRotationSpeed);
            }
            if ((transform.position - target.position).magnitude <= range)
            {
                Agent.SetDestination(target.position);

                if (Mathf.Abs((target.position - transform.position).magnitude) <= stoppingRange)
                {
                    //Stop

                    time += Time.deltaTime;
                    if (time >= attackRate)
                    {
                        //Attack
                        time = 0;
                        attacking = true;
                        if (!isRanged)
                        {
                            anim.Play("attack");
                            Invoke("endAttack", 0.8f);
                        }
                        else
                        {
                            GameObject go2 = Instantiate(arrow, firePoint.position, Quaternion.identity);
                            float angle2 = Mathf.Atan2((target.position - go2.transform.position).normalized.y, (target.position - go2.transform.position).normalized.x) * Mathf.Rad2Deg;
                            go2.transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle2 + 225, Vector3.forward);
                            go2.GetComponent<EnemyBullet>().damage = damage;
                            go2.GetComponent<EnemyBullet>().setMoveDirection((target.position - go2.transform.position).normalized);
                            Invoke("endAttack", 0.5f);
                        }
                    }

                    Agent.velocity = Vector2.zero;
                    Agent.isStopped = true;
                }
                else
                {
                    time = attackRate;
                    Agent.isStopped = false;
                }
            }
            die();
        }
    }

    private void endAttack()
    {
        attacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, stoppingRange);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}