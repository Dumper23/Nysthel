using UnityEngine;

public static class SaveVariables
{
    public static int PLAYER_GOLD = 0;
    public static int PLAYER_WOOD = 0;
    public static int PLAYER_ATTACK = 0;
    public static float PLAYER_SPEED = 0;
    public static float PLAYER_ATTACK_SPEED = 0;
    public static float PLAYER_RANGE = 0;
    public static float PLAYER_DASH_RECOVERY = 0;
    public static float PLAYER_DASH_RANGE = 0;
    public static int PLAYER_LIFE = 0;

    public static int GOLD_STATUE = 0;
    public static int HOLY_STATUE = 0;
    public static int CHANCE_STATUE = 0;
    public static int EMMYR_STATUE = 0;
    public static int DAMAGE_STATUE = 0;
    public static int ACTIVATED_STATUES = 0;

    public static int PLAYER_USING_CONTROLLER = 0; //Default controller is mouse

    public static int TALKED_VORDKOR = 0;
    public static int TALKED_HALLBORG = 0;
    public static int TALKED_GROMODIN = 0;
    public static int HAS_EMMYR_ITEM = 0;

    public static int MAX_WORLD = 0;
    public static int CURRENT_WORLD = 0;

    public static int 
        BLACKSMITH_LEVEL = 0,
        LIFE_LEVEL = 0,
        ATTACK_LEVEL = 0,
        SPEED_LEVEL = 0,
        ATTACK_SPEED_LEVEL = 0,
        RANGE_LEVEL = 0,
        DASH_RECOVERY_LEVEL = 0,
        DASH_RANGE_LEVEL = 0;

    public static int
        INV_SMALL_POTION = 0,
        INV_BIG_POTION = 0,
        INV_SHIELD_POTION = 0,
        INV_GOLD_POTION = 0,
        INV_TELEPORT_POTION = 0,
        INV_TIME_POTION = 0,
        INV_MULTIAXE = 0,
        INV_BASIC_AXE = 1, //Per defecte sempre tindras l'arma base
        INV_DOUBLE_AXE = 0;

    public static void clearVariables()
    {
        PLAYER_GOLD = 0;
        PLAYER_WOOD = 0;
        PLAYER_ATTACK = 0;
        PLAYER_SPEED = 0;
        PLAYER_ATTACK_SPEED = 0;
        PLAYER_RANGE = 0;
        PLAYER_DASH_RECOVERY = 0;
        PLAYER_DASH_RANGE = 0;
        PLAYER_LIFE = 0;

        GOLD_STATUE = 0;
        HOLY_STATUE = 0;
        CHANCE_STATUE = 0;
        EMMYR_STATUE = 0;
        DAMAGE_STATUE = 0;
        ACTIVATED_STATUES = 0;

        TALKED_VORDKOR = 0;
        TALKED_HALLBORG = 0;
        TALKED_GROMODIN = 0;
        HAS_EMMYR_ITEM = 0;

        MAX_WORLD = 0;
        CURRENT_WORLD = 0;

        BLACKSMITH_LEVEL = 0;
        LIFE_LEVEL = 0;
        ATTACK_LEVEL = 0;
        SPEED_LEVEL = 0;
        ATTACK_SPEED_LEVEL = 0;
        RANGE_LEVEL = 0;
        DASH_RECOVERY_LEVEL = 0;
        DASH_RANGE_LEVEL = 0;


        INV_SMALL_POTION = 0;
        INV_BIG_POTION = 0;
        INV_SHIELD_POTION = 0;
        INV_GOLD_POTION = 0;
        INV_TELEPORT_POTION = 0;
        INV_TIME_POTION = 0;
        INV_MULTIAXE = 0;
        INV_BASIC_AXE = 1;
        INV_DOUBLE_AXE = 0;
    }

    public static void clearInventory()
    {
        INV_SMALL_POTION = 0;
        INV_BIG_POTION = 0;
        INV_SHIELD_POTION = 0;
        INV_GOLD_POTION = 0;
        INV_TELEPORT_POTION = 0;
        INV_TIME_POTION = 0;
        INV_MULTIAXE = 0;
        INV_BASIC_AXE = 1;
        INV_DOUBLE_AXE = 0;
    }


    public static string getCurrentWorld()
    {
        switch (CURRENT_WORLD)
        {
            case 0:
                return "Forest";
            default:
                return "Village";
        }
    }
}
