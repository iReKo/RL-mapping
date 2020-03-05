using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleWalls : MonoBehaviour
{
    public float curvature;
    public FlexibleWall[] walls;


    // Update is called once per frame
    void FixedUpdate()
    {
        foreach( FlexibleWall wall in walls)
        {
            wall.curvature = curvature;
        }
    }
}
