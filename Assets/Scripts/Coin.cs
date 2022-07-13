using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Vector3 target;
    public float coinForce = 10f;
    public bool isWood = false;
    [HideInInspector]
    public bool isSet = false;
    [HideInInspector]
    public bool playerInRange = false;
    public int coinValue = 1;
    public Vector3 startPosition;

    private float time = 0;
    private bool hasArrived = false;
    private float coinMagnetSpeed = 200;
    private Player p;

    

    private void OnDisable()
    {
        hasArrived = false;
        playerInRange = false;
        time = 0;
    }

    void Update()
    {
        if (!hasArrived && isSet && (transform.position - target).magnitude >= 0.05f)
        {
            transform.position = Vector2.Lerp(startPosition, target, time / 0.4f);
            time += Time.deltaTime;
        }
        else if((transform.position - target).magnitude < 0.05f)
        {
            hasArrived = true;
        }
        
        if (hasArrived && playerInRange)
        {
            transform.Translate((p.transform.position - transform.position) * coinMagnetSpeed * Time.deltaTime);
            if((transform.position - p.transform.position).magnitude <= 1)
            {
                if (p != null)
                {
                    if (isWood)
                    {
                        p.recieveWood(coinValue);
                    }
                    else
                    {
                        p.recieveGold(coinValue);
                    }
                }
                if (!isWood)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("coinContainer"))
        {
            p = collision.GetComponentInParent<Player>();
            playerInRange = true;
            coinMagnetSpeed = p.coinMagnetSpeed;
        }
    }
}
