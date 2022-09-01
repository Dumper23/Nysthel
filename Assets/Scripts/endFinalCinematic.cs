using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endFinalCinematic : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("endGame");
    }
}
