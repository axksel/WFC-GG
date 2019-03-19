using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{

    public Texture2D noise;
    public Color[,] pixelValues;
    public slot[,,] grid;

    void Awake()
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

    public void InitNoiseWeights(int sizeX, int sizeY, int sizeZ)
    {
        //Debug.Log(pixelValues.Length);
        for (int i = 0; i < sizeX; i++)
        {
            for (int k = 0; k < sizeY; k++)
            {
                for (int j = 0; j < sizeZ; j++)
                {
                    grid[i, k, j].noiseWeight = pixelValues[i, j].grayscale;
                }
            }
        }
    }
}
