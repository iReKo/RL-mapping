using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleWall : MonoBehaviour
{
    public float curvature = 0;


    // Update is called once per frame
    void FixedUpdate()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Vector3[] vertices = meshFilter.mesh.vertices;

        if (curvature == 0)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = 0;
            }
        } else if (curvature < 0 && curvature > -Mathf.Sqrt(2) / 10f)
        {
            
            float r = 1f / curvature;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = -(Mathf.Sqrt(r * r - vertices[i].x * vertices[i].x) - Mathf.Sqrt(r * r - 25));
            }
        } else if (curvature > 0 && curvature < Mathf.Sqrt(2) / 10f)
        {
            float r = 1f / curvature;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = Mathf.Sqrt(r*r - vertices[i].x * vertices[i].x) - Mathf.Sqrt(r * r - 25);
            }
        }

        meshFilter.mesh.vertices = vertices;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.mesh;
    }
}
