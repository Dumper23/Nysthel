using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class StatueManager : MonoBehaviour
{
    public GameObject holyStatue, goldStatue, chanceStatue, emmyrStatue, damageStatue;
    public Light2D holyStatueL, goldStatueL, chanceStatueL, emmyrStatueL, damageStatueL;

    private void Start()
    {
        holyStatue.SetActive(false);
        goldStatue.SetActive(false);
        chanceStatue.SetActive(false);
        emmyrStatue.SetActive(false);
        damageStatue.SetActive(false);
        holyStatueL.intensity = 0;
        goldStatueL.intensity = 0;
        chanceStatueL.intensity = 0;
        emmyrStatueL.intensity = 0;
        damageStatueL.intensity = 0;
    }

    void Update()
    {
        if (SaveVariables.HOLY_STATUE == 1)
        {
            holyStatue.SetActive(true);
        }
        if (SaveVariables.GOLD_STATUE == 1)
        {
            goldStatue.SetActive(true);
        }
        if (SaveVariables.CHANCE_STATUE == 1)
        {
            chanceStatue.SetActive(true);
        }
        if (SaveVariables.EMMYR_STATUE == 1)
        {
            emmyrStatue.SetActive(true);
        }
        if (SaveVariables.DAMAGE_STATUE == 1)
        {
            damageStatue.SetActive(true);
        }

        //---------------------------------
        if (SaveVariables.ACTIVATED_STATUES <= 3)
        {
            if (SaveVariables.HOLY_STATUE == 2)
            {
                holyStatue.SetActive(true);
                holyStatueL.intensity = 1;
            }
            else
            {
                holyStatueL.intensity = 0;
            }

            if (SaveVariables.GOLD_STATUE == 2)
            {
                goldStatue.SetActive(true);
                goldStatueL.intensity = 1;
            }
            else
            {
                goldStatueL.intensity = 0;
            }

            if (SaveVariables.CHANCE_STATUE == 2)
            {
                chanceStatue.SetActive(true);
                chanceStatueL.intensity = 1;
            }
            else
            {
                chanceStatueL.intensity = 0;
            }

            if (SaveVariables.EMMYR_STATUE == 2)
            {
                emmyrStatue.SetActive(true);
                emmyrStatueL.intensity = 1;
            }
            else
            {
                emmyrStatueL.intensity = 0;
            }

            if (SaveVariables.DAMAGE_STATUE == 2)
            {
                damageStatue.SetActive(true);
                damageStatueL.intensity = 1;
            }
            else
            {
                damageStatueL.intensity = 0;
            }
        }
    }
}
