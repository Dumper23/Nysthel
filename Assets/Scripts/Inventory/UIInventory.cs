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
    public bool isPointerIn = false;
    public Image skillImage;

    private Inventory inventory;
    private Transform container;
    private Transform itemTemplate;
    private Transform statistics;
    private Transform weaponEquiped;
    private Transform orbEquiped;
    private Transform skillEquiped;
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
        orbEquiped = transform.Find("OE");
        skillEquiped = transform.Find("SE");
        itemSpecs = transform.Find("itemSpecs");

        container.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        defenseText.SetText(SaveVariables.PLAYER_DEFENSE.ToString());
        TextMeshProUGUI t = statistics.Find("stats").GetComponent<TextMeshProUGUI>();
        t.text = "(" + SaveVariables.LIFE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) + ") Life:............................" + player.maxHealth + "\n"
               + "(" + SaveVariables.ATTACK_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackUpgrade) + ") Attack:..........................." + player.damage + "\n"
               + "(" + SaveVariables.SPEED_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.SpeedUpgrade) + ") Speed:............................" + player.moveSpeed.ToString("F2") + "\n"
               + "(" + SaveVariables.ATTACK_SPEED_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.AttackSpeedUpgrade) + ") Attack Speed:....................." + (1 / player.attackRate).ToString("F2") + "\n"
               + "(" + SaveVariables.RANGE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.RangeUpgrade) + ") Magnet Range:....................." + player.coinMagnetRange.ToString("F2") + "\n"
               + "(" + SaveVariables.DASH_RECOVERY_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRecoveryUpgrade) + ") Dash Recovery:...................." + player.dashRestoreTime.ToString("F2") + "\n"
               + "(" + SaveVariables.DASH_RANGE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.DashRangeUpgrade) + ") Dash Range:......................." + player.dashForce.ToString("F2") + "\n"
               + "(" + SaveVariables.PLAYER_DEFENSE + "/50) " + "Defense: " + SaveVariables.PLAYER_DEFENSE + "% of damage blocked";
        refreshInventory();
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
        foreach (Item item in inventory.getItemList())
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
            switch (item.itemType)
            {
                case Item.ItemType.waterPuddle:
                    if (SaveVariables.WATER_SKILL == 0)
                    {
                        image.color = Color.black;
                    }
                    else
                    {
                        image.color = Color.white;
                    }
                    break;

                case Item.ItemType.acidPuddle:
                    if (SaveVariables.ACID_SKILL == 0)
                    {
                        image.color = Color.black;
                    }
                    else
                    {
                        image.color = Color.white;
                    }
                    break;

                case Item.ItemType.golem:
                    if (SaveVariables.GOLEM_SKILL == 0)
                    {
                        image.color = Color.black;
                    }
                    else
                    {
                        image.color = Color.white;
                    }
                    break;

                case Item.ItemType.boost:
                    if (SaveVariables.BOOST_SKILL == 0)
                    {
                        image.color = Color.black;
                    }
                    else
                    {
                        image.color = Color.white;
                    }
                    break;

                case Item.ItemType.scare:
                    if (SaveVariables.SCARE_SKILL == 0)
                    {
                        image.color = Color.black;
                    }
                    else
                    {
                        image.color = Color.white;
                    }
                    break;

                case Item.ItemType.teleportClone:
                    if (SaveVariables.TELEPORT_SKILL == 0)
                    {
                        image.color = Color.black;
                    }
                    else
                    {
                        image.color = Color.white;
                    }
                    break;
            }
            TextMeshProUGUI uiText = itemTemplateRect.Find("AmountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
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
        if (SaveVariables.ELECTRIC_ORB < 2 && SaveVariables.EARTH_ORB < 2 && SaveVariables.FIRE_ORB < 2 && SaveVariables.ICE_ORB < 2)
        {
            orbEquiped.gameObject.SetActive(false);
        }
        else
        {
            orbEquiped.gameObject.SetActive(true);
        }
        if (SaveVariables.WATER_SKILL < 2 && SaveVariables.ACID_SKILL < 2 && SaveVariables.GOLEM_SKILL < 2 && SaveVariables.BOOST_SKILL < 2 && SaveVariables.SCARE_SKILL < 2 && SaveVariables.TELEPORT_SKILL < 2)
        {
            skillEquiped.gameObject.SetActive(false);
            skillImage.gameObject.SetActive(false);
        }
        else
        {
            skillEquiped.gameObject.SetActive(true);
            skillImage.gameObject.SetActive(true);
        }

        checkImages();

        if (!isPointerIn && EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "ItemTemplate(Clone)")
            {
                showItemInfo(EventSystem.current.currentSelectedGameObject.transform.GetChild(5).name);
            }
        }
    }

    public void checkFirstTime()
    {
        if (SaveVariables.WATER_SKILL < 2 && SaveVariables.ACID_SKILL < 2 && SaveVariables.GOLEM_SKILL < 2 && SaveVariables.BOOST_SKILL < 2 && SaveVariables.SCARE_SKILL < 2 && SaveVariables.TELEPORT_SKILL < 2)
        {
            skillEquiped.gameObject.SetActive(false);
            skillImage.gameObject.SetActive(false);
        }
        else
        {
            skillEquiped.gameObject.SetActive(true);
            skillImage.gameObject.SetActive(true);
        }
        if (SaveVariables.WATER_SKILL == 2)
        {
            skillEquiped.GetComponent<Image>().sprite = inventory.GetItem(Item.ItemType.waterPuddle).GetSprite();
            skillImage.sprite = inventory.GetItem(Item.ItemType.waterPuddle).GetSprite();
        }

        if (SaveVariables.ACID_SKILL == 2)
        {
            skillEquiped.GetComponent<Image>().sprite = inventory.GetItem(Item.ItemType.acidPuddle).GetSprite();
            skillImage.sprite = inventory.GetItem(Item.ItemType.acidPuddle).GetSprite();
        }

        if (SaveVariables.GOLEM_SKILL == 2)
        {
            skillEquiped.GetComponent<Image>().sprite = inventory.GetItem(Item.ItemType.golem).GetSprite();
            skillImage.sprite = inventory.GetItem(Item.ItemType.golem).GetSprite();
        }

        if (SaveVariables.BOOST_SKILL == 2)
        {
            skillEquiped.GetComponent<Image>().sprite = inventory.GetItem(Item.ItemType.boost).GetSprite();
            skillImage.sprite = inventory.GetItem(Item.ItemType.boost).GetSprite();
        }

        if (SaveVariables.SCARE_SKILL == 2)
        {
            skillEquiped.GetComponent<Image>().sprite = inventory.GetItem(Item.ItemType.scare).GetSprite();
            skillImage.sprite = inventory.GetItem(Item.ItemType.scare).GetSprite();
        }

        if (SaveVariables.TELEPORT_SKILL == 2)
        {
            skillEquiped.GetComponent<Image>().sprite = inventory.GetItem(Item.ItemType.teleportClone).GetSprite();
            skillImage.sprite = inventory.GetItem(Item.ItemType.teleportClone).GetSprite();
        }
    }

    private void checkImages()
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

                case Item.ItemType.electricOrb:
                    if (SaveVariables.ELECTRIC_ORB == 2)
                    {
                        orbEquiped.GetComponent<Image>().sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.earthOrb:
                    if (SaveVariables.EARTH_ORB == 2)
                    {
                        orbEquiped.GetComponent<Image>().sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.fireOrb:
                    if (SaveVariables.FIRE_ORB == 2)
                    {
                        orbEquiped.GetComponent<Image>().sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.iceOrb:
                    if (SaveVariables.ICE_ORB == 2)
                    {
                        orbEquiped.GetComponent<Image>().sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.waterPuddle:
                    if (SaveVariables.WATER_SKILL == 2)
                    {
                        skillEquiped.GetComponent<Image>().sprite = item.GetSprite();
                        skillImage.sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.acidPuddle:
                    if (SaveVariables.ACID_SKILL == 2)
                    {
                        skillEquiped.GetComponent<Image>().sprite = item.GetSprite();
                        skillImage.sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.golem:
                    if (SaveVariables.GOLEM_SKILL == 2)
                    {
                        skillEquiped.GetComponent<Image>().sprite = item.GetSprite();
                        skillImage.sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.boost:
                    if (SaveVariables.BOOST_SKILL == 2)
                    {
                        skillEquiped.GetComponent<Image>().sprite = item.GetSprite();
                        skillImage.sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.scare:
                    if (SaveVariables.SCARE_SKILL == 2)
                    {
                        skillEquiped.GetComponent<Image>().sprite = item.GetSprite();
                        skillImage.sprite = item.GetSprite();
                    }
                    break;

                case Item.ItemType.teleportClone:
                    if (SaveVariables.TELEPORT_SKILL == 2)
                    {
                        skillEquiped.GetComponent<Image>().sprite = item.GetSprite();
                        skillImage.sprite = item.GetSprite();
                    }
                    break;
            }
        }
    }

    private void Clicked(Item item)
    {
        int i = 0;
        foreach (Item itm in inventory.getItemList())
        {
            if (itm.itemType == item.itemType)
            {
                if (item.isStackable())
                {
                    if (item.amount <= 1)
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
                    else
                    {
                        lastITemSelected = i;
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

    public void showItemInfo(string itemSelected)
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
                infoName.text = "Emmyr's gifted Axe";
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
                infoName.text = "Nysthel Axe";
                break;

            case "trueAxe":
                description.text = "DMG = +50\nSPD = -2";
                infoName.text = "True dwarf Axe";
                break;

            case "electricOrb":
                description.text = "deals 10 DMG to all enemies that are close to each other";
                infoName.text = "Electric Orb";
                break;

            case "fireOrb":
                description.text = "Sets an enemy on fire dealing 10 DMG in total";
                infoName.text = "Fire Orb";
                break;

            case "earthOrb":
                description.text = "Deals half of nysthels current DMG (including axe stats)";
                infoName.text = "Earth Orb";
                break;

            case "iceOrb":
                description.text = "Freezes enemies but deals 0 DMG";
                infoName.text = "Ice Orb";
                break;

            case "waterPuddle":
                if (SaveVariables.WATER_SKILL == 0)
                {
                    description.text = "It is hidden in the village!";
                    infoName.text = "?";
                }
                else
                {
                    description.text = "It soaks the enemies in water\nOnly equippable in the Village!";
                    infoName.text = "Water Puddle";
                }
                break;

            case "acidPuddle":
                if (SaveVariables.ACID_SKILL == 0)
                {
                    description.text = "It can be found in a chest in the Forest!";
                    infoName.text = "?";
                }
                else
                {
                    description.text = "It soaks the enemies in acid dealing DMG to them\nOnly equippable in the Village!";
                    infoName.text = "Acid Puddle";
                }
                break;

            case "golem":
                if (SaveVariables.GOLEM_SKILL == 0)
                {
                    description.text = "It is given as a reward in the Gold Rush for lasting more than 4 minutes!";
                    infoName.text = "?";
                }
                else
                {
                    description.text = "It spawns a golem during some time that will help you kill your enemies!\nOnly equippable in the Village!";
                    infoName.text = "Rock Golem";
                }
                break;

            case "boost":
                if (SaveVariables.BOOST_SKILL == 0)
                {
                    description.text = "It can be found in a chest in the Forest!";
                    infoName.text = "?";
                }
                else
                {
                    description.text = "Makes nysthel enter a rage state, your damage and attack speed will increase\nOnly equippable in the Village!";
                    infoName.text = "Holy blessing";
                }
                break;

            case "scare":
                if (SaveVariables.SCARE_SKILL == 0)
                {
                    description.text = "It can be found in a chest in the Ruins!";
                    infoName.text = "?";
                }
                else
                {
                    description.text = "Nysthel uses dark forces to create a shield that will scare the enemies and prevent her from any damage\nOnly equippable in the Village!";
                    infoName.text = "Dark spell";
                }
                break;

            case "teleportClone":
                if (SaveVariables.TELEPORT_SKILL == 0)
                {
                    description.text = "It can be found in a chest in the Ruins!";
                    infoName.text = "?";
                }
                else
                {
                    description.text = "The dark energy opens a portal, to which Nysthel can return whenever she wants\nOnly equippable in the Village!";
                    infoName.text = "Dark portal";
                }
                break;

            default:
                description.text = itemSelected;
                infoName.text = itemSelected;
                break;
        }
    }
}