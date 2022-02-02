using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
   public enum ItemType
    {
        Sword,
        potion,
        coin,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword:    return ItemAssets.Instance.swordSprite;
            case ItemType.potion:   return ItemAssets.Instance.potionSprite;
            case ItemType.coin:     return ItemAssets.Instance.coinSprite;
        }
    }

    public bool isStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.coin:
            case ItemType.potion:
                return true;
            case ItemType.Sword:
                return false;

        }
    }
}
