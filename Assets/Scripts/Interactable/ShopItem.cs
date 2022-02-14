using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    public enum ItemType
    {
        LifeUpgrade,
        AttackUpgrade,
        SpeedUpgrade,
        AttackSpeedUpgrade,
        RangeUpgrade,
        DashRecoveryUpgrade,
        DashRangeUpgrade
    }

    private static int LifeUpgradeLevel = 0,
        AttackUpgradeLevel = 0,
        SpeedUpgradeLevel = 0,
        AttackSpeedUpgradeLevel = 0,
        RangeUpgradeLevel = 0,
        DashRecoveryUpgradeLevel = 0,
        DashRangeUpgradeLevel = 0;

    //Falta Balancejar preus
    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            //Cost function: Mathf.RoundToInt(Mathf.Pow(ShopAssets.Instance.BlackSmithLevel + 1, 2)) * 25
            default:
            case ItemType.LifeUpgrade: return 300 * (ShopAssets.Instance.BlackSmithLevel + 1);
            case ItemType.AttackUpgrade: return 300 * (ShopAssets.Instance.BlackSmithLevel + 1);
            case ItemType.SpeedUpgrade: return 150 * (ShopAssets.Instance.BlackSmithLevel + 1);
            case ItemType.AttackSpeedUpgrade: return 250 * (ShopAssets.Instance.BlackSmithLevel + 1);
            case ItemType.RangeUpgrade: return 150 * (ShopAssets.Instance.BlackSmithLevel + 1);
            case ItemType.DashRecoveryUpgrade: return 200 * (ShopAssets.Instance.BlackSmithLevel + 1);
            case ItemType.DashRangeUpgrade: return 150 * (ShopAssets.Instance.BlackSmithLevel + 1);
        }
    }

    //Falta Balancejar Nivell maxim
    public static int GetMaxLevel(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.LifeUpgrade: return 10;
            case ItemType.AttackUpgrade: return 5;
            case ItemType.SpeedUpgrade: return 2;
            case ItemType.AttackSpeedUpgrade: return 5;
            case ItemType.RangeUpgrade: return 5;
            case ItemType.DashRecoveryUpgrade: return 3;
            case ItemType.DashRangeUpgrade: return 5;
        }
    }

    public static int GetCurrentLevel(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.LifeUpgrade: return LifeUpgradeLevel;
            case ItemType.AttackUpgrade: return AttackUpgradeLevel;
            case ItemType.SpeedUpgrade: return SpeedUpgradeLevel;
            case ItemType.AttackSpeedUpgrade: return AttackSpeedUpgradeLevel;
            case ItemType.RangeUpgrade: return RangeUpgradeLevel;
            case ItemType.DashRecoveryUpgrade: return DashRecoveryUpgradeLevel;
            case ItemType.DashRangeUpgrade: return DashRangeUpgradeLevel;
        }
    }

    public static void SetCurrentLevel(ItemType itemType, int level)
    {
        switch (itemType)
        {
            case ShopItem.ItemType.LifeUpgrade:
                LifeUpgradeLevel = level;
                break;

            case ShopItem.ItemType.AttackUpgrade:
                AttackUpgradeLevel = level;
                break;

            case ShopItem.ItemType.SpeedUpgrade:
                SpeedUpgradeLevel = level;
                break;

            case ShopItem.ItemType.AttackSpeedUpgrade:
                AttackSpeedUpgradeLevel = level;
                break;

            case ShopItem.ItemType.RangeUpgrade:
                RangeUpgradeLevel = level;
                break;

            case ShopItem.ItemType.DashRecoveryUpgrade:
                DashRecoveryUpgradeLevel = level;
                break;

            case ShopItem.ItemType.DashRangeUpgrade:
                DashRangeUpgradeLevel = level;
                break;

        }
    }

    public static void AddLevel(ItemType itemType)
    {
        switch (itemType)
        {
            case ShopItem.ItemType.LifeUpgrade:
                LifeUpgradeLevel++;
                break;

            case ShopItem.ItemType.AttackUpgrade:
                AttackUpgradeLevel++;
                break;

            case ShopItem.ItemType.SpeedUpgrade:
                SpeedUpgradeLevel++;
                break;

            case ShopItem.ItemType.AttackSpeedUpgrade:
                AttackSpeedUpgradeLevel++;
                break;

            case ShopItem.ItemType.RangeUpgrade:
                RangeUpgradeLevel++;
                break;

            case ShopItem.ItemType.DashRecoveryUpgrade:
                DashRecoveryUpgradeLevel++;
                break;

            case ShopItem.ItemType.DashRangeUpgrade:
                DashRangeUpgradeLevel++;
                break;

        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.LifeUpgrade: return ShopAssets.Instance.LifeUpgrade;
            case ItemType.AttackUpgrade: return ShopAssets.Instance.AttackUpgrade;
            case ItemType.SpeedUpgrade: return ShopAssets.Instance.SpeedUpgrade;
            case ItemType.AttackSpeedUpgrade: return ShopAssets.Instance.AttackSpeedUpgrade;
            case ItemType.RangeUpgrade: return ShopAssets.Instance.RangeUpgrade;
            case ItemType.DashRecoveryUpgrade: return ShopAssets.Instance.DashRecoveryUpgrade;
            case ItemType.DashRangeUpgrade: return ShopAssets.Instance.DashRangeUpgrade;
        }
    }
}
