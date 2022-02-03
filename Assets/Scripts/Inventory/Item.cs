using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
   public enum ItemType
    {
        smallPotion,
        bigPotion,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.smallPotion:    return ItemAssets.Instance.SmallPotion;
            case ItemType.bigPotion:   return ItemAssets.Instance.BigPotion;
        }
    }

    public bool isStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.bigPotion:
            case ItemType.smallPotion:
                return true;
            //case ItemType.Sword:
             //   return false;

        }
    }
}
