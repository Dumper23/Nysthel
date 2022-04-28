using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public GameObject[] objectToGive;
    public Color colorOfText = new Color(0,0,0,255);
    public bool opened;
    public bool hasMoney = false;
    public GameObject destruction;
    public ParticleSystem destructionParticles;
    public GameObject coinType;
    [Range(0f, 1f)]
    public float breakingProbability;
    [HideInInspector]
    public Animator anim;
    public int moneyQuantity = 0;
    public TextMeshProUGUI moneyText;
    public bool givePrize = false;
    public GameObject chestWinSound;

    public Image h1;
    public Image h2;
    public Image h3;

    private int hp = 3;
    private bool immune = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        moneyText.enabled = false;
        moneyText.color = colorOfText;
    }

    private void Update()
    {
        if(hp == 3)
        {
            h1.enabled = true;
            h2.enabled = true;
            h3.enabled = true;
        }else if (hp == 2)
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
        if(hp <= 0)
        {
            for (int i = 0; i <= moneyQuantity; i++)
            {
                GameObject go = Instantiate(coinType, transform.position, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 10f, ForceMode2D.Impulse);
            }
            Instantiate(destruction);
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
                Invoke("vulnerable",0.5f);
            }
        }
    }

    private void vulnerable()
    {
        immune = false;
    }
}
