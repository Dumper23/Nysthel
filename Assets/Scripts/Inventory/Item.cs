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
        shieldPotion,
        multiAxe,
        doubleAxe,
        basicAxe,
        bloodAxe,
        goldPotion,
        teleportPotion,
        timePotion
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
            case ItemType.shieldPotion:   return ItemAssets.Instance.ShieldPotion;
            case ItemType.multiAxe:   return ItemAssets.Instance.MultiAxe;
            case ItemType.doubleAxe:   return ItemAssets.Instance.DoubleAxe;
            case ItemType.basicAxe:   return ItemAssets.Instance.BasicAxe;
            case ItemType.bloodAxe:   return ItemAssets.Instance.BloodAxe;
            case ItemType.goldPotion:   return ItemAssets.Instance.GoldPotion;
            case ItemType.teleportPotion:   return ItemAssets.Instance.TeleportPotion;
            case ItemType.timePotion:   return ItemAssets.Instance.TimePotion;
        }
    }

    public bool isStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.smallPotion:
            case ItemType.shieldPotion:
            case ItemType.bigPotion:
            case ItemType.goldPotion:
            case ItemType.teleportPotion:
            case ItemType.timePotion:
                return true;
            case ItemType.multiAxe:
            case ItemType.doubleAxe:
            case ItemType.basicAxe:
            case ItemType.bloodAxe:
                return false;
        }
    }
}
