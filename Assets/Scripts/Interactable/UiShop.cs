using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UiShop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;
    private List<Transform> templates = new List<Transform>();

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

        CreateItemButton(ShopItem.ItemType.LifeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.LifeUpgrade), "Life Upgrade", ShopItem.GetCost(ShopItem.ItemType.LifeUpgrade), 0);
        CreateItemButton(ShopItem.ItemType.AttackUpgrade, ShopItem.GetSprite(ShopItem.ItemType.AttackUpgrade), "Attack Upgrade", ShopItem.GetCost(ShopItem.ItemType.AttackUpgrade), 1);
        CreateItemButton(ShopItem.ItemType.SpeedUpgrade, ShopItem.GetSprite(ShopItem.ItemType.SpeedUpgrade), "Speed Upgrade", ShopItem.GetCost(ShopItem.ItemType.SpeedUpgrade), 2);
        CreateItemButton(ShopItem.ItemType.AttackSpeedUpgrade, ShopItem.GetSprite(ShopItem.ItemType.AttackSpeedUpgrade), "Attack Speed Upgrade", ShopItem.GetCost(ShopItem.ItemType.AttackSpeedUpgrade), 3);
        CreateItemButton(ShopItem.ItemType.RangeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.RangeUpgrade), "Range Upgrade", ShopItem.GetCost(ShopItem.ItemType.RangeUpgrade), 4);
        CreateItemButton(ShopItem.ItemType.DashRecoveryUpgrade, ShopItem.GetSprite(ShopItem.ItemType.DashRecoveryUpgrade), "Dash Recovery Upgrade", ShopItem.GetCost(ShopItem.ItemType.DashRecoveryUpgrade), 5);
        CreateItemButton(ShopItem.ItemType.DashRangeUpgrade, ShopItem.GetSprite(ShopItem.ItemType.DashRangeUpgrade), "Dash Range Upgrade", ShopItem.GetCost(ShopItem.ItemType.DashRangeUpgrade), 6);
        hide();
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

        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItem(itemType); });
        if (positionIndex == 0)
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
                ShopAssets.Instance.BlackSmithLevel++;
                SaveVariables.BLACKSMITH_LEVEL = ShopAssets.Instance.BlackSmithLevel;

                for (int i = 0; i < templates.Count; i++)
                {
                    switch (templates[i].Find("itemName").GetComponent<TextMeshProUGUI>().text)
                    {
                        case "Life Upgrade":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText((300 * (ShopAssets.Instance.BlackSmithLevel + 1)).ToString());
                            break;
                        case "Attack Upgrade":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText((300 * (ShopAssets.Instance.BlackSmithLevel + 1)).ToString());
                            break;
                        case "Speed Upgrade":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText((150 * (ShopAssets.Instance.BlackSmithLevel + 1)).ToString());
                            break;
                        case "Attack Speed Upgrade":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText((250 * (ShopAssets.Instance.BlackSmithLevel + 1)).ToString());
                            break;
                        case "Range Upgrade":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText((150 * (ShopAssets.Instance.BlackSmithLevel + 1)).ToString());
                            break;
                        case "Dash Recovery Upgrade":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText((200 * (ShopAssets.Instance.BlackSmithLevel + 1)).ToString());
                            break;
                        case "Dash Range Upgrade":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText((150 * (ShopAssets.Instance.BlackSmithLevel + 1)).ToString());
                            break;
                    }
                }
                shopCustomer.BoughtItem(itemType);
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
        EventSystem.current.SetSelectedGameObject(templates[0].gameObject);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
}
