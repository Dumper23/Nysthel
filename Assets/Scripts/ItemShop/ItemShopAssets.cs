using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemShopAssets : MonoBehaviour
{
    public static ItemShopAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite HealthPotion;
    public Sprite BigHealthPotion;
    public Sprite ShieldPotion;
    public Sprite GoldPotion;
    public Sprite TeleportPotion;
    public Sprite TimePotion;
    public Sprite DoubleAxe;
    public Sprite BloodyAxe;
    public Sprite SeekAxe;
    public Sprite battleAxe;
    public Sprite nysthelAxe;
    public Sprite trueAxe;
}
