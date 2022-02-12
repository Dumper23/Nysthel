using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedRooms : MonoBehaviour
{
   
    void Start()
    {
        if(transform.position != Vector3.zero)
        {
            Instantiate(GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>().closedRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
