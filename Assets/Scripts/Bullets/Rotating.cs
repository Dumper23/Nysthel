using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float rotationSpeed = 20f;
    public Transform center;

    // Update is called once per frame
    void Update()
    {
        center.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
