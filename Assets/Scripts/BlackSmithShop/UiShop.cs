using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UiShop : MonoBehaviour
{
    public TextMeshProUGUI rerollGold;

    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;
    private List<Transform> templates = new List<Transform>();
    private List<GameObject> itemsToBuy = new List<GameObject>();

    private int minItemsShown = 2;
    private int[] itemsShown;
    private int reRollCost;
    private int upgradeShopCost = 200;
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

    private void GenerateUI()
    {
        minItemsShown = ShopAssets.Instance.BlackSmithLevel + 1;
        reRollCost = (ShopAssets.Instance.BlackSmithLevel+1) * 50;
        rerollGold.text = reRollCost.ToString();
        itemsShown = new int[minItemsShown];

        for (int i = 0; i < minItemsShown; i++)
        {
            itemsShown[i] = Random.Range(0, System.Enum.GetValues(typeof(ShopItem.ItemType)).Length);
        }

        for (int i = 0; i < container.childCount; i++)
        {
            if (container.GetChild(i).name == "ShopItemTemplate(Clone)")
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
        templates.Clear();

        CreateItemButton(ShopItem.ItemType.LifeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.LifeUpgrade), "Life Upgrade", ShopItem.GetCost(ShopItem.ItemType.LifeUpgrade), 0);

        CreateItemButton(ShopItem.ItemType.AttackUpgrade, ShopItem.GetSprite(ShopItem.ItemType.AttackUpgrade), "Attack Upgrade", ShopItem.GetCost(ShopItem.ItemType.AttackUpgrade), 1);

        CreateItemButton(ShopItem.ItemType.SpeedUpgrade, ShopItem.GetSprite(ShopItem.ItemType.SpeedUpgrade), "Speed Upgrade", ShopItem.GetCost(ShopItem.ItemType.SpeedUpgrade), 2);

        CreateItemButton(ShopItem.ItemType.AttackSpeedUpgrade, ShopItem.GetSprite(ShopItem.ItemType.AttackSpeedUpgrade), "Attack Speed Upgrade", ShopItem.GetCost(ShopItem.ItemType.AttackSpeedUpgrade), 3);

        CreateItemButton(ShopItem.ItemType.RangeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.RangeUpgrade), "Range Upgrade", ShopItem.GetCost(ShopItem.ItemType.RangeUpgrade), 4);

        CreateItemButton(ShopItem.ItemType.DashRecoveryUpgrade, ShopItem.GetSprite(ShopItem.ItemType.DashRecoveryUpgrade), "Dash Recovery Upgrade", ShopItem.GetCost(ShopItem.ItemType.DashRecoveryUpgrade), 5);

        CreateItemButton(ShopItem.ItemType.DashRangeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.DashRangeUpgrade), "Dash Range Upgrade", ShopItem.GetCost(ShopItem.ItemType.DashRangeUpgrade), 6);

    }

    private void CreateItemButton(ShopItem.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
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

        for (int i = 0; i < minItemsShown; i++)
        {
            if (itemsShown[i] == positionIndex) {
                shopItemTransform.GetComponent<Button>().interactable = true;
                break;
            }
            else
            {
                shopItemTransform.GetComponent<Button>().interactable = false;
            }
        }

        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItem(itemType); });
        if (positionIndex == lastItemSelected)
        {
            EventSystem.current.SetSelectedGameObject(shopItemTransform.gameObject);
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
                lastItemSelected = shopCustomer.BoughtItem(itemType);
                EventSystem.current.SetSelectedGameObject(templates[lastItemSelected].gameObject);
                
                Transform t = transform.Find("Panel").Find("Statistics");
                float[] s = shopCustomer.GetStatistics();
                t.GetComponent<TextMeshProUGUI>().text =
                    "Statistics:" +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.LifeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) + ") Health:\t\t" + s[1] +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) + ") Attack:\t\t\t" + s[0] +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.SpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) + ") Speed:\t\t\t" + s[2].ToString("F2") +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackSpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) + ") Attack Speed:\t" + s[3].ToString("F2") + " (attacks/s)" +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.RangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) + ") Magnet Range:\t" + s[6].ToString("F2") +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRecoveryUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) + ") Dash Recv.:\t\t" + s[4].ToString("F2") +
                    "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) + ") Dash Range:\t\t" + s[5].ToString("F2");
            }
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
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) + ") Attack:\t\t\t" + s[0] +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.SpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) + ") Speed:\t\t\t" + s[2].ToString("F2") +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.AttackSpeedUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) + ") Attack Speed:\t" + s[3].ToString("F2") + " (attacks/s)" +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.RangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) + ") Magnet Range:\t" + s[6].ToString("F2") +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRecoveryUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) + ") Dash Recv.:\t\t" + s[4].ToString("F2") +
            "\n(" + ShopItem.GetCurrentLevel(ShopItem.ItemType.DashRangeUpgrade) + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) + ") Dash Range:\t\t" + s[5].ToString("F2");

        EventSystem.current.SetSelectedGameObject(templates[lastItemSelected].gameObject);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeShop()
    {
        if (shopCustomer.TrySpendGoldAmount(upgradeShopCost))
        {
            ShopAssets.Instance.BlackSmithLevel++;
            SaveVariables.BLACKSMITH_LEVEL = ShopAssets.Instance.BlackSmithLevel;
            GenerateUI();

        }
    }

    public void ReRoll()
    {
        if (shopCustomer.TrySpendGoldAmount(reRollCost))
        {
            GenerateUI();
        }
    }
}
