using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideAfterTime : MonoBehaviour
{
    public float timeToHide = 20f;

    private void OnEnable()
    {
        Invoke("hide", timeToHide);
    }
    private void hide()
    {
        gameObject.SetActive(false);
    }
}
