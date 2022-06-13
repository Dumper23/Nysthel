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
    public TextMeshProUGUI infoName, description, defenseText;

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
        defenseText.SetText(SaveVariables.PLAYER_DEFENSE.ToString());
        TextMeshProUGUI t = statistics.Find("stats").GetComponent<TextMeshProUGUI>();
        t.text = "Nysthel Statistics:\n"
               + "(" + SaveVariables.LIFE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) + ") Life: \t\t\t" + player.maxHealth + "\n"
               + "(" + SaveVariables.ATTACK_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) + ") Attack: \t\t\t" + player.damage + "\n"
               + "(" + SaveVariables.SPEED_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) + ") Speed: \t\t\t" + player.moveSpeed.ToString("F2") + "\n"
               + "(" + SaveVariables.ATTACK_SPEED_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) + ") Attack Speed: \t\t\t" + (1/player.attackRate).ToString("F2") + "\n"
               + "(" + SaveVariables.RANGE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) + ") Magnet Range: \t\t\t" + player.coinMagnetRange.ToString("F2") + "\n"
               + "(" + SaveVariables.DASH_RECOVERY_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) + ") Dash Recovery: \t\t" + player.dashRestoreTime.ToString("F2") + "\n"
               + "(" + SaveVariables.DASH_RANGE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) + ") Dash Range: \t\t\t" + player.dashForce.ToString("F2") + "\n"
               + "Defense: " + SaveVariables.PLAYER_DEFENSE+"% of damage blocked";
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
            a.name = "TempItem";
            a.transform.parent = itemTemplateRect.transform;

            GameObject temp = Instantiate(a, itemTemplateRect.transform);
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
            case Item.ItemType.battleAxe:
                if (SaveVariables.INV_BATTLE_AXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                break;
            case Item.ItemType.nysthelAxe:
                if (SaveVariables.INV_NYSTHEL_AXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                break;
            case Item.ItemType.trueAxe:
                if (SaveVariables.INV_TRUE_AXE == 2)
                {
                    weaponEquiped.GetComponent<Image>().sprite = item.GetSprite();
                }
                break;
            }
        }

        if (EventSystem.current.currentSelectedGameObject != null) {
            if (EventSystem.current.currentSelectedGameObject.name == "ItemTemplate(Clone)")
            {
                showItemInfo(EventSystem.current.currentSelectedGameObject.transform.GetChild(5).name);
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
                if (item.isStackable() && item.amount <= 1)
                {
                    if (i - 1 >= 0)
                    {
                        lastITemSelected = i - 1;
                    }
                    else
                    {
                        lastITemSelected = i + 1;
                    }
                }
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
        switch (itemSelected)
        {
            case "smallPotion":
                description.text = "+10 hp";
                infoName.text = "Small potion";
                break;
            case "bigPotion":
                description.text = "+20 hp";
                infoName.text = "Big potion";
                break;
            case "shieldPotion":
                description.text = "Immune for 6s, but disables dash during its duration";
                infoName.text = "Shield potion";
                break;
            case "goldPotion":
                description.text = "x2 gold during 30s";
                infoName.text = "Gold potion";
                break;
            case "timePotion":
                description.text = "Slows time but not for Nysthel";
                infoName.text = "Time potion";
                break;
            case "teleportPotion":
                description.text = "Teleports Nysthel to the starting room";
                infoName.text = "Teleport potion";
                break;
            case "basicAxe":
                description.text = "DMG = +0\nSPD = 3";
                infoName.text = "Emmyr's Axe";
                break;
            case "bloodAxe":
                description.text = "DMG = +15\nSPD = 2";
                infoName.text = "Bloody Axe";
                break;
            case "doubleAxe":
                description.text = "DMG = +2.5\nSPD = 3";
                infoName.text = "Double Axe";
                break;
            case "multiAxe":
                description.text = "DMG = +0\nSPD = 1";
                infoName.text = "Multi Axe";
                break;
            case "seekAxe":
                description.text = "DMG = +10\nSPD = 3";
                infoName.text = "Messenger Axe";
                break;
            case "battleAxe":
                description.text = "DMG = +5\nSPD = 1";
                infoName.text = "Advanced battle Axe";
                break;
            case "nysthelAxe":
                description.text = "DMG = +20\nSPD = 4";
                infoName.text = "Messenger Axe";
                break;
            case "trueAxe":
                description.text = "DMG = +50\nSPD = -2";
                infoName.text = "True dwarf Axe";
                break;
        }
    }
}
