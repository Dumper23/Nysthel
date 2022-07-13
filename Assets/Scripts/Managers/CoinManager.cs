using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int startCoins = 100;
    public GameObject lowCoin;
    public GameObject midCoin;
    public GameObject highCoin;

    private List<GameObject> lowCoins = new List<GameObject>();
    private List<GameObject> midCoins = new List<GameObject>();
    private List<GameObject> highCoins = new List<GameObject>();

    private bool notEnoughCoins = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lowCoins = new List<GameObject>();
        midCoins = new List<GameObject>();
        highCoins = new List<GameObject>();
        for (int i = 0; i < startCoins; i++)
        {
            GameObject lc = Instantiate(lowCoin, gameObject.transform);
            GameObject mc = Instantiate(midCoin, gameObject.transform);
            GameObject hc = Instantiate(highCoin, gameObject.transform);
            lc.SetActive(false);
            mc.SetActive(false);
            hc.SetActive(false);
            lowCoins.Add(lc);
            midCoins.Add(mc);
            highCoins.Add(hc);
            
        }
    }

    public GameObject GetCoin(int coinType)
    {
        switch (coinType) {
            case 0:
                for (int i = 0; i < lowCoins.Count; i++)
                {
                    if (!lowCoins[i].activeInHierarchy)
                    {
                        return lowCoins[i];
                    }
                }

                if (notEnoughCoins)
                {
                    GameObject coin = Instantiate(lowCoin, gameObject.transform);
                    coin.SetActive(false);
                    lowCoins.Add(coin);
                    return coin;
                }

                return null;

            case 1:
                for (int i = 0; i < midCoins.Count; i++)
                {
                    if (!midCoins[i].activeInHierarchy)
                    {
                        return midCoins[i];
                    }
                }

                if (notEnoughCoins)
                {
                    GameObject coin = Instantiate(midCoin, gameObject.transform);
                    coin.SetActive(false);
                    midCoins.Add(coin);
                    return coin;
                }

                return null;
                break;

            case 2:
                for (int i = 0; i < highCoins.Count; i++)
                {
                    if (!highCoins[i].activeInHierarchy)
                    {
                        return highCoins[i];
                    }
                }

                if (notEnoughCoins)
                {
                    GameObject coin = Instantiate(highCoin, gameObject.transform);
                    coin.SetActive(false);
                    highCoins.Add(coin);
                    return coin;
                }

                return null;
                break;

            default:
                return null;

        }

    }

    public int getActiveCoinsCount(int coinType)
    {
        int activeCoins = 0;
        switch (coinType)
        {
            case 0:
                foreach (GameObject coin in lowCoins)
                {
                    if (coin.activeInHierarchy)
                    {
                        activeCoins++;
                    }
                }
                return activeCoins;

            case 1:
                foreach (GameObject coin in midCoins)
                {
                    if (coin.activeInHierarchy)
                    {
                        activeCoins++;
                    }
                }
                return activeCoins;

            case 2:
                foreach (GameObject coin in highCoins)
                {
                    if (coin.activeInHierarchy)
                    {
                        activeCoins++;
                    }
                }
                return activeCoins;
            default:

                return 0;
        }
    }

}
