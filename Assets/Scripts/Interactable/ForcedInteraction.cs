using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedInteraction : MonoBehaviour
{
    public enum interaction
    {
        skillTutorial
    }

    private void Start()
    {
        if (SaveVariables.WATER_SKILL != 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (SaveVariables.WATER_SKILL == 0)
            {
                string[] s = new string[3];
                s[0] = "Cute Slime: Hey Nysthel! This chest contains a special object, a Skill. You will find more around the world, you can check you inventory to see where to find the others!";
                s[1] = "Cute Slime: Skills have a determinated duration and a reload time! And you can only equip and unequip skills in the village so plan which one you want to use before going out!";
                s[2] = "Cute Slime: Skills can be used alone, but when combined with modifiers great synergies can be made such as water with ice or fire with acid.";

                FindObjectOfType<DialogSystem>().startDialog(s, null);
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}