using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public GameObject[] objectToGive;
    public Color colorOfText = new Color(0, 0, 0, 255);
    public bool opened;
    public bool hasMoney = false;
    public bool hasObject = true;
    public GameObject destruction;
    public ParticleSystem destructionParticles;
    public int coinType;
    public bool moneyConvertion = false;
    public float coinForce = 5;

    [Range(0f, 1f)]
    public float breakingProbability;

    [HideInInspector]
    public Animator anim;

    public int moneyQuantity = 0;
    public TextMeshProUGUI moneyText;
    public bool givePrize = false;
    public GameObject chestWinSound;
    public GameObject chestOpenSound;

    [Range(0, 50)]
    public int minCoinStep = 0;

    [Range(0, 50)]
    public int maxCoinStep = 10;

    public Image h1;
    public Image h2;
    public Image h3;

    private int hp = 3;
    private bool immune = false;
    private bool coin2 = false;
    private bool coin3 = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        moneyText.enabled = false;
        moneyText.color = colorOfText;
        if (objectToGive == null || objectToGive.Length <= 0)
        {
            hasObject = false;
        }
    }

    private void Update()
    {
        if (hp == 3)
        {
            h1.enabled = true;
            h2.enabled = true;
            h3.enabled = true;
        }
        else if (hp == 2)
        {
            h1.enabled = true;
            h2.enabled = true;
            h3.enabled = false;
        }
        else if (hp == 1)
        {
            h1.enabled = true;
            h2.enabled = false;
            h3.enabled = false;
        }

        if (opened)
        {
            moneyText.enabled = true;
            moneyText.text = "x" + moneyQuantity;
        }
        else
        {
            moneyText.enabled = false;
        }
        if (hp <= 0)
        {
            if (moneyConvertion)
            {
                if (moneyQuantity > 100)
                {
                    moneyQuantity /= 2;
                    coin2 = true;
                }
                if (moneyQuantity > 100)
                {
                    moneyQuantity /= 3;
                    coin3 = true;
                }
            }
            else
            {
                if(coinType == 1)
                {
                    coin2 = true;
                }else if(coinType == 2)
                {
                    coin3 = true;
                }
            }
            for (int i = 0; i <= moneyQuantity; i++)
            {
                if (coin2 && !coin3)
                {
                    coinType = 1;
                }
                else if (coin3)
                {
                    coinType = 2;
                }
                else
                {
                    coinType = 0;
                }

                GameObject go = CoinManager.Instance.GetCoin(coinType);
                go.SetActive(true);
                go.transform.position = transform.position;
                Coin c = go.GetComponent<Coin>();
                c.playerInRange = false;
                c.startPosition = transform.position;
                c.coinForce = coinForce;
                c.target = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) + transform.position;
                c.isSet = true;
            }
            Instantiate(destruction, transform.position, Quaternion.identity);
            Instantiate(destructionParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            if (!immune)
            {
                hp--;
                Destroy(collision.gameObject);
                immune = true;
                Invoke("vulnerable", 0.25f);
            }
        }
    }

    private void vulnerable()
    {
        immune = false;
    }
}