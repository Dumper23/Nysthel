using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI hpText;

    private Player p;

    private void Start()
    {
        p = FindObjectOfType<Player>();
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        hpText.SetText(health + " / " + health);
    }

    public void setHealth(int health)
    {
        slider.value = health;
        if (health < 0)
        {
            hpText.SetText(0 + " / " + p.maxHealth);
        }
        else
        {
            hpText.SetText(health + " / " + p.maxHealth);
        }
    }
}