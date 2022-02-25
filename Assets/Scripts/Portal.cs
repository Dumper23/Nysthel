using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public bool ScenePortal = true;
    public bool isPortalBoss = true;
    public string SceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Add sound of teleport
            Invoke("ChangeScene", 2f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //We cancel the teleport
            CancelInvoke("ChangeScene");
        }
    }

    private void ChangeScene()
    {
        if (isPortalBoss)
        {
            if (SaveVariables.CURRENT_WORLD >= SaveVariables.MAX_WORLD) {
                //Pujar el nivell del mon per poder accedir a altres mons
                SaveVariables.MAX_WORLD++;
            }
        }

        SaveManager.Instance.SaveGame();

        SceneManager.LoadScene(SceneToLoad);
    }
}
