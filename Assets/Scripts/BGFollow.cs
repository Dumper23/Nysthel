using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFollow : MonoBehaviour
{
    public float parallaxEffect;
    private Transform target;

    private float lengthX, lengthY, startposX, startposY;


    void Start()
    {
        target = Camera.main.transform;
        startposX = transform.position.x;
        startposY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (target.transform.position.x * (1 - parallaxEffect));
        float distX = (target.transform.position.x * parallaxEffect);

        float tempY = (target.transform.position.y * (1 - parallaxEffect));
        float distY = (target.transform.position.y * parallaxEffect);

        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);
        if (tempX > startposX + lengthX) startposX += lengthX;
        else if (tempX < startposX - lengthX) startposX -= lengthX;

        if (tempY > startposY + lengthY) startposY += lengthY;
        else if (tempY < startposY - lengthY) startposY -= lengthY;

    }
}
