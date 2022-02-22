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
            case ItemType.LifeUpgrade: return 300 * (GetCurrentLevel(ItemType.LifeUpgrade) + 1);
            case ItemType.AttackUpgrade: return 300 * (GetCurrentLevel(ItemType.AttackUpgrade) + 1);
            case ItemType.SpeedUpgrade: return 150 * (GetCurrentLevel(ItemType.SpeedUpgrade) + 1);
            case ItemType.AttackSpeedUpgrade: return 250 * (GetCurrentLevel(ItemType.AttackSpeedUpgrade) + 1);
            case ItemType.RangeUpgrade: return 150 * (GetCurrentLevel(ItemType.RangeUpgrade) + 1);
            case ItemType.DashRecoveryUpgrade: return 200 * (GetCurrentLevel(ItemType.DashRecoveryUpgrade) + 1);
            case ItemType.DashRangeUpgrade: return 150 * (GetCurrentLevel(ItemType.DashRangeUpgrade) + 1);
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
        LifeUpgradeLevel = SaveVariables.LIFE_LEVEL;
        AttackUpgradeLevel = SaveVariables.ATTACK_LEVEL;
        SpeedUpgradeLevel = SaveVariables.SPEED_LEVEL;
        AttackSpeedUpgradeLevel = SaveVariables.ATTACK_SPEED_LEVEL;
        RangeUpgradeLevel = SaveVariables.RANGE_LEVEL;
        DashRecoveryUpgradeLevel = SaveVariables.DASH_RECOVERY_LEVEL;
        DashRangeUpgradeLevel = SaveVariables.DASH_RANGE_LEVEL;

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
                SaveVariables.LIFE_LEVEL = LifeUpgradeLevel;
                break;

            case ShopItem.ItemType.AttackUpgrade:
                AttackUpgradeLevel = level;
                SaveVariables.ATTACK_LEVEL = AttackUpgradeLevel;
                break;

            case ShopItem.ItemType.SpeedUpgrade:
                SpeedUpgradeLevel = level;
                SaveVariables.SPEED_LEVEL = SpeedUpgradeLevel;
                break;

            case ShopItem.ItemType.AttackSpeedUpgrade:
                AttackSpeedUpgradeLevel = level;
                SaveVariables.ATTACK_SPEED_LEVEL = AttackSpeedUpgradeLevel;
                break;

            case ShopItem.ItemType.RangeUpgrade:
                RangeUpgradeLevel = level;
                SaveVariables.RANGE_LEVEL = RangeUpgradeLevel;
                break;

            case ShopItem.ItemType.DashRecoveryUpgrade:
                DashRecoveryUpgradeLevel = level;
                SaveVariables.DASH_RECOVERY_LEVEL = DashRecoveryUpgradeLevel;
                break;

            case ShopItem.ItemType.DashRangeUpgrade:
                DashRangeUpgradeLevel = level;
                SaveVariables.DASH_RANGE_LEVEL = DashRangeUpgradeLevel;
                break;

        }
    }

    public static void AddLevel(ItemType itemType)
    {
        switch (itemType)
        {
            case ShopItem.ItemType.LifeUpgrade:
                LifeUpgradeLevel++;
                SaveVariables.LIFE_LEVEL = LifeUpgradeLevel;
                break;

            case ShopItem.ItemType.AttackUpgrade:
                AttackUpgradeLevel++;
                SaveVariables.ATTACK_LEVEL = AttackUpgradeLevel;
                break;

            case ShopItem.ItemType.SpeedUpgrade:
                SpeedUpgradeLevel++;
                SaveVariables.SPEED_LEVEL = SpeedUpgradeLevel;
                break;

            case ShopItem.ItemType.AttackSpeedUpgrade:
                AttackSpeedUpgradeLevel++;
                SaveVariables.ATTACK_SPEED_LEVEL = AttackSpeedUpgradeLevel;
                break;

            case ShopItem.ItemType.RangeUpgrade:
                RangeUpgradeLevel++;
                SaveVariables.RANGE_LEVEL = RangeUpgradeLevel;
                break;

            case ShopItem.ItemType.DashRecoveryUpgrade:
                DashRecoveryUpgradeLevel++;
                SaveVariables.DASH_RECOVERY_LEVEL = DashRecoveryUpgradeLevel;
                break;

            case ShopItem.ItemType.DashRangeUpgrade:
                DashRangeUpgradeLevel++;
                SaveVariables.DASH_RANGE_LEVEL = DashRangeUpgradeLevel;
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
