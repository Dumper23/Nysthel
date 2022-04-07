using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopItem
{
    public enum ItemType
    {
        smallHealthPotion,
        bigHealthPotion,
        shieldPotion,
        goldPotion,
        teleportPotion,
        timePotion,
        doubleAxe,
        bloodAxe,
        seekAxe
    }

    //Falta Balancejar preus
    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.smallHealthPotion: return 10;
            case ItemType.bigHealthPotion: return 10;
            case ItemType.shieldPotion: return 10;
            case ItemType.goldPotion: return 10;
            case ItemType.teleportPotion: return 10;
            case ItemType.timePotion: return 10;
            case ItemType.doubleAxe: return 10;
            case ItemType.bloodAxe: return 10;
            case ItemType.seekAxe: return 10;
        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.smallHealthPotion: return ItemShopAssets.Instance.HealthPotion;
            case ItemType.bigHealthPotion: return ItemShopAssets.Instance.BigHealthPotion;
            case ItemType.shieldPotion: return ItemShopAssets.Instance.ShieldPotion;
            case ItemType.goldPotion: return ItemShopAssets.Instance.GoldPotion;
            case ItemType.teleportPotion: return ItemShopAssets.Instance.TeleportPotion;
            case ItemType.timePotion: return ItemShopAssets.Instance.TimePotion;
            case ItemType.doubleAxe: return ItemShopAssets.Instance.DoubleAxe;
            case ItemType.bloodAxe: return ItemShopAssets.Instance.BloodyAxe;
            case ItemType.seekAxe: return ItemShopAssets.Instance.SeekAxe;
        }
    }
}
