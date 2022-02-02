using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform container;
    private Transform itemTemplate;
    private Player player;

    private void Awake()
    {
        container = transform.Find("Container");
        itemTemplate = container.Find("ItemTemplate");
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }

    public void setInventory(Inventory inv)
    {
        inventory = inv;

        inventory.OnItemListChange += Inventory_OnItemListChanged;
        refreshInventory();
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
        foreach(Item item in inventory.getItemList())
        {
            RectTransform itemTemplateRect = Instantiate(itemTemplate, container).GetComponent<RectTransform>();
            itemTemplateRect.gameObject.SetActive(true);


            itemTemplateRect.GetComponent<Button_UI>().ClickFunc = () =>
            {
                //Use Item

            };

            itemTemplateRect.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                //Drop Item
                inventory.RemoveItem(item);
                ItemWorld.DropItem(player.transform.position, item);
            };


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
        }
    }
}
