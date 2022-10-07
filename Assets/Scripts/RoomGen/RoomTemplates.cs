using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
    public GameObject entryRoom;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;

    public GameObject boss;
    public GameObject bossIndicator;
    public GameObject[] bossBarriers;
    public AudioClip endBoss;
    public AudioClip startBoss;

    private float initialWait;
    private int emergencyBreak = 0;
    private bool spawnedBoss;
    private int roomIndex = 0;
    private bool doing = false;

    private void Start()
    {
        Debug.Log("Template start");
        initialWait = waitTime;
        GameStateManager.Instance.SetState(GameState.Paused);
        GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false && !doing)
        {
            doing = true;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    if (rooms[i].GetComponent<AddRoom>().canSpawnBoss)
                    {
                        Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                        Instantiate(bossIndicator, rooms[i].transform.position, Quaternion.identity);
                        spawnedBoss = true;
                        roomIndex = i;
                        GameStateManager.Instance.SetState(GameState.Gameplay);
                        GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else if (i - 1 >= 0 && rooms[i - 1].GetComponent<AddRoom>().canSpawnBoss)
                    {
                        Instantiate(boss, rooms[i - 1].transform.position, Quaternion.identity);
                        Instantiate(bossIndicator, rooms[i - 1].transform.position, Quaternion.identity);
                        spawnedBoss = true;
                        roomIndex = i - 1;
                        GameStateManager.Instance.SetState(GameState.Gameplay);
                        GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else if (i - 2 >= 0 && rooms[i - 2].GetComponent<AddRoom>().canSpawnBoss)
                    {
                        Instantiate(boss, rooms[i - 2].transform.position, Quaternion.identity);
                        Instantiate(bossIndicator, rooms[i - 2].transform.position, Quaternion.identity);
                        spawnedBoss = true;
                        roomIndex = i - 2;
                        GameStateManager.Instance.SetState(GameState.Gameplay);
                        GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else if (i - 3 >= 0 && rooms[i - 3].GetComponent<AddRoom>().canSpawnBoss)
                    {
                        Instantiate(boss, rooms[i - 3].transform.position, Quaternion.identity);
                        Instantiate(bossIndicator, rooms[i - 3].transform.position, Quaternion.identity);
                        spawnedBoss = true;
                        roomIndex = i - 3;
                        GameStateManager.Instance.SetState(GameState.Gameplay);
                        GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            
            if (!spawnedBoss)
            {
                Debug.Log("Room Template while started");
                for(int i = 0; i < rooms.Count; i++)
                {
                    if (!spawnedBoss && rooms[i].GetComponent<AddRoom>().canSpawnBoss)
                    {
                        Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                        Instantiate(bossIndicator, rooms[i].transform.position, Quaternion.identity);
                        spawnedBoss = true;
                        GameStateManager.Instance.SetState(GameState.Gameplay);
                        GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                Debug.Log("Room Template while finished");
                if (!spawnedBoss)
                {
                    Debug.Log("No boss :(");
                }
            }
            else{
                Debug.Log("Room Template no while needed");
            }
            doing = false;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}