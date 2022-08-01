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
        seekAxe,
        goldPotion,
        teleportPotion,
        timePotion,
        battleAxe,
        nysthelAxe,
        trueAxe,
        shield,
        electricOrb,
        fireOrb,
        earthOrb,
        iceOrb,
        waterPuddle,
        acidPuddle,
        golem,
        boost,
        scare,
        teleportClone
    }

    public ItemType itemType;
    public int amount;
    public int positionInventory;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.smallPotion: return ItemAssets.Instance.SmallPotion;
            case ItemType.bigPotion: return ItemAssets.Instance.BigPotion;
            case ItemType.shieldPotion: return ItemAssets.Instance.ShieldPotion;
            case ItemType.multiAxe: return ItemAssets.Instance.MultiAxe;
            case ItemType.doubleAxe: return ItemAssets.Instance.DoubleAxe;
            case ItemType.basicAxe: return ItemAssets.Instance.BasicAxe;
            case ItemType.bloodAxe: return ItemAssets.Instance.BloodAxe;
            case ItemType.seekAxe: return ItemAssets.Instance.SeekAxe;
            case ItemType.goldPotion: return ItemAssets.Instance.GoldPotion;
            case ItemType.teleportPotion: return ItemAssets.Instance.TeleportPotion;
            case ItemType.timePotion: return ItemAssets.Instance.TimePotion;
            case ItemType.battleAxe: return ItemAssets.Instance.battleAxe;
            case ItemType.nysthelAxe: return ItemAssets.Instance.nysthelAxe;
            case ItemType.trueAxe: return ItemAssets.Instance.trueAxe;
            case ItemType.shield: return ItemAssets.Instance.shield;
            case ItemType.electricOrb: return ItemAssets.Instance.electricOrb;
            case ItemType.fireOrb: return ItemAssets.Instance.fireOrb;
            case ItemType.earthOrb: return ItemAssets.Instance.earthOrb;
            case ItemType.iceOrb: return ItemAssets.Instance.iceOrb;
            case ItemType.waterPuddle: return ItemAssets.Instance.waterPuddle;
            case ItemType.acidPuddle: return ItemAssets.Instance.acidPuddle;
            case ItemType.golem: return ItemAssets.Instance.golem;
            case ItemType.boost: return ItemAssets.Instance.boost;
            case ItemType.scare: return ItemAssets.Instance.scare;
            case ItemType.teleportClone: return ItemAssets.Instance.teleportClone;
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
            case ItemType.shield:
                return true;

            case ItemType.multiAxe:
            case ItemType.doubleAxe:
            case ItemType.basicAxe:
            case ItemType.bloodAxe:
            case ItemType.seekAxe:
            case ItemType.battleAxe:
            case ItemType.nysthelAxe:
            case ItemType.trueAxe:
            case ItemType.electricOrb:
            case ItemType.fireOrb:
            case ItemType.earthOrb:
            case ItemType.iceOrb:
            case ItemType.waterPuddle:
            case ItemType.acidPuddle:
            case ItemType.golem:
            case ItemType.boost:
            case ItemType.scare:
            case ItemType.teleportClone:
                return false;
        }
    }
}