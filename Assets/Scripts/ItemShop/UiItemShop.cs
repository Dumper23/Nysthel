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
    public Interactable shopInteractable;

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

        CreateItemButton(ItemShopItem.ItemType.doubleAxe, ItemShopItem.GetSprite(ItemShopItem.ItemType.doubleAxe), "Double axe", ItemShopItem.GetCost(ItemShopItem.ItemType.doubleAxe), 6);

        CreateItemButton(ItemShopItem.ItemType.bloodAxe, ItemShopItem.GetSprite(ItemShopItem.ItemType.bloodAxe), "Bloody axe", ItemShopItem.GetCost(ItemShopItem.ItemType.bloodAxe), 7);

        CreateItemButton(ItemShopItem.ItemType.seekAxe, ItemShopItem.GetSprite(ItemShopItem.ItemType.seekAxe), "Seek axe", ItemShopItem.GetCost(ItemShopItem.ItemType.seekAxe), 8);

        CreateItemButton(ItemShopItem.ItemType.battleAxe, ItemShopItem.GetSprite(ItemShopItem.ItemType.battleAxe), "Advanced battle axe", ItemShopItem.GetCost(ItemShopItem.ItemType.battleAxe), 9);

        CreateItemButton(ItemShopItem.ItemType.nysthelAxe, ItemShopItem.GetSprite(ItemShopItem.ItemType.nysthelAxe), "Nysthel axe", ItemShopItem.GetCost(ItemShopItem.ItemType.nysthelAxe), 10);

        CreateItemButton(ItemShopItem.ItemType.trueAxe, ItemShopItem.GetSprite(ItemShopItem.ItemType.trueAxe), "True dwarf axe", ItemShopItem.GetCost(ItemShopItem.ItemType.trueAxe), 11);

        CreateItemButton(ItemShopItem.ItemType.shield, ItemShopItem.GetSprite(ItemShopItem.ItemType.shield), "Shield", ItemShopItem.GetCost(ItemShopItem.ItemType.shield), 12);
    }

    private void CreateItemButton(ItemShopItem.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        shopItemTransform.gameObject.SetActive(true);

        float shopItemHeight = 80f;
        if (positionIndex < 6)
        {
            shopItemRectTransform.anchoredPosition = new Vector2(shopItemTransform.localPosition.x, shopItemTransform.localPosition.y - (shopItemHeight * positionIndex));
        }
        else
        {
            shopItemRectTransform.anchoredPosition = new Vector2(350, shopItemTransform.localPosition.y - (shopItemHeight * (positionIndex - 6)));
        }
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
        bool maxShield = false;
        if (itemType == ItemShopItem.ItemType.shield)
        {
            if (SaveVariables.PLAYER_DEFENSE <= 75)
            {
                maxShield = false;
            }
            else
            {
                maxShield = true;
                for (int i = 0; i < templates.Count; i++)
                {
                    switch (templates[i].Find("itemName").GetComponent<TextMeshProUGUI>().text)
                    {
                        case "Shield":
                            templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText("Max Shield!");
                            break;
                    }
                }
            }
        }
        if (!maxShield && shopCustomer.TrySpendGoldAmount(ItemShopItem.GetCost(itemType)))
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

                    case "Double axe":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.doubleAxe).ToString());
                        break;

                    case "Bloody axe":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.bloodAxe).ToString());
                        break;

                    case "Seek axe":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.seekAxe).ToString());
                        break;

                    case "Advanced battle axe":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.battleAxe).ToString());
                        break;

                    case "Nysthel axe":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.nysthelAxe).ToString());
                        break;

                    case "True dwarf axe":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.trueAxe).ToString());
                        break;

                    case "Shield":
                        templates[i].Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(ItemShopItem.GetCost(ItemShopItem.ItemType.shield).ToString());
                        break;
                }
            }
            if (!maxShield)
            {
                lastItemSelected = shopCustomer.BoughtItem(itemType);
                EventSystem.current.SetSelectedGameObject(templates[lastItemSelected].gameObject);
            }
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

    public void exitShop()
    {
        Invoke("endShop", 0.1f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inShop = false;
        gameObject.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Time.timeScale = 1f;
    }

    private void endShop()
    {
        shopInteractable.setInShop(false);
    }
}