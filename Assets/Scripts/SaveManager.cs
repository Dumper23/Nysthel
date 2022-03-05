using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string path;
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        path = Application.persistentDataPath + "SaveGame.nsthl";
    }

    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        data.LoadData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void DeleteSaveGame()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void loadGame()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            SaveVariables.PLAYER_GOLD = data.PLAYER_GOLD;
            SaveVariables.PLAYER_WOOD = data.PLAYER_WOOD;
            SaveVariables.PLAYER_ATTACK = data.PLAYER_ATTACK;
            SaveVariables.PLAYER_LIFE = data.PLAYER_LIFE;
            SaveVariables.PLAYER_SPEED = data.PLAYER_SPEED; 
            SaveVariables.PLAYER_ATTACK_SPEED = data.PLAYER_ATTACK_SPEED;
            SaveVariables.PLAYER_RANGE = data.PLAYER_RANGE;
            SaveVariables.PLAYER_DASH_RECOVERY = data.PLAYER_DASH_RECOVERY;
            SaveVariables.PLAYER_DASH_RANGE = data.PLAYER_DASH_RANGE;


            //World Stats
            SaveVariables.MAX_WORLD = data.MAX_WORLD;
            SaveVariables.CURRENT_WORLD = data.CURRENT_WORLD;

            //Blacksmith level
            SaveVariables.BLACKSMITH_LEVEL = data.BLACKSMITH_LEVEL;

            //Upgrade levels
            SaveVariables.ATTACK_LEVEL = data.ATTACK_LEVEL;
            SaveVariables.LIFE_LEVEL = data.LIFE_LEVEL;
            SaveVariables.SPEED_LEVEL = data.SPEED_LEVEL;
            SaveVariables.ATTACK_SPEED_LEVEL = data.ATTACK_SPEED_LEVEL;
            SaveVariables.RANGE_LEVEL = data.RANGE_LEVEL;
            SaveVariables.DASH_RECOVERY_LEVEL = data.DASH_RECOVERY_LEVEL;
            SaveVariables.DASH_RANGE_LEVEL = data.DASH_RANGE_LEVEL;

            //Inventory
            SaveVariables.INV_SMALL_POTION = data.INV_SMALL_POTION;
            SaveVariables.INV_BIG_POTION = data.INV_BIG_POTION;
            SaveVariables.INV_SHIELD_POTION = data.INV_SHIELD_POTION;
            SaveVariables.INV_GOLD_POTION = data.INV_GOLD_POTION;
            SaveVariables.INV_TELEPORT_POTION = data.INV_TELEPORT_POTION;
            SaveVariables.INV_TIME_POTION = data.INV_TIME_POTION;

            SaveVariables.INV_BASIC_AXE = data.INV_BASIC_AXE;
            SaveVariables.INV_MULTIAXE = data.INV_MULTIAXE;
            SaveVariables.INV_DOUBLE_AXE = data.INV_DOUBLE_AXE;
        }
    }
}
