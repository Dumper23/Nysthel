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

    private bool spawnedBoss;
    private int roomIndex = 0;

    private void Start()
    {
        GameStateManager.Instance.SetState(GameState.Paused);
        GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
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
                int i = 0;
                while (!spawnedBoss)
                {
                    if (i < rooms.Count)
                    {
                        if (rooms[i].GetComponent<AddRoom>().canSpawnBoss)
                        {
                            Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                            Instantiate(bossIndicator, rooms[i].transform.position, Quaternion.identity);
                            spawnedBoss = true;
                            GameStateManager.Instance.SetState(GameState.Gameplay);
                            GameObject.FindGameObjectWithTag("LoadingScreen").transform.GetChild(0).gameObject.SetActive(false);
                        }
                        i++;
                    }
                    else
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                }
            }

            /*foreach(GameObject room in rooms)
            {
				if((room.transform.position - entryRoom.transform.position).magnitude <= 35)
                {
					Debug.Log(room.transform.name + " is a close room");
                }
            }*/
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}