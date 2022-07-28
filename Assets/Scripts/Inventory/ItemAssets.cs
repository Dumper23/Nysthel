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

    public Sprite BigPotion;
    public Sprite SmallPotion;
    public Sprite ShieldPotion;
    public Sprite GoldPotion;
    public Sprite TimePotion;
    public Sprite TeleportPotion;
    public Sprite MultiAxe;
    public Sprite DoubleAxe;
    public Sprite BasicAxe;
    public Sprite BloodAxe;
    public Sprite SeekAxe;
    public Sprite battleAxe;
    public Sprite nysthelAxe;
    public Sprite trueAxe;
    public Sprite shield;
    public Sprite electricOrb;
    public Sprite fireOrb;
    public Sprite earthOrb;
    public Sprite iceOrb;
}