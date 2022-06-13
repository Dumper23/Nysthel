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
        seekAxe,
        battleAxe,
        nysthelAxe,
        trueAxe,
        shield
    }

    //Falta Balancejar preus
    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.smallHealthPotion: return 40;
            case ItemType.bigHealthPotion: return 90;
            case ItemType.shieldPotion: return 30;
            case ItemType.goldPotion: return 110;
            case ItemType.teleportPotion: return 200;
            case ItemType.timePotion: return 130;
            case ItemType.doubleAxe: return 1000;
            case ItemType.bloodAxe: return 1500;
            case ItemType.seekAxe: return 2000;
            case ItemType.battleAxe: return 3000;
            case ItemType.nysthelAxe: return 2500;
            case ItemType.trueAxe: return 3000;
            case ItemType.shield: return 250;
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
            case ItemType.battleAxe: return ItemShopAssets.Instance.battleAxe;
            case ItemType.nysthelAxe: return ItemShopAssets.Instance.nysthelAxe;
            case ItemType.trueAxe: return ItemShopAssets.Instance.trueAxe;
            case ItemType.shield: return ItemShopAssets.Instance.shield;
        }
    }
}
