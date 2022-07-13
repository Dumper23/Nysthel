using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;
    public bool isEmmyrSoul = false;
    public GameObject light;
    public GameObject particles;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (isEmmyrSoul && !light.activeInHierarchy && SaveVariables.HAS_EMMYR_ITEM == 1)
        {
            light.SetActive(true);
            particles.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if (Target != null)
        {
            Vector3 targetPosition = Target.position + Offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}