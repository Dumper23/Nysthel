using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSkill : MonoBehaviour
{
    public GameObject fire;
    public bool isInFire = false;

    private void Start()
    {
        fire.SetActive(false);
        isInFire = false;
    }

    public void incendiate()
    {
        fire.SetActive(true);
        isInFire = true;
    }
}