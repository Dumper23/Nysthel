using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Vector2 pos, dir;
    private GameObject laserObj;
    LineRenderer laser;
    List<Vector2> laserIndices = new List<Vector2>();

    public Laser(Vector2 pos, Vector2 dir, Material material)
    {
        this.laser = new LineRenderer();
        this.laserObj = new GameObject();
        this.laserObj.name = "Laser";
        this.pos = pos;
        this.dir = dir;

        this.laser = this.laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        this.laser.material = material;
        this.laser.startColor = Color.red;
        this.laser.endColor = Color.red;

        CastRay(pos, dir, laser);
    }

    void CastRay(Vector2 pos, Vector2 dir, LineRenderer laser)
    {
        laserIndices.Add(pos);
        if (Physics2D.Raycast(pos, dir)){
            RaycastHit2D hit = Physics2D.Raycast(pos, dir);
            laserIndices.Add(hit.point);
            updateLaser();
        }
        else
        {
            laserIndices.Add(pos * 30f);
            updateLaser();
        }
    }

    void updateLaser()
    {
        int count = 0;
        laser.positionCount = laserIndices.Count;
        foreach (Vector2 idx in laserIndices)
        {
            laser.SetPosition(count, idx);
            count++;
        }
    }
}
