using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;

    [Header("Potions")]
    public Sprite BigPotion;

    public Sprite SmallPotion;
    public Sprite ShieldPotion;
    public Sprite GoldPotion;
    public Sprite TimePotion;
    public Sprite TeleportPotion;

    [Header("Weapons")]
    public Sprite MultiAxe;

    public Sprite DoubleAxe;
    public Sprite BasicAxe;
    public Sprite BloodAxe;
    public Sprite SeekAxe;
    public Sprite battleAxe;
    public Sprite nysthelAxe;
    public Sprite trueAxe;

    [Header("Defense")]
    public Sprite shield;

    [Header("Bullet Modifiers")]
    public Sprite electricOrb;

    public Sprite fireOrb;
    public Sprite earthOrb;
    public Sprite iceOrb;

    [Header("Skills")]
    public Sprite waterPuddle;

    public Sprite acidPuddle;
    public Sprite golem;
    public Sprite boost;
    public Sprite scare;
    public Sprite teleportClone;
}