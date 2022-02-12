using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBasic : MonoBehaviour
{

    public GameObject[] environmentObjects;
    public Transform[] points;

    [Range(0f, 1f)]
    public float probability_ = 0.75f;

    private float[] probability;
    private GameObject[] instantiated;

    void Start()
    {
        probability = new float[points.Length];
        instantiated = new GameObject[points.Length];
        reload();
    }

    public void reload()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (instantiated[i] != null)
            {
                Destroy(instantiated[i]);
            }

            probability[i] = probability_;
            if (Random.Range(0f, 1f) <= probability[i])
            {
                instantiated[i] = Instantiate(
                    environmentObjects[Random.Range(0, environmentObjects.Length)],
                    points[i].transform.position,
                    Quaternion.identity) as GameObject;

            }
        }
    }
}
