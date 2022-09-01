using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour
{
    bool done = false;
    float time = 0;
    public Animator fade;
    public Animator fadeMusic;

    void Update()
    {
        if (Input.anyKey)
        {
            time += Time.deltaTime;
            if (time >= 3)
            {
                fade.Play("fade");
                fadeMusic.Play("fadeMusic");
                Invoke("go", 3.5f);
            }
        }
        else
        {
            time = 0;
        }
    }

    private void go()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
