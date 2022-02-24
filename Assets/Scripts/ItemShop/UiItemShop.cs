using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UiItemShop : MonoBehaviour
{

    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;
    private List<Transform> templates = new List<Transform>();

    private int lastItemSelected = 0;

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ItemShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        GenerateUI();
        hide();
    }

    private void GenerateUI()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            if (container.GetChild(i).name == "ItemShopItemTemplate(Clone)")
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
        templates.Clear();

        CreateItemButton(ItemShopItem.ItemType.smallHealthPotion, ItemShopItem.GetSprite(ItemShopItem.ItemType.smallHealthPotion), "Health potion", ItemShopItem.GetCost(ItemShopItem.ItemType.smallHealthPotion), 0);

        CreateItemButton(ItemShopItem.ItemType.bigHealthPotion, ItemShopItem.GetSprite(ItemShopItem.ItemType.bigHealthPotion), "Big health potion", ItemShopItem.GetCost(ItemShopItem.ItemType.bigHealthPotion), 1);

        CreateItemButton(ItemShopItem.ItemType.shieldPotion, ItemShopItem.GetSprite(ItemShopItem.ItemType.shieldPotion), "Shield potion", ItemShopItem.GetCost(ItemShopItem.ItemType.shieldPotion), 2);

        CreateItemButton(ItemShopItem.ItemType.goldPotion, ItemShopItem.GetSprite(ItemShopItem.ItemType.goldPotion), "Gold potion", ItemShopItem.GetCost(ItemShopItem.ItemType.goldPotion), 3);

        CreateItemButton(ItemShopItem.ItemType.teleportPotion, ItemShopItem.GetSprite(ItemShopItem.ItemType.teleportPotion), "Teleport potion", ItemShopItem.GetCost(ItemShopItem.ItemType.teleportPotion), 4);

        CreateItemButton(ItemShopItem.ItemType.timePotion, ItemShopItem.GetSprite(ItemShopItem.ItemType.timePotion), "Time potion", ItemShopItem.GetCost(ItemShopItem.ItemType.timePotion), 5);


    }

    private void CreateItemButton(ItemShopItem.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        shopItemTransform.gameObject.SetActive(true);

        float shopItemHeight = 120f;
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

    private void TryBuyItem(ItemShopItem.ItemType itemType)
    {
        if (shopCustomer.TrySpendGoldAmount(ItemShopItem.GetCost(itemType)))
        {
            for (int i = 0; i < templates.Count; i++)
            {
                switch (templates[i].Find("itemName").GetComponent<TextMeshProUGUI>().text)
                {
                    case "Health potion":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.smallHealthPotion).ToString());
                        break;
                    case "Big health potion":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.bigHealthPotion).ToString());
                        break;
                    case "Shield potion":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.shieldPotion).ToString());
                        break;
                    case "Gold potion":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.goldPotion).ToString());
                        break;
                    case "Teleport potion":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.teleportPotion).ToString());
                        break;
                    case "Time potion":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.timePotion).ToString());
                        break;
                }
            }
            lastItemSelected = shopCustomer.BoughtItem(itemType);
            EventSystem.current.SetSelectedGameObject(templates[lastItemSelected].gameObject);
        }
    }

    public void setLastItemSelected(int index)
    {
        lastItemSelected = index;
    }

    public void show(IShopCustomer shopCustomer)
    {
        this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(templates[lastItemSelected].gameObject);
    }

    public void hide()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inShop = false;
        gameObject.SetActive(false);
    }
}
