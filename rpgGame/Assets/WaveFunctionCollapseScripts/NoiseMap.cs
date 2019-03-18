using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{

    public Texture2D noise;
    public Color[,] pixelValues;

    void Start()
    {
        pixelValues = new Color[noise.width, noise.height];
        for (int i = 0; i < noise.width; i++)
        {
            for (int j = 0; j < noise.height; j++)
            {
                pixelValues[i, j] = noise.GetPixel(i, j);
            }
        }
    }

    void Update()
    {
        
    }
}
