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
        shield,
        electricOrb,
        fireOrb,
        earthOrb,
        iceOrb
    }

    //Falta Balancejar preus
    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.smallHealthPotion: return 35;
            case ItemType.bigHealthPotion: return 65;
            case ItemType.shieldPotion: return 30;
            case ItemType.goldPotion: return 110;
            case ItemType.teleportPotion: return 200;
            case ItemType.timePotion: return 80;
            case ItemType.doubleAxe: return 550;
            case ItemType.bloodAxe: return 1600;
            case ItemType.seekAxe: return 2000;
            case ItemType.battleAxe: return 2600;
            case ItemType.nysthelAxe: return 3000;
            case ItemType.trueAxe: return 3200;
            case ItemType.shield: return 75;
            case ItemType.electricOrb: return 1750;
            case ItemType.fireOrb: return 1400;
            case ItemType.earthOrb: return 1600;
            case ItemType.iceOrb: return 450;
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
            case ItemType.electricOrb: return ItemShopAssets.Instance.electricOrb;
            case ItemType.fireOrb: return ItemShopAssets.Instance.fireOrb;
            case ItemType.earthOrb: return ItemShopAssets.Instance.earthOrb;
            case ItemType.iceOrb: return ItemShopAssets.Instance.iceOrb;
        }
    }
}