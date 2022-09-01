using System.Collections;
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
            if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door.
                rand = Random.Range(0, templates.bottomRooms.Length);
                if (isCloseToStart)
                {
                    emergencyBreak = 0;
                    while (emergencyBreak <= 100 || templates.bottomRooms[rand].name == "B" || templates.bottomRooms[rand].name == "T" || templates.bottomRooms[rand].name == "L" || templates.bottomRooms[rand].name == "R")
                    {
                        emergencyBreak++;
                        rand = Random.Range(0, templates.bottomRooms.Length);
                    }
                }
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door.
                rand = Random.Range(0, templates.topRooms.Length);
                if (isCloseToStart)
                {
                    emergencyBreak = 0;
                    while (emergencyBreak <= 100 || templates.topRooms[rand].name == "B" || templates.topRooms[rand].name == "T" || templates.topRooms[rand].name == "L" || templates.topRooms[rand].name == "R")
                    {
                        rand = Random.Range(0, templates.topRooms.Length);
                        emergencyBreak++;
                    }
                }
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a LEFT door.
                rand = Random.Range(0, templates.leftRooms.Length);
                if (isCloseToStart)
                {
                    emergencyBreak = 0;
                    while (emergencyBreak <= 100 || templates.leftRooms[rand].name == "B" || templates.leftRooms[rand].name == "T" || templates.leftRooms[rand].name == "L" || templates.leftRooms[rand].name == "R")
                    {
                        rand = Random.Range(0, templates.leftRooms.Length);
                        emergencyBreak++;
                    }
                }
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a RIGHT door.
                rand = Random.Range(0, templates.rightRooms.Length);
                if (isCloseToStart)
                {
                    emergencyBreak = 0;
                    while (emergencyBreak <= 100 || templates.rightRooms[rand].name == "B" || templates.rightRooms[rand].name == "T" || templates.rightRooms[rand].name == "L" || templates.rightRooms[rand].name == "R")
                    {
                        rand = Random.Range(0, templates.rightRooms.Length);
                        emergencyBreak++;
                    }
                }
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
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