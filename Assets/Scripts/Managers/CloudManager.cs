using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject cloud;
    public int cloudNumber = 10;
    public int maxDist = 300;
    public List<GameObject> clouds = new List<GameObject>();

    private Vector3 player;

    private void Start()
    {
        if (FindObjectOfType<Player>())
        {
            player = FindObjectOfType<Player>().transform.position;
        }
        else
        {
            player = transform.position;
        }
        for (int i = 0; i < cloudNumber; i++)
        {
            Vector3 pos = player + new Vector3(Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist), 0);
            Vector3 rotation = new Vector3(0, 0, Random.Range(0, 360));
            GameObject Icloud = Instantiate(cloud, pos, Quaternion.Euler(rotation));
            Icloud.transform.position = pos;
            Icloud.GetComponent<Cloud>().maxDist = maxDist;
            Icloud.GetComponent<Cloud>().target = player;
            clouds.Add(Icloud);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(player, maxDist);
    }
}