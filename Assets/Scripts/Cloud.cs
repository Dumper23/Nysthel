using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public int maxDist;
    public Vector3 target;

    private void Start()
    {
        speed = Random.Range(1f, 5f);
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0); ;
    }

    private void Update()
    {
        if ((transform.position + target).magnitude < maxDist)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-direction * speed * Time.deltaTime);
        }
    }
}