using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChest : MonoBehaviour
{
    public bool isWater = false;
    public bool isAcid = false;
    public bool isBoost = false;
    public bool isScare = false;
    public bool isTeleport = false;

    private void Update()
    {
        if (isWater)
        {
            if (SaveVariables.WATER_SKILL <= 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (isAcid)
        {
            if (SaveVariables.ACID_SKILL <= 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (isBoost)
        {
            if (SaveVariables.BOOST_SKILL <= 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (isScare)
        {
            if (SaveVariables.SCARE_SKILL <= 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (isTeleport)
        {
            if (SaveVariables.TELEPORT_SKILL <= 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}