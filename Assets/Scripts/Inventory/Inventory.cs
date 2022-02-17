using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChange;

    private List<Item> itemList;
    private Action<Item> useItemAction;

    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
    }

    public void addItem(Item item)
    {
        if (item.isStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach(Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChange?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item) 
    {
        if (item.isStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChange?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> getItemList()
    {
        return itemList;
    }

    public Item GetItem(Item.ItemType type)
    {
        foreach (Item item in itemList)
        {
            if (item.itemType == type)
            {
                return item;
            }
        }
        return null;
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }
}
