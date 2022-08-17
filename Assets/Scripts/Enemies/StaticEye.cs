using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEye : Enemy
{
    public float defDistance = 100f;
    public LineRenderer lineRenderer;
    public bool rebote = false;
    public bool rotate = false;
    public float rotationSpeed = 15f;
    public ParticleSystem hitParticles;
    public GameObject hitLight;
    public GameObject deathSound;

    private bool isSounding = false;
    private Rotating rotation;
    private AudioSource audioSource;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.pitch = Random.Range(0.70f, 0.90f);
        if (rotate)
        {
            rotation = gameObject.AddComponent<Rotating>();
            rotation.center = this.transform;
            rotation.rotationSpeed = rotationSpeed;
            rotation.enabled = false;
        }
    }

    private void Update()
    {
        if (activated && GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            if (target != null && target.gameObject.GetComponent<Player>() && target.gameObject.GetComponent<Player>().scare)
            {
                isScared = true;
            }
            else
            {
                isScared = false;
            }
            if (!isSounding)
            {
                audioSource.Play();
                isSounding = true;
            }
            if (rotate)
            {
                rotation.enabled = true;
            }
            if (health <= 0)
            {
                Instantiate(deathSound, transform.position, Quaternion.identity);
                die();
            }
            if (!isScared && !isFrozen)
            {
                hitParticles.gameObject.SetActive(true);
                hitLight.gameObject.SetActive(true);
                lineRenderer.enabled = true;
                shootLaser();
            }
            else
            {
                hitParticles.gameObject.SetActive(false);
                hitLight.gameObject.SetActive(false);
                lineRenderer.enabled = false;
            }
        }
    }

    private void shootLaser()
    {
        if (Physics2D.Raycast(firePoint.position, firePoint.right, 1000, LayerMask.NameToLayer("Eenmy")))
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right);
            Vector3 pos = hit.point;
            if (hit.transform.tag == "mirror" && rebote)
            {
                Vector2 dir = Vector2.Reflect(firePoint.right, hit.normal);
                if (Physics2D.Raycast(pos, ((Vector2)pos + dir.normalized * defDistance)))
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(pos, ((Vector2)pos + dir.normalized * defDistance));
                    Vector3 pos2 = hit2.point;
                    if (hit2.transform.tag == "mirror" && rebote)
                    {
                        Vector2 dir2 = Vector2.Reflect(pos2 - pos, hit2.normal);
                        if (Physics2D.Raycast(pos2, ((Vector2)pos2 + dir2.normalized * defDistance)))
                        {
                            RaycastHit2D hit3 = Physics2D.Raycast(pos2, ((Vector2)pos2 + dir2.normalized * defDistance));

                            drawRay(firePoint.position, pos, ((Vector2)pos + dir.normalized * hit2.distance), ((Vector2)pos2 + dir2.normalized * hit3.distance));
                            if (hit3.transform.tag == "Player")
                            {
                                if (hit3.transform.GetComponent<Player>())
                                {
                                    hit3.transform.GetComponent<Player>().takeDamage(damage / 3);
                                }
                            }
                        }
                        else
                        {
                            if (hit2.transform.tag == "Player")
                            {
                                if (hit2.transform.GetComponent<Player>())
                                {
                                    hit2.transform.GetComponent<Player>().takeDamage(damage / 2);
                                }
                            }
                            drawRay(firePoint.position, pos, ((Vector2)pos + dir.normalized * hit2.distance));
                        }
                    }
                }
            }
            else
            {
                hitParticles.transform.position = pos;
                hitLight.transform.position = pos;
                drawRay(firePoint.position, pos);
            }
            if (hit && hit.transform.tag == "Player")
            {
                if (hit.transform.GetComponent<Player>())
                {
                    hit.transform.GetComponent<Player>().takeDamage(damage);
                }
            }
        }
    }

    private void drawRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    private void drawRay(Vector2 startPos, Vector2 pos1, Vector2 endPos)
    {
        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, pos1);
        lineRenderer.SetPosition(2, endPos);
    }

    private void drawRay(Vector2 startPos, Vector2 pos1, Vector2 pos2, Vector2 endPos)
    {
        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, pos1);
        lineRenderer.SetPosition(2, pos2);
        lineRenderer.SetPosition(3, endPos);
    }
}