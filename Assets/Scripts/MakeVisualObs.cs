using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisualObs : MonoBehaviour
{

    public hapticsSensor2 hapticsSensor;

    private Renderer renderer;
    private Texture2D tex;

    void Start()
    {
        renderer = this.GetComponent<Renderer>();
        tex = new Texture2D(360, 1);
    }

    void FixedUpdate()
    {
        int w = 360, h = 1;

        for(int widx = 0; widx < w; widx++)
        {
            for(int hidx = 0; hidx < h; hidx++)
            {
                float dist = hapticsSensor.distances[widx + (widx < w/2 ? w/2: -w/2)];
                tex.SetPixel(widx, hidx, new Color(dist, dist, dist, 1));
            }
        }

        tex.Apply();
        renderer.material.mainTexture = tex;
    }
}
