using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutScene : MonoBehaviour
{
    public float timeToChangeScene = 32f;
    
    void Start()
    {
        Time.timeScale = 0.75f;
        Invoke("end", timeToChangeScene);
    }

    void end()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }
   
}
