
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{

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

	void Update()
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
                    }
                    else if(i - 1 >= 0 && rooms[i-1].GetComponent<AddRoom>().canSpawnBoss)
                    {
						Instantiate(boss, rooms[i-1].transform.position, Quaternion.identity);
						Instantiate(bossIndicator, rooms[i-1].transform.position, Quaternion.identity);
						spawnedBoss = true;
						roomIndex = i-1;
					}
					else if (i - 2 >= 0 && rooms[i - 2].GetComponent<AddRoom>().canSpawnBoss)
					{
						Instantiate(boss, rooms[i - 2].transform.position, Quaternion.identity);
						Instantiate(bossIndicator, rooms[i - 2].transform.position, Quaternion.identity);
						spawnedBoss = true;
						roomIndex = i-2;
					}
					else if (i - 3 >= 0 && rooms[i - 3].GetComponent<AddRoom>().canSpawnBoss)
					{
						Instantiate(boss, rooms[i - 3].transform.position, Quaternion.identity);
						Instantiate(bossIndicator, rooms[i - 3].transform.position, Quaternion.identity);
						spawnedBoss = true;
						roomIndex = i - 3;
					}
				}
			}

			if (!spawnedBoss)
			{
				int i = 0;
				while (!spawnedBoss)
				{
					if (i <= rooms.Count)
					{
						if (rooms[i].GetComponent<AddRoom>().canSpawnBoss)
						{
							Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
							Instantiate(bossIndicator, rooms[i].transform.position, Quaternion.identity);
							spawnedBoss = true;
						}
						i++;
                    }
                    else
                    {
						break;
                    }
				}
			}
		}
		else
		{
			waitTime -= Time.deltaTime;
		}
	}
}