using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAssets : MonoBehaviour
{
    public TextMeshProUGUI level;
    public static ShopAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        level.text = "Blacksmith level: " + BlackSmithLevel.ToString();
    }

    public int BlackSmithLevel;

    public Sprite LifeUpgrade;
    public Sprite AttackUpgrade;
    public Sprite SpeedUpgrade;
    public Sprite AttackSpeedUpgrade;
    public Sprite RangeUpgrade;
    public Sprite DashRecoveryUpgrade;
    public Sprite DashRangeUpgrade;
}
