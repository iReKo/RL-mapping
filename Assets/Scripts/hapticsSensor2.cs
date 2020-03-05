using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hapticsSensor2 : MonoBehaviour
{
    public LayerMask layerMask;
    public float[] distances;
    public float ray_length = 10;

    private void Start()
    {
        distances = new float[360];
        for (int i = 0; i < 360; i++)
        {
            distances[i] = 1;
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray[] rays = new Ray[360];
        for (int i = 0; i < 360; i++)
        {
            rays[i] = new Ray(transform.position, transform.TransformVector(new Vector3(Mathf.Cos((float)i * Mathf.Deg2Rad), 0, Mathf.Sin((float)i * Mathf.Deg2Rad))));
            //Debug.DrawRay(rays[i].origin, rays[i].direction * ray_length, Color.red);
            if (Physics.Raycast(rays[i], out hit, ray_length, layerMask))
            {
                distances[i] = hit.distance / ray_length;
            } else
            {
                distances[i] = 1;
            }
        }

    }
}
