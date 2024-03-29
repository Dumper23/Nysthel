using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform container;
    private Transform itemTemplate;
    private Transform statistics;
    private Transform weaponEquiped;
    private Transform itemSpecs;
    private Player player;
    private Item currentItem;

    

    private int lastITemSelected = 0;

    private void Awake()
    {
        container = transform.Find("Container");
        itemTemplate = container.Find("ItemTemplate");
        statistics = transform.Find("Statistics");
        weaponEquiped = transform.Find("WE");
        itemSpecs = transform.Find("itemSpecs");
        container.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        TextMeshProUGUI t = statistics.Find("stats").GetComponent<TextMeshProUGUI>();
        t.text = "Nysthel Statistics:\n"
               + "(" + SaveVariables.LIFE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) + ") Life: \t\t\t\t" + player.maxHealth + "\n"
               + "(" + SaveVariables.ATTACK_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) + ") Attack: \t\t\t" + player.damage + "\n"
               + "(" + SaveVariables.SPEED_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) + ") Speed: \t\t\t" + player.moveSpeed.ToString("F2") + "\n"
               + "(" + SaveVariables.ATTACK_SPEED_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) + ") Attack Speed: \t\t" + (1/player.attackRate).ToString("F2") + "\n"
               + "(" + SaveVariables.RANGE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) + ") Magnet Range: \t\t" + player.coinMagnetRange.ToString("F2") + "\n"
               + "(" + SaveVariables.DASH_RECOVERY_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) + ") Dash Recovery: \t\t" + player.dashRestoreTime.ToString("F2") + "\n"
               + "(" + SaveVariables.DASH_RANGE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) + ") Dash Range: \t\t" + player.dashForce.ToString("F2");
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }

    public void setInventory(Inventory inv)
    {
        inventory = inv;

        inventory.OnItemListChange += Inventory_OnItemListChanged;
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        refreshInventory();
    }

    public void refreshInventory()
    {
        foreach (Transform child in container)
        {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float cellSize = 80f;
        int i = 0;
        foreach(Item item in inventory.getItemList())
        {
            RectTransform itemTemplateRect = Instantiate(itemTemplate, container).GetComponent<RectTransform>();
            GameObject a = new GameObject();

            GameObject temp = Instantiate(a, itemTemplateRect) as GameObject;
            temp.name = item.itemType.ToString();
            itemTemplateRect.gameObject.SetActive(true);
            itemTemplateRect.GetComponent<Button>().onClick.AddListener(delegate { Clicked(item); });

            

            itemTemplateRect.anchoredPosition = new Vector2(x * cellSize, -y * cellSize);
            Image image = itemTemplateRect.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI uiText = itemTemplateRect.Find("AmountText").GetComponent<TextMeshProUGUI>();
            
            if (item.amount > 1){
                if (item.isStackable())
                {
                    uiText.SetText(item.amount.ToString());
                }
                else
                {
                    uiText.SetText("");
                }
            }
            else
            {
                uiText.SetText("");
            }

            if (i == lastITemSelected)
            {
                EventSystem.current.SetSelectedGameObject(itemTemplateRect.gameObject);
            }

            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
            i++;
        }
    }

    private void Update()
    {
        foreach (Item item in inventory.getItemList())
        {
            switch (item.itemType)
            {
            case Item.ItemType.basicAxe:
                if (SaveVariables.INV_BASIC_AXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                break;
            case Item.ItemType.bloodAxe:
                if (SaveVariables.INV_BLOOD_AXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                    
                break;
            case Item.ItemType.multiAxe:
                if (SaveVariables.INV_MULTIAXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                break;
            case Item.ItemType.doubleAxe:
                if (SaveVariables.INV_DOUBLE_AXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                break;
            case Item.ItemType.seekAxe:
                if (SaveVariables.INV_SEEK_AXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                break;
            }
        }

        if (EventSystem.current.currentSelectedGameObject != null) {
            if (EventSystem.current.currentSelectedGameObject.name == "ItemTemplate(Clone)")
            {
                showItemInfo(EventSystem.current.currentSelectedGameObject.transform.GetChild(4).name);
            }
        }
    }
    private void Clicked(Item item)
    {
        int i = 0;
        foreach(Item itm in inventory.getItemList())
        {
            if (itm.Equals(item))
            {
                lastITemSelected = i;
            }
            i++;
        }
        inventory.UseItem(item);
        inventory.OnItemListChange += Inventory_OnItemListChanged;
    }

    public void navigationRefresh()
    {
        if (container.Find("ItemTemplate(Clone)") != null)
        {
            GameObject temp = container.Find("ItemTemplate(Clone)").gameObject;
            EventSystem.current.SetSelectedGameObject(temp);
        }
    }

    private void showItemInfo(string itemSelected)
    {
        TextMeshProUGUI desc = itemSpecs.Find("description").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI name = itemSpecs.Find("name").GetComponent<TextMeshProUGUI>();
        switch (itemSelected)
        {
            case "smallPotion":
                desc.text = "+10 hp";
                name.text = "Small potion";
                break;
            case "bigPotion":
                desc.text = "+20 hp";
                name.text = "Big potion";
                break;
            case "shieldPotion":
                desc.text = "Immune for 6s, but disables dash during its duration";
                name.text = "Shield potion";
                break;
            case "goldPotion":
                desc.text = "x2 gold during 30s";
                name.text = "Gold potion";
                break;
            case "timePotion":
                desc.text = "Slows time but not for Nysthel";
                name.text = "Time potion";
                break;
            case "teleportPotion":
                desc.text = "Teleports Nysthel to the starting room";
                name.text = "Teleport potion";
                break;
            case "basicAxe":
                desc.text = "DMG = x1\nSPD = 5";
                name.text = "Emmyr's Axe";
                break;
            case "bloodAxe":
                desc.text = "DMG = +6\nSPD = 2";
                name.text = "Bloody Axe";
                break;
            case "doubleAxe":
                desc.text = "DMG = x1\nSPD = 3";
                name.text = "Double Axe";
                break;
            case "multiAxe":
                desc.text = "DMG = x1\nSPD = 1";
                name.text = "Multi Axe";
                break;
            case "seekAxe":
                desc.text = "DMG = +3\nSPD = 3";
                name.text = "Messenger Axe";
                break;
        }
    }
}
