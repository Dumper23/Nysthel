using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public Interactions interaction;
    public GameObject interactionDialog;
    public TextMeshProUGUI text;
    public UiShop uiShop;
    public UiItemShop uiItemShop;
    public GameObject marker;
    public GameObject mapSelectionUi;

    [Header("--------Chest Shop----------")]
    public GameObject[] chests;
    public GameObject[] chestSpawnPoint;
    public Collider2D chestDetection;
    public int chestShopPrice = 50;

    private Player player;
    private bool inShop = false;
    private bool playerIn = false;

    public enum Interactions
    {
        OpenChest,
        EnterShop,
        GoToVillage,
        GoToAdventure,
        EnterBlackSmith,
        Save,
        GoToWoodFarm,

        TalkToStatueRestaurator,
        TalkToGoodEnt,
        EstatuaOr,
        EstatuaEmmyr,
        EstatuaDamage,
        EstatuaSecondChance,
        EstatuaBendicion,
        ChestShop,
    };

    private bool inRange = false;
    

    private void Update()
    {
        if (playerIn && !inShop && interactionDialog != null)
        {
            interactionDialog.SetActive(true);
        }

        if (inRange && Input.GetButtonDown("Interact"))
        {
            SaveVariables.PLAYER_LIFE = player.maxHealth;
            SaveVariables.PLAYER_ATTACK = player.damage;
            SaveVariables.PLAYER_SPEED = player.moveSpeed;
            SaveVariables.PLAYER_ATTACK_SPEED = player.attackRate;
            SaveVariables.PLAYER_RANGE = player.coinMagnetRange;
            SaveVariables.PLAYER_DASH_RECOVERY = player.dashRestoreTime;
            SaveVariables.PLAYER_DASH_RANGE = player.dashForce;
            player.saveInventory();

            //Functionality of the interaction
            switch (interaction)
            {
                case Interactions.GoToVillage:
                    if (SceneManager.GetActiveScene().name != "WoodFarm")
                    {
                        if (player.gold >= 30)
                        {
                            if (player.gold - Mathf.RoundToInt(player.gold * 0.3f) >= 0)
                            {
                                SaveVariables.PLAYER_GOLD -= Mathf.RoundToInt(player.gold * 0.3f);
                                SaveManager.Instance.SaveGame();

                                SceneManager.LoadScene("Village");
                            }
                        }
                        //Else fer un Popup per mostrar que no te diners i que no pot viatjar

                    }
                    else
                    {
                        SaveManager.Instance.SaveGame();
                        SceneManager.LoadScene("Village");
                    }
                    
                    break;

                case Interactions.OpenChest:
                    Chest chest = GetComponentInParent<Chest>();
                    if(chest != null)
                    {
                        if (chest.hasMoney)
                        {
                            if (chest.opened)
                            {
                                if (Random.Range(0f, 1f) < chest.breakingProbability)
                                {
                                    Instantiate(chest.destruction);
                                    Instantiate(chest.destructionParticles, transform.position, Quaternion.identity);
                                    Destroy(chest.gameObject);
                                }
                                else
                                {
                                    Instantiate(chest.chestWinSound);
                                    chest.moneyQuantity += Random.Range(chest.minCoinStep, chest.maxCoinStep);
                                }
                            }
                        }
                        else
                        {
                            Instantiate(chest.destruction);
                            Instantiate(chest.destructionParticles, transform.position, Quaternion.identity);
                            Destroy(chest.gameObject);
                        }

                        if (!chest.opened)
                        {
                            if (chest.hasObject)
                            {
                                Instantiate(chest.objectToGive[Random.Range(0, chest.objectToGive.Length)], transform.position, Quaternion.identity);
                            }
                            Instantiate(chest.chestOpenSound);
                            chest.opened = true;
                            chest.anim.Play("chestOpen");
                        }
                        
                        
                    }
                    break;

                case Interactions.ChestShop:
                    if (!chestDetection.IsTouchingLayers(LayerMask.NameToLayer("chest")))
                    {
                        if (player.gold - chestShopPrice >= 0)
                        {
                            player.feedBackScreenPanel.GetComponent<Animator>().Play("SaveScreen");
                            player.gold -= chestShopPrice;
                            player.goldText.SetText(player.gold.ToString());
                            if (chestSpawnPoint.Length > 0 && chests.Length > 0)
                            {
                                Instantiate(chests[Random.Range(0, chests.Length)], chestSpawnPoint[0].transform.position, Quaternion.identity);
                                Instantiate(chests[Random.Range(0, chests.Length)], chestSpawnPoint[1].transform.position, Quaternion.identity);
                                Instantiate(chests[Random.Range(0, chests.Length)], chestSpawnPoint[2].transform.position, Quaternion.identity);
                            }
                        }
                    }
                    break;

                case Interactions.GoToAdventure:
                    SaveManager.Instance.SaveGame();
                    mapSelectionUi.SetActive(true);
                    inShop = true;
                    EventSystem.current.SetSelectedGameObject(mapSelectionUi.transform.GetChild(1).gameObject);
                    GameStateManager.Instance.SetState(GameState.Paused);
                    Time.timeScale = 0f;
                    break;

                case Interactions.EnterBlackSmith:
                    if (SaveVariables.TALKED_HALLBORG == 1)
                    {
                        if (!inShop)
                        {
                            player.inShop = true;
                            uiShop.show(player);
                            inShop = true;
                            if (interactionDialog != null)
                            {
                                interactionDialog.SetActive(false);
                            }
                            Time.timeScale = 0f;
                            GameStateManager.Instance.SetState(GameState.Paused);
                        }
                    }
                    else
                    {
                        string[] s = new string[5];
                        s[0] = "Hallborg: Hey Nysthel! I'm glad to see you are again with us.";
                        s[1] = "Hallborg: Do you remember me? I'm the blacksmith of Izhester, you used to play with my swords when you where young.";
                        s[2] = "Hallborg: I know that you want revenge, it's not going to be easy but I'm with you. Ulrus shouldn't get away without paying for what he did!";
                        s[3] = "Hallborg: I can improve your equipement and help you improve your skills!";
                        s[4] = "Hallborg: Since we are not in Izhester I'll need some gold to buy new tools, so if you find gold you can bring it to me and I'll " +
                            "help you get stronger.";
                        SaveVariables.TALKED_HALLBORG = 1;
                        FindObjectOfType<DialogSystem>().startDialog(s, this);
                        if (interactionDialog != null) { 

                            interactionDialog.SetActive(false);
                        }
                        this.enabled = false;
                    }
                    break;

                case Interactions.EnterShop:
                    if (SaveVariables.TALKED_GROMODIN == 1)
                    {
                        if (!inShop)
                        {
                            player.inShop = true;
                            uiItemShop.show(player);
                            inShop = true;
                            Time.timeScale = 0f;
                            GameStateManager.Instance.SetState(GameState.Paused);
                            if (interactionDialog != null)
                            {
                                interactionDialog.SetActive(false);
                            }
                        }
                        else
                        {
                            if (interactionDialog != null)
                            {
                                interactionDialog.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        string[] s = new string[7];
                        s[0] = "Gromodin: Hello dear customer! Do you see all this stuff?";
                        s[1] = "Gromodin: Well, it can be all yours by a modest price, it's the cheapest mercancy you will find in the area!";
                        s[2] = "Gromodin: Oh...";
                        s[3] = "Gromodin: It's you Nisthel...";
                        s[4] = "Gromodin: Great...";
                        s[5] = "Gromodin: I guess you could have a little discount for being part of this camp...";
                        s[6] = "Gromodin: But I'm not going to explain you what every item does, you will have to buy them and try them.";

                        SaveVariables.TALKED_GROMODIN = 1;
                        FindObjectOfType<DialogSystem>().startDialog(s, this);
                        if (interactionDialog != null)
                        {
                            interactionDialog.SetActive(false);
                        }
                        this.enabled = false;
                    }
                    break;

                case Interactions.Save:
                    player.feedBackScreenPanel.GetComponent<Animator>().Play("SaveScreen");
                    SaveManager.Instance.SaveGame();
                    break;

                case Interactions.GoToWoodFarm:
                    if (SaveVariables.PLAYER_GOLD >= 100)
                    { 
                        SaveVariables.PLAYER_GOLD = SaveVariables.PLAYER_GOLD - 100;
                        SaveManager.Instance.SaveGame();
                        SceneManager.LoadScene("WoodFarm");
                    }                    
                    break;

                case Interactions.EstatuaBendicion:
                    if(player.wood >= 5000 && SaveVariables.HOLY_STATUE == 0)
                    {
                        SaveVariables.PLAYER_WOOD -= 5000;
                        player.wood = SaveVariables.PLAYER_WOOD;
                        player.woodText.SetText(SaveVariables.PLAYER_WOOD.ToString());

                        SaveVariables.HOLY_STATUE = 1;
                    }
                    else if (SaveVariables.HOLY_STATUE == 1)
                    {
                        if (SaveVariables.ACTIVATED_STATUES < 3)
                        {
                            SaveVariables.HOLY_STATUE = 2;
                            SaveVariables.ACTIVATED_STATUES++;
                        }
                    }
                    else if (SaveVariables.HOLY_STATUE == 2)
                    {
                        SaveVariables.HOLY_STATUE = 1;
                        if (SaveVariables.ACTIVATED_STATUES > 0)
                            SaveVariables.ACTIVATED_STATUES--;
                    }
                    break;

                case Interactions.EstatuaDamage:
                    if (player.wood >= 4000 && SaveVariables.DAMAGE_STATUE == 0)
                    {
                        SaveVariables.PLAYER_WOOD -= 4000;
                        player.wood = SaveVariables.PLAYER_WOOD;
                        player.woodText.SetText(SaveVariables.PLAYER_WOOD.ToString());

                        SaveVariables.DAMAGE_STATUE = 1;
                    }
                    else if (SaveVariables.DAMAGE_STATUE == 1)
                    {

                        if (SaveVariables.ACTIVATED_STATUES < 3)
                        {
                            SaveVariables.ACTIVATED_STATUES++;
                            SaveVariables.DAMAGE_STATUE = 2;
                        }
                    }
                    else if (SaveVariables.DAMAGE_STATUE == 2)
                    {
                        SaveVariables.DAMAGE_STATUE = 1;
                        if (SaveVariables.ACTIVATED_STATUES > 0)
                            SaveVariables.ACTIVATED_STATUES--;
                    }
                    break;

                case Interactions.EstatuaEmmyr:
                    if (SaveVariables.PLAYER_WOOD >= 1000 && SaveVariables.HAS_EMMYR_ITEM == 1 && SaveVariables.EMMYR_STATUE == 0)
                    {
                        SaveVariables.PLAYER_WOOD -= 1000;
                        player.wood = SaveVariables.PLAYER_WOOD;
                        player.woodText.SetText(SaveVariables.PLAYER_WOOD.ToString());

                        SaveVariables.EMMYR_STATUE = 1;
                    }
                    else if (SaveVariables.EMMYR_STATUE == 1)
                    {
                       
                        if (SaveVariables.ACTIVATED_STATUES < 3)
                        {
                            SaveVariables.ACTIVATED_STATUES++;
                            SaveVariables.EMMYR_STATUE = 2;
                        }
                    }
                    else if (SaveVariables.EMMYR_STATUE == 2)
                    {
                        SaveVariables.EMMYR_STATUE = 1;
                        if (SaveVariables.ACTIVATED_STATUES > 0)
                            SaveVariables.ACTIVATED_STATUES--;
                    }
                    break;

                case Interactions.EstatuaOr:
                    if (SaveVariables.PLAYER_WOOD >= 3500 && SaveVariables.GOLD_STATUE == 0)
                    {
                        SaveVariables.PLAYER_WOOD -= 3500;
                        player.wood = SaveVariables.PLAYER_WOOD;
                        player.woodText.SetText(SaveVariables.PLAYER_WOOD.ToString());

                        SaveVariables.GOLD_STATUE = 1;
                    }
                    else if (SaveVariables.GOLD_STATUE == 1)
                    {
                        
                        if (SaveVariables.ACTIVATED_STATUES < 3)
                        {
                            SaveVariables.ACTIVATED_STATUES++;
                            SaveVariables.GOLD_STATUE = 2;
                        }
                    }
                    else if (SaveVariables.GOLD_STATUE == 2)
                    {
                        SaveVariables.GOLD_STATUE = 1;
                        if (SaveVariables.ACTIVATED_STATUES > 0)
                            SaveVariables.ACTIVATED_STATUES--;
                    }
                    break;

                case Interactions.EstatuaSecondChance:
                    if (player.wood >= 6666 && SaveVariables.CHANCE_STATUE == 0)
                    {
                        SaveVariables.PLAYER_WOOD -= 6666;
                        player.wood = SaveVariables.PLAYER_WOOD;
                        player.woodText.SetText(SaveVariables.PLAYER_WOOD.ToString());

                        SaveVariables.CHANCE_STATUE = 1;
                    }
                    else if (SaveVariables.CHANCE_STATUE == 1)
                    {
                        
                        if (SaveVariables.ACTIVATED_STATUES < 3)
                        {
                            SaveVariables.CHANCE_STATUE = 2;
                            SaveVariables.ACTIVATED_STATUES++;
                        }
                    }
                    else if (SaveVariables.CHANCE_STATUE == 2)
                    {
                        SaveVariables.CHANCE_STATUE = 1;
                        if (SaveVariables.ACTIVATED_STATUES > 0)
                            SaveVariables.ACTIVATED_STATUES--;
                    }

                    break;
                case Interactions.TalkToStatueRestaurator:
                    if (SaveVariables.TALKED_VORDKOR == 0)
                    {
                        string[] s = new string[12];
                        s[0] = "???: Hello Nysthel, finally awake!";
                        s[1] = "???: You've been sleeping for a long time, things have changed a lot around here since we found you in the ground after the attack, let me bring you up to date.";
                        s[2] = "???: The village we lived in, Izhester, was completely destroyed by Ulrus, a feared giant called the Burning King.";
                        s[3] = "???: And now, we've been forced to create this camp until things get a little better for us.";
                        s[4] = "???: The battle in Izhester was very hard, I'm so sorry about Emmyr...";
                        s[5] = "???: To tell the truth, there are very few of us who managed to survive the attack.";
                        s[6] = "???: In this camp we are only Gromodin the merchant, Hallborg the blacksmith, you and me.";
                        s[7] = "???: We couldn't get to know each other too well before the attack, I'm Vordkor the statue repairman.";
                        s[8] = "Vordkor: My job is to repair the statues that honor our ancestors, and protect our presents.";
                        s[9] = "Vordkor: If you find some wood, we can rebuild some statues that we managed to save from Izhester, these will give you strength!";
                        s[10] = "Vordkor: The statues are all over the camp, check them to see how much wood you will need.";
                        s[11] = "Vordkor: And Nysthel... Good luck, you'll need it.";

                        SaveVariables.TALKED_VORDKOR = 1;
                        FindObjectOfType<DialogSystem>().startDialog(s, null);
                        Destroy(marker);
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }
                    break;
                case Interactions.TalkToGoodEnt:
                    if (SaveVariables.HAS_EMMYR_ITEM == 0)
                    {
                        string[] s = new string[11];
                        s[0] = "???: Hey You!";
                        s[1] = "???: Yes yes, the redheaded viking girl!";
                        s[2] = "???: Over here, in the forest!";
                        s[3] = "???: I'm an Ent, my tribe is very furious with you because you are always cutting trees!";
                        s[4] = "Ent: But don't worry, I'm not angry with you.";
                        s[5] = "Ent: I was in Izhester and I saw everything. It was terrifying!";
                        s[6] = "Ent: After the attack, I entered the aftermath of the village and I found this.";
                        s[7] = "Ent: It looks like an ancient rune, it has an inscription that says: 'Emmyr's soul'.";
                        s[8] = "Ent: I thought that it was important so I came to give it back to you.";
                        s[9] = "* You Obtained: Emmyr's Soul*";
                        s[10] = "Ent: Please, don't tell anyone I have been here, or they will kill me.";

                        FindObjectOfType<DialogSystem>().startDialog(s, this);
                        if (interactionDialog != null)
                        {
                            interactionDialog.SetActive(false);
                        }
                        this.enabled = false;

                        SaveVariables.HAS_EMMYR_ITEM = 1;
                    }
                    else
                    {
                        string[] s = new string[1];
                        s[0] = "Ent: Shhhtt!! I'm a tree. Trees don't talk.";

                        FindObjectOfType<DialogSystem>().startDialog(s, this);
                        if (interactionDialog != null)
                        {
                            interactionDialog.SetActive(false);
                        }
                        this.enabled = false;
                    }
                    break;
            }
        }

        if ((Input.GetButtonDown("Pause") || Input.GetButtonDown("Inventory") || Input.GetButtonDown("Cancel")) && inShop)
        {
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().timeSlowed)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0.5f;
            }
            GameStateManager.Instance.SetState(GameState.Gameplay);
            uiShop.hide();
            uiItemShop.hide();
            if (mapSelectionUi != null)
            {
                mapSelectionUi.SetActive(false);
            }
            inShop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIn = true;
            player = collision.GetComponent<Player>();
            if (text != null)
            {
                switch (interaction)
                {
                    case Interactions.GoToVillage:
                        if (SceneManager.GetActiveScene().name != "WoodFarm")
                        {
                            text.SetText("Press X or E to go to the Village by 30% of your gold, you need to have at least 30 gold. (" + Mathf.RoundToInt(SaveVariables.PLAYER_GOLD * 0.3f) + ")");
                        }
                        else
                        {
                            text.SetText("Press X or E to return to the Village.");
                        }
                        break;
                    case Interactions.OpenChest:
                        text.SetText("Press X or E to open chest!");
                        break;
                    case Interactions.GoToAdventure:
                        text.SetText("Press X or E to select a destination!");
                        break;
                    case Interactions.EnterBlackSmith:
                        text.SetText("Press X or E to talk with the BlackSmith");
                        break;
                    case Interactions.EnterShop:
                        text.SetText("Press X or E to talk with the Mercader");
                        break;
                    case Interactions.Save:
                        text.SetText("Press X or E to save your progress.");
                        break;
                    case Interactions.GoToWoodFarm:
                        text.SetText("Press X or E to go to the Wood Farm. It's not a safe place! (Cost: 100)");
                        break;
                    case Interactions.EstatuaBendicion:
                        text.SetText("Holy Statue, your projectiles will destroy the enemy projectiles! (press to Activate or Desactivate)");
                        if (SaveVariables.HOLY_STATUE == 0)
                            text.SetText("Press X or E to build the Holy Statue! (2500 wood)");
                        break;
                    case Interactions.EstatuaDamage:
                        text.SetText("Angry Statue, your damage has been augmented (press to Activate or Desactivate)");
                        if (SaveVariables.DAMAGE_STATUE == 0)
                            text.SetText("Press X or E to build the Angry Statue! (4000 wood)");
                        break;
                    case Interactions.EstatuaEmmyr:
                        text.SetText("Emmyr's Statue, his soul will allways be with you (press to Activate or Desactivate)");
                        if (SaveVariables.EMMYR_STATUE == 0)
                            text.SetText("Press X or E to build Emmyr's Statue! (1000 wood & Emmyr's Soul)");
                        break;
                    case Interactions.EstatuaOr:
                        text.SetText("Golden Statue, coins will be more valuable (press to Activate or Desactivate)");
                        if (SaveVariables.GOLD_STATUE == 0)
                            text.SetText("Press X or E to build the Golden Statue! (3500 wood)");
                        break;
                    case Interactions.EstatuaSecondChance:
                        text.SetText("Resurrection Statue, your chances of having a second chance have been incremented (press to Activate or Desactivate)");
                        if (SaveVariables.CHANCE_STATUE == 0)
                            text.SetText("Press X or E to build the Resurrection Statue! (6666 wood)");
                        break;
                    case Interactions.TalkToStatueRestaurator:
                        text.SetText("Press X or E to talk with the statue restaurator!");
                        break;
                    case Interactions.TalkToGoodEnt:
                        text.SetText("Press X or E to talk with the Good Ent!");
                        break;
                    case Interactions.ChestShop:
                        text.SetText("Press X or E to buy 3 random gold Chests by " + chestShopPrice + " gold");
                        break;
                }
            }
            if (interactionDialog != null)
            {
                interactionDialog.SetActive(true);
            }
            inRange = true;
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIn = false;
            if (mapSelectionUi != null)
            {
                mapSelectionUi.SetActive(false);
                inShop = false;
            }
            this.enabled = true;
            if (interactionDialog != null)
            {
                interactionDialog.SetActive(false);
            }
            inRange = false;
            if (inShop)
            {
                inShop = false;
                player.inShop = false;
                uiShop.hide();
                uiItemShop.hide();
                if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().timeSlowed)
                {
                    Time.timeScale = 1f;
                }
                else
                {
                    Time.timeScale = 0.5f;
                }
                GameStateManager.Instance.SetState(GameState.Gameplay);
            }
        }
    }
}
