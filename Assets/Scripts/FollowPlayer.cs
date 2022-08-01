using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (player.position.x > transform.position.x)
        {
            transform.position = player.transform.position - new Vector3(0.5f, 0, 0);
        }
        else
        {
            transform.position = player.transform.position + new Vector3(0.5f, 0, 0);
        }
    }
}