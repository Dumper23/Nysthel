using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class goldRushEnemy : Enemy
{
    public int level = 0;
    public int timesDead = 0;
    public TextMeshPro levelText;
    public SpriteRenderer sp;
    public int restartCoins = 0;
    public List<Sprite> sprites = new List<Sprite>();

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("TransparentFX"));
        levelText.SetText("Lvl: " + level);
        level = 0;
        sp.sprite = sprites[0];
        target = FindObjectOfType<Player>().transform;
    }

    private void OnEnable()
    {
        levelText.SetText("Lvl: " + level);
        if (level > 0)
        {
            if (level > 2)
            {
                maxGoldToGive = Mathf.RoundToInt(level / 2) - restartCoins;
                minGoldToGive = Mathf.RoundToInt(level / 2) - restartCoins;
            }
            if (level > 6)
            {
                health = (startHealth * Mathf.RoundToInt(level * 1.33f));
            }
            else
            {
                health = (startHealth * level);
            }
        }
        else
        {
            health = startHealth;
        }

        if (sprites != null && sprites.Count > 0 && level < sprites.Count)
        {
            sp.sprite = sprites[level];
        }
        if (level < Mathf.RoundToInt(sprites.Count * 0.15f))
        {
            levelText.faceColor = new Color32(255, 0, 209, 255);
            coinType = 0;
        }
        if (level >= Mathf.RoundToInt(sprites.Count * 0.15f) && level < Mathf.RoundToInt(sprites.Count * 0.35f))
        {
            levelText.faceColor = new Color32(0, 251, 255, 255);
            coinType = 0;
        }
        if (level >= Mathf.RoundToInt(sprites.Count * 0.35f) && level < Mathf.RoundToInt(sprites.Count * 0.5f))
        {
            levelText.faceColor = new Color32(0, 61, 250, 255);
            coinType = 1;
        }
        if (level >= Mathf.RoundToInt(sprites.Count * 0.5f) && level < Mathf.RoundToInt(sprites.Count * 0.75f))
        {
            levelText.faceColor = new Color32(255, 147, 0, 255);
            coinType = 1;
            restartCoins = Mathf.RoundToInt(sprites.Count * 0.5f);
        }
        if (level >= Mathf.RoundToInt(sprites.Count * 0.75f) && level < sprites.Count)
        {
            levelText.faceColor = new Color32(255, 241, 0, 255);
            coinType = 2;
            restartCoins = Mathf.RoundToInt(sprites.Count * 0.75f);
        }
        if (level >= sprites.Count)
        {
            levelText.faceColor = new Color32(255, 0, 0, 255);
            coinType = 2;
        }
    }

    private void Update()
    {
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        if (currentGameState == GameState.Gameplay)
        {
            if (target.position.x > transform.position.x)
            {
                sp.flipX = true;
            }
            else
            {
                sp.flipX = false;
            }

            if (Mathf.Abs(rb.velocity.magnitude) <= 0.2f)
            {
                rb.velocity = (target.position - transform.position).normalized * -moveSpeed;
            }
            else
            {
                rb.velocity = (target.position - transform.position).normalized * moveSpeed;
            }
            disapear();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    protected void disapear()
    {
        if (health <= 0)
        {
            Statistics.Instance.enemiesKilled += 1;
            if (bloodStain != null)
            {
                Instantiate(bloodStain, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
            if (bloodParticles != null)
            {
                Instantiate(bloodParticles, transform.position, Quaternion.Euler(90, 0, 0));
            }
            int g = Random.Range(minGoldToGive, maxGoldToGive);
            if (g < 0) g = 0;
            goldToGive = g;
            if (g > 0)
            {
                for (int i = 0; i <= goldToGive; i++)
                {
                    GameObject go = CoinManager.Instance.GetCoin(coinType);
                    go.SetActive(true);
                    go.transform.position = transform.position;
                    Coin c = go.GetComponent<Coin>();
                    c.playerInRange = false;
                    c.startPosition = transform.position;
                    c.target = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) + transform.position;
                    c.coinForce = coinForce;
                    c.isSet = true;
                }
            }
            timesDead++;
            if (timesDead % 2 == 0)
            {
                level++;
                damage++;
            }
            //waveZoneManager.Instance.enemiesKilled++;
            gameObject.SetActive(false);
        }
    }
}