using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UiShop : MonoBehaviour
{
    public TextMeshProUGUI rerollGold;
    public Interactable shopInteractable;

    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;
    private List<Transform> templates = new List<Transform>();
    private List<GameObject> itemsToBuy = new List<GameObject>();
    public GameObject upgradeButton;
    public GameObject exitButton;

    private int upgradeShopCost = 50;
    private int lastItemSelected = 0;

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        ShopAssets.Instance.BlackSmithLevel = SaveVariables.BLACKSMITH_LEVEL;

        ShopItem.SetCurrentLevel(ShopItem.ItemType.LifeUpgrade, SaveVariables.LIFE_LEVEL);
        ShopItem.SetCurrentLevel(ShopItem.ItemType.AttackUpgrade, SaveVariables.ATTACK_LEVEL);
        ShopItem.SetCurrentLevel(ShopItem.ItemType.AttackSpeedUpgrade, SaveVariables.ATTACK_SPEED_LEVEL);
        ShopItem.SetCurrentLevel(ShopItem.ItemType.SpeedUpgrade, SaveVariables.SPEED_LEVEL);
        ShopItem.SetCurrentLevel(ShopItem.ItemType.RangeUpgrade, SaveVariables.RANGE_LEVEL);
        ShopItem.SetCurrentLevel(ShopItem.ItemType.DashRecoveryUpgrade, SaveVariables.DASH_RECOVERY_LEVEL);
        ShopItem.SetCurrentLevel(ShopItem.ItemType.DashRangeUpgrade, SaveVariables.DASH_RANGE_LEVEL);

        GenerateUI();
        hide();
    }

    private void Update()
    {
        if (EventSystem.current != null && (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy))
        {
            bool found = false;
            int i = 0;
            while (!found)
            {
                if (templates.Count > 0 && i < templates.Count)
                {
                    if (templates[i].gameObject.activeInHierarchy)
                    {
                        EventSystem.current.SetSelectedGameObject(templates[i].gameObject);
                        found = true;
                    }
                    i++;
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(exitButton);
                    found = true;
                }
            }
        }
    }

    private void GenerateUI()
    {
        if (SaveVariables.BLACKSMITH_LEVEL >= 6)
        {
            upgradeButton.SetActive(false);
        }
        for (int i = 0; i < container.childCount; i++)
        {
            if (container.GetChild(i).name == "ShopItemTemplate(Clone)")
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
        templates.Clear();

        if (ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) > ShopItem.GetCurrentLevel(ShopItem.ItemType.LifeUpgrade) && SaveVariables.BLACKSMITH_LEVEL >= 0)
        {
            CreateItemButton(ShopItem.ItemType.LifeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.LifeUpgrade), "Life Upgrade", ShopItem.GetCost(ShopItem.ItemType.LifeUpgrade), 0);
        }
        if (ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) > ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackUpgrade) && SaveVariables.BLACKSMITH_LEVEL >= 1)
        {
            CreateItemButton(ShopItem.ItemType.AttackUpgrade, ShopItem.GetSprite(ShopItem.ItemType.AttackUpgrade), "Attack Upgrade", ShopItem.GetCost(ShopItem.ItemType.AttackUpgrade), 1);
        }
        if (ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) > ShopItem.GetCurrentLevel(ShopItem.ItemType.SpeedUpgrade) && SaveVariables.BLACKSMITH_LEVEL >= 2)
        {
            CreateItemButton(ShopItem.ItemType.SpeedUpgrade, ShopItem.GetSprite(ShopItem.ItemType.SpeedUpgrade), "Speed Upgrade", ShopItem.GetCost(ShopItem.ItemType.SpeedUpgrade), 2);
        }
        if (ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) > ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackSpeedUpgrade) && SaveVariables.BLACKSMITH_LEVEL >= 3)
        {
            CreateItemButton(ShopItem.ItemType.AttackSpeedUpgrade, ShopItem.GetSprite(ShopItem.ItemType.AttackSpeedUpgrade), "Attack Speed Upgrade", ShopItem.GetCost(ShopItem.ItemType.AttackSpeedUpgrade), 3);
        }
        if (ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) > ShopItem.GetCurrentLevel(ShopItem.ItemType.RangeUpgrade) && SaveVariables.BLACKSMITH_LEVEL >= 4)
        {
            CreateItemButton(ShopItem.ItemType.RangeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.RangeUpgrade), "Range Upgrade", ShopItem.GetCost(ShopItem.ItemType.RangeUpgrade), 4);
        }
        if (ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) > ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRecoveryUpgrade) && SaveVariables.BLACKSMITH_LEVEL >= 5)
        {
            CreateItemButton(ShopItem.ItemType.DashRecoveryUpgrade, ShopItem.GetSprite(ShopItem.ItemType.DashRecoveryUpgrade), "Dash Recovery Upgrade", ShopItem.GetCost(ShopItem.ItemType.DashRecoveryUpgrade), 5);
        }
        if (ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) > ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRangeUpgrade) && SaveVariables.BLACKSMITH_LEVEL >= 6)
        {
            CreateItemButton(ShopItem.ItemType.DashRangeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.DashRangeUpgrade), "Dash Range Upgrade", ShopItem.GetCost(ShopItem.ItemType.DashRangeUpgrade), 6);
        }
    }

    private void CreateItemButton(ShopItem.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        if (ShopItem.GetMaxLevel(itemType) > ShopItem.GetCurrentLevel(itemType))
        {
            Transform shopItemTransform = Instantiate(shopItemTemplate, container);
            RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
            shopItemTransform.gameObject.SetActive(true);

            float shopItemHeight = 55f;
            shopItemRectTransform.anchoredPosition = new Vector2(0, shopItemTransform.localPosition.y - (shopItemHeight * positionIndex));

            shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
            shopItemTransform.Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

            templates.Add(shopItemTransform);

            shopItemTransform.Find("itemIcon").GetComponent<Image>().sprite = itemSprite;

            shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItem(itemType); });

            if (positionIndex == lastItemSelected)
            {
                EventSystem.current.SetSelectedGameObject(shopItemTransform.gameObject);
            }
        }
    }

    private void TryBuyItem(ShopItem.ItemType itemType)
    {
        if (ShopItem.GetMaxLevel(itemType) > ShopItem.GetCurrentLevel(itemType))
        {
            if (shopCustomer.TrySpendGoldAmount(ShopItem.GetCost(itemType)))
            {
                ShopItem.AddLevel(itemType);

                for (int i = 0; i < templates.Count; i++)
                {
                    if (templates[i] != null)
                    {
                        switch (templates[i].Find("itemName").GetComponent<TextMeshProUGUI>().text)
                        {
                            case "Life Upgrade":
                                templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ShopItem.GetCost(ShopItem.ItemType.LifeUpgrade).ToString());
                                break;

                            case "Attack Upgrade":
                                templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ShopItem.GetCost(ShopItem.ItemType.AttackUpgrade).ToString());
                                break;

                            case "Speed Upgrade":
                                templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ShopItem.GetCost(ShopItem.ItemType.SpeedUpgrade).ToString());
                                break;

                            case "Attack Speed Upgrade":
                                templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ShopItem.GetCost(ShopItem.ItemType.AttackSpeedUpgrade).ToString());
                                break;

                            case "Range Upgrade":
                                templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ShopItem.GetCost(ShopItem.ItemType.RangeUpgrade).ToString());
                                break;

                            case "Dash Recovery Upgrade":
                                templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ShopItem.GetCost(ShopItem.ItemType.DashRecoveryUpgrade).ToString());
                                break;

                            case "Dash Range Upgrade":
                                templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ShopItem.GetCost(ShopItem.ItemType.DashRangeUpgrade).ToString());
                                break;
                        }
                    }
                }

                lastItemSelected = shopCustomer.BoughtItem(itemType);
                if (lastItemSelected < templates.Count && lastItemSelected >= 0 && templates[lastItemSelected] != null)
                {
                    EventSystem.current.SetSelectedGameObject(templates[lastItemSelected].gameObject);
                }

                Transform t = transform.Find("Panel").Find("Statistics");
                float[] s = shopCustomer.GetStatistics();
                t.GetComponent<TextMeshProUGUI>().text =
                    "Statistics:" +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.LifeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) + ") Health:\t\t" + s[1] +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) + ") Attack:\t\t" + s[0] +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.SpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) + ") Speed:\t\t" + s[2].ToString("F2") +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackSpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) + ") Attack Speed:\t\t" + s[3].ToString("F2") +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.RangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) + ") Magnet Range:\t\t" + s[6].ToString("F2") +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRecoveryUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) + ") Dash Recv.:\t\t" + s[4].ToString("F2") +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) + ") Dash Range:\t\t" + s[5].ToString("F2");
            }
            SaveManager.Instance.SaveGame();
        }
        else
        {
            //Hide button or somthing
            for (int i = 0; i < templates.Count; i++)
            {
                if (templates[i] != null)
                {
                    if (templates[i].Find("itemName") != null)
                    {
                        if (templates[i].Find("itemName").GetComponent<TextMeshProUGUI>() != null)
                        {
                            switch (templates[i].Find("itemName").GetComponent<TextMeshProUGUI>().text)
                            {
                                case "Life Upgrade":
                                    if (ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) <= ShopItem.GetCurrentLevel(ShopItem.ItemType.LifeUpgrade))
                                    {
                                        templates[i].gameObject.SetActive(false);
                                    }
                                    break;

                                case "Attack Upgrade":
                                    if (ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) <= ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackUpgrade))
                                    {
                                        templates[i].gameObject.SetActive(false);
                                    }
                                    break;

                                case "Speed Upgrade":
                                    if (ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) <= ShopItem.GetCurrentLevel(ShopItem.ItemType.SpeedUpgrade))
                                    {
                                        templates[i].gameObject.SetActive(false);
                                    }
                                    break;

                                case "Attack Speed Upgrade":
                                    if (ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) <= ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackSpeedUpgrade))
                                    {
                                        templates[i].gameObject.SetActive(false);
                                    }
                                    break;

                                case "Range Upgrade":
                                    if (ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) <= ShopItem.GetCurrentLevel(ShopItem.ItemType.RangeUpgrade))
                                    {
                                        templates[i].gameObject.SetActive(false);
                                    }
                                    break;

                                case "Dash Recovery Upgrade":
                                    if (ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) <= ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRecoveryUpgrade))
                                    {
                                        templates[i].gameObject.SetActive(false);
                                    }
                                    break;

                                case "Dash Range Upgrade":
                                    if (ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) <= ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRangeUpgrade))
                                    {
                                        templates[i].gameObject.SetActive(false);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            //EventSystem.current.SetSelectedGameObject(templates[itemsShown[0]].gameObject);
        }
    }

    public void show(IShopCustomer shopCustomer)
    {
        ShopAssets.Instance.BlackSmithLevel = SaveVariables.BLACKSMITH_LEVEL;
        this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
        Transform t = transform.Find("Panel").Find("Statistics");
        float[] s = shopCustomer.GetStatistics();
        t.GetComponent<TextMeshProUGUI>().text =
            "Statistics:" +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.LifeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) + ") Health:\t\t" + s[1] +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) + ") Attack:\t\t" + s[0] +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.SpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) + ") Speed:\t\t" + s[2].ToString("F2") +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackSpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) + ") Attack Speed:\t\t" + s[3].ToString("F2") +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.RangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) + ") Magnet Range:\t\t" + s[6].ToString("F2") +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRecoveryUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) + ") Dash Recv.:\t\t" + s[4].ToString("F2") +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) + ") Dash Range:\t\t" + s[5].ToString("F2");

        if (lastItemSelected < templates.Count && templates[lastItemSelected] != null)
        {
            EventSystem.current.SetSelectedGameObject(templates[lastItemSelected].gameObject);
        }
    }

    public void hide()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inShop = false;
        gameObject.SetActive(false);
    }

    public void UpgradeShop()
    {
        if (SaveVariables.BLACKSMITH_LEVEL < 6 && shopCustomer.TrySpendGoldAmount(upgradeShopCost))
        {
            ShopAssets.Instance.BlackSmithLevel++;
            SaveVariables.BLACKSMITH_LEVEL = ShopAssets.Instance.BlackSmithLevel;
            if (SaveVariables.BLACKSMITH_LEVEL >= 6)
            {
                upgradeButton.SetActive(false);
            }
            GenerateUI();
        }
        if (SaveVariables.BLACKSMITH_LEVEL >= 6)
        {
            upgradeButton.SetActive(false);
        }
    }

    public void exitShop()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inShop = false;
        gameObject.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Invoke("endShop", 0.1f);
        Time.timeScale = 1f;
    }

    private void endShop()
    {
        shopInteractable.setInShop(false);
    }
}