using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SaveGame()
    {
        //Player upgrades and gold
        PlayerPrefs.SetInt("gold", SaveVariables.PLAYER_GOLD);
        PlayerPrefs.SetInt("attack", SaveVariables.PLAYER_ATTACK);
        PlayerPrefs.SetInt("life", SaveVariables.PLAYER_LIFE);
        PlayerPrefs.SetFloat("speed", SaveVariables.PLAYER_SPEED);
        PlayerPrefs.SetFloat("attackSpeed", SaveVariables.PLAYER_ATTACK_SPEED);
        PlayerPrefs.SetFloat("range", SaveVariables.PLAYER_RANGE);
        PlayerPrefs.SetFloat("dashRecovery", SaveVariables.PLAYER_DASH_RECOVERY);
        PlayerPrefs.SetFloat("dashRange", SaveVariables.PLAYER_DASH_RANGE);

        //World Stats
        PlayerPrefs.SetInt("maxWorld", SaveVariables.MAX_WORLD);
        PlayerPrefs.SetInt("currentWorld", SaveVariables.CURRENT_WORLD);

        //Blacksmith level
        PlayerPrefs.SetInt("blacksmith", SaveVariables.BLACKSMITH_LEVEL);

        //Upgrade levels
        PlayerPrefs.SetInt("attackLevel", SaveVariables.ATTACK_LEVEL);
        PlayerPrefs.SetInt("lifeLevel", SaveVariables.LIFE_LEVEL);
        PlayerPrefs.SetInt("speedLevel", SaveVariables.SPEED_LEVEL);
        PlayerPrefs.SetInt("attackSpeedLevel", SaveVariables.ATTACK_SPEED_LEVEL);
        PlayerPrefs.SetInt("rangeLevel", SaveVariables.RANGE_LEVEL);
        PlayerPrefs.SetInt("dashRecoveryLevel", SaveVariables.DASH_RECOVERY_LEVEL);
        PlayerPrefs.SetInt("dashRangeLevel", SaveVariables.DASH_RANGE_LEVEL);
    }

    public void loadGame()
    {
        try
        {
            //Player upgrades and gold
            SaveVariables.PLAYER_GOLD = PlayerPrefs.GetInt("gold");
            SaveVariables.PLAYER_ATTACK = PlayerPrefs.GetInt("attack");
            SaveVariables.PLAYER_LIFE = PlayerPrefs.GetInt("life");
            SaveVariables.PLAYER_SPEED = PlayerPrefs.GetFloat("speed");
            SaveVariables.PLAYER_ATTACK_SPEED = PlayerPrefs.GetFloat("attackSpeed");
            SaveVariables.PLAYER_RANGE = PlayerPrefs.GetFloat("range");
            SaveVariables.PLAYER_DASH_RECOVERY = PlayerPrefs.GetFloat("dashRecovery");
            SaveVariables.PLAYER_DASH_RANGE = PlayerPrefs.GetFloat("dashRange");

            //World Stats
            SaveVariables.MAX_WORLD = PlayerPrefs.GetInt("maxWorld");
            SaveVariables.CURRENT_WORLD = PlayerPrefs.GetInt("currentWorld");

            //Blacksmith level
            SaveVariables.BLACKSMITH_LEVEL = PlayerPrefs.GetInt("blacksmith");

            //Upgrade levels
            SaveVariables.ATTACK_LEVEL = PlayerPrefs.GetInt("attackLevel");
            SaveVariables.LIFE_LEVEL = PlayerPrefs.GetInt("lifeLevel");
            SaveVariables.SPEED_LEVEL = PlayerPrefs.GetInt("speedLevel");
            SaveVariables.ATTACK_SPEED_LEVEL = PlayerPrefs.GetInt("attackSpeedLevel");
            SaveVariables.RANGE_LEVEL = PlayerPrefs.GetInt("rangeLevel");
            SaveVariables.DASH_RECOVERY_LEVEL = PlayerPrefs.GetInt("dashRecoveryLevel");
            SaveVariables.DASH_RANGE_LEVEL = PlayerPrefs.GetInt("dashRangeLevel");
        }catch(PlayerPrefsException e)
        {
            Debug.LogError(e);
        }
    }
}
