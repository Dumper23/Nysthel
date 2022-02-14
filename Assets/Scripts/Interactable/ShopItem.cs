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
