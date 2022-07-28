using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int TUTORIAL_DONE = 0;
    public int PLAYER_USING_CONTROLLER = 0; //Default controll is mouse

    public int KILLS_GOLD_RUSH = 0;
    public float SECONDS_GOLD_RUSH = 0;

    public int PLAYER_GOLD = 0;
    public int PLAYER_WOOD = 0;
    public int PLAYER_ATTACK = 0;
    public int PLAYER_DEFENSE = 0;
    public float PLAYER_SPEED = 0;
    public float PLAYER_ATTACK_SPEED = 0;
    public float PLAYER_RANGE = 0;
    public float PLAYER_DASH_RECOVERY = 0;
    public float PLAYER_DASH_RANGE = 0;
    public int PLAYER_LIFE = 0;

    public int GOLD_STATUE = 0;
    public int HOLY_STATUE = 0;
    public int CHANCE_STATUE = 0;
    public int EMMYR_STATUE = 0;
    public int DAMAGE_STATUE = 0;
    public int ACTIVATED_STATUES = 0;

    public int TALKED_VORDKOR = 0;
    public int TALKED_HALLBORG = 0;
    public int TALKED_GROMODIN = 0;
    public int HAS_EMMYR_ITEM = 0;

    public int MAX_WORLD = 0;
    public int CURRENT_WORLD = 0;

    public int
        BLACKSMITH_LEVEL = 0,
        LIFE_LEVEL = 0,
        ATTACK_LEVEL = 0,
        SPEED_LEVEL = 0,
        ATTACK_SPEED_LEVEL = 0,
        RANGE_LEVEL = 0,
        DASH_RECOVERY_LEVEL = 0,
        DASH_RANGE_LEVEL = 0;

    public int
        INV_SMALL_POTION = 0,
        INV_BIG_POTION = 0,
        INV_SHIELD_POTION = 0,
        INV_GOLD_POTION = 0,
        INV_TELEPORT_POTION = 0,
        INV_TIME_POTION = 0,
        INV_MULTIAXE = 0,
        INV_BASIC_AXE = 2, //Per defecte sempre tindras l'arma base
        INV_DOUBLE_AXE = 0,
        INV_BLOOD_AXE = 0,
        INV_SEEK_AXE = 0,
        INV_BATTLE_AXE = 0,
        INV_NYSTHEL_AXE = 0,
        INV_TRUE_AXE = 0;

    public void LoadData()
    {
        TUTORIAL_DONE = SaveVariables.TUTORIAL_DONE;

        PLAYER_USING_CONTROLLER = SaveVariables.PLAYER_USING_CONTROLLER;

        KILLS_GOLD_RUSH = SaveVariables.KILLS_GOLD_RUSH;
        SECONDS_GOLD_RUSH = SaveVariables.SECONDS_GOLD_RUSH;

        //Player upgrades and gold
        PLAYER_GOLD = SaveVariables.PLAYER_GOLD;
        PLAYER_WOOD = SaveVariables.PLAYER_WOOD;
        PLAYER_ATTACK = SaveVariables.PLAYER_ATTACK;
        PLAYER_DEFENSE = SaveVariables.PLAYER_DEFENSE;
        PLAYER_LIFE = SaveVariables.PLAYER_LIFE;
        PLAYER_SPEED = SaveVariables.PLAYER_SPEED;
        PLAYER_ATTACK_SPEED = SaveVariables.PLAYER_ATTACK_SPEED;
        PLAYER_RANGE = SaveVariables.PLAYER_RANGE;
        PLAYER_DASH_RECOVERY = SaveVariables.PLAYER_DASH_RECOVERY;
        PLAYER_DASH_RANGE = SaveVariables.PLAYER_DASH_RANGE;

        GOLD_STATUE = SaveVariables.GOLD_STATUE;
        HOLY_STATUE = SaveVariables.HOLY_STATUE;
        CHANCE_STATUE = SaveVariables.CHANCE_STATUE;
        EMMYR_STATUE = SaveVariables.EMMYR_STATUE;
        DAMAGE_STATUE = SaveVariables.DAMAGE_STATUE;
        ACTIVATED_STATUES = SaveVariables.ACTIVATED_STATUES;

        TALKED_VORDKOR = SaveVariables.TALKED_VORDKOR;
        TALKED_HALLBORG = SaveVariables.TALKED_HALLBORG;
        TALKED_GROMODIN = SaveVariables.TALKED_GROMODIN;
        HAS_EMMYR_ITEM = SaveVariables.HAS_EMMYR_ITEM;

        //World Stats
        MAX_WORLD = SaveVariables.MAX_WORLD;
        CURRENT_WORLD = SaveVariables.CURRENT_WORLD;

        //Blacksmith level
        BLACKSMITH_LEVEL = SaveVariables.BLACKSMITH_LEVEL;

        //Upgrade levels
        ATTACK_LEVEL = SaveVariables.ATTACK_LEVEL;
        LIFE_LEVEL = SaveVariables.LIFE_LEVEL;
        SPEED_LEVEL = SaveVariables.SPEED_LEVEL;
        ATTACK_SPEED_LEVEL = SaveVariables.ATTACK_SPEED_LEVEL;
        RANGE_LEVEL = SaveVariables.RANGE_LEVEL;
        DASH_RECOVERY_LEVEL = SaveVariables.DASH_RECOVERY_LEVEL;
        RANGE_LEVEL = SaveVariables.DASH_RANGE_LEVEL;
        DASH_RANGE_LEVEL = SaveVariables.DASH_RANGE_LEVEL;

        //Inventory
        INV_SMALL_POTION = SaveVariables.INV_SMALL_POTION;
        INV_BIG_POTION = SaveVariables.INV_BIG_POTION;
        INV_SHIELD_POTION = SaveVariables.INV_SHIELD_POTION;
        INV_GOLD_POTION = SaveVariables.INV_GOLD_POTION;
        INV_TELEPORT_POTION = SaveVariables.INV_TELEPORT_POTION;
        INV_TIME_POTION = SaveVariables.INV_TIME_POTION;
        INV_BASIC_AXE = SaveVariables.INV_BASIC_AXE;
        INV_MULTIAXE = SaveVariables.INV_MULTIAXE;
        INV_DOUBLE_AXE = SaveVariables.INV_DOUBLE_AXE;
        INV_BLOOD_AXE = SaveVariables.INV_BLOOD_AXE;
        INV_SEEK_AXE = SaveVariables.INV_SEEK_AXE;
        INV_BATTLE_AXE = SaveVariables.INV_BATTLE_AXE;
        INV_NYSTHEL_AXE = SaveVariables.INV_NYSTHEL_AXE;
        INV_TRUE_AXE = SaveVariables.INV_TRUE_AXE;
    }
}