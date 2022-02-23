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
    private Player player;
    private Item currentItem;

    private int lastITemSelected = 0;

    private void Awake()
    {
        container = transform.Find("Container");
        itemTemplate = container.Find("ItemTemplate");
        statistics = transform.Find("Statistics");
        container.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        TextMeshProUGUI t = statistics.Find("stats").GetComponent<TextMeshProUGUI>();
        t.text = "(" + SaveVariables.LIFE_LEVEL + "/" + ShopItem.GetMaxLevel(ShopItem.ItemType.LifeUpgrade) + ") Life: \t\t\t\t" + player.maxHealth + "\n"
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
            itemTemplateRect.gameObject.SetActive(true);
            itemTemplateRect.GetComponent<Button>().onClick.AddListener(delegate { Clicked(item); });

            if (i == lastITemSelected)
            {
                EventSystem.current.SetSelectedGameObject(itemTemplateRect.gameObject);
            }

            itemTemplateRect.anchoredPosition = new Vector2(x * cellSize, -y * cellSize);
            Image image = itemTemplateRect.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI uiText = itemTemplateRect.Find("AmountText").GetComponent<TextMeshProUGUI>();

            if (item.amount > 1){
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
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
    }

    public void navigationRefresh()
    {
        if (container.Find("ItemTemplate(Clone)") != null)
        {
            GameObject temp = container.Find("ItemTemplate(Clone)").gameObject;
            EventSystem.current.SetSelectedGameObject(temp);
        }
    }

    public static void updateText(Item item)
    {

    }
}
