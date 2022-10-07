﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    public bool isCloseToStart = false;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private RoomTemplates templates;
    private int rand;
    private int emergencyBreak = 0;

    public bool spawned = false;

    public float waitTime = 4f;

    private void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {
        if (spawned == false)
        {
            Debug.Log("Room Spawner whiles started (" + gameObject.GetComponentInParent<AddRoom>().gameObject.name + ")");
            if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door.
                rand = Random.Range(0, templates.bottomRooms.Length);
                if (isCloseToStart)
                {
                    Instantiate(templates.bottomRooms[0], transform.position, templates.bottomRooms[0].transform.rotation);
                }
                else
                {
                    Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                }
               
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door.
                rand = Random.Range(0, templates.topRooms.Length);
                if (isCloseToStart)
                {
                    Instantiate(templates.topRooms[0], transform.position, templates.topRooms[0].transform.rotation); ;
                }
                else
                {
                    Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                }
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a LEFT door.
                rand = Random.Range(0, templates.leftRooms.Length);
                if (isCloseToStart)
                {
                    Instantiate(templates.leftRooms[0], transform.position, templates.leftRooms[0].transform.rotation);
                }
                else
                {
                    Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                }
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a RIGHT door.
                rand = Random.Range(0, templates.rightRooms.Length);
                if (isCloseToStart)
                {
                    Instantiate(templates.rightRooms[0], transform.position, templates.rightRooms[0].transform.rotation);
                }
                else
                {
                    Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                }
            }
            Debug.Log("Room Spawner whiles finished (" + gameObject.GetComponentInParent<AddRoom>().gameObject.name + ")");
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            spawned = true;
            if (other.GetComponent<RoomSpawner>() != null)
            {
                if (!other.GetComponent<RoomSpawner>().spawned && other.transform.name != "Destroyer")
                {
                    GameObject go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
                    go.transform.name = "closedRoom";
                    go.AddComponent<ClosedRooms>();
                }
            }
        }
    }
}