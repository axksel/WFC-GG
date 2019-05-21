using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{

    public Texture2D noise;
    public Color[,] pixelValues;
    public slot[,,] grid;
    public ScriptableObjectList modules;
    GridManager gm;

    public Texture2D nTex;

    void Awake()
    {
        for (int i = 0; i < modules.list.Count; i++)
        {
            modules.list[i].GetComponent<Modulescript>().weightModifier = 0;
            for (int j = 0; j < modules.list[i].GetComponent<Modulescript>().neighbours.Length - 2; j++)
            {
                if (modules.list[i].GetComponent<Modulescript>().neighbours[j] == 0) modules.list[i].GetComponent<Modulescript>().weightModifier += 0.25f;
            }

        }


    }

    public void InitNoiseWeights(int sizeX, int sizeY, int sizeZ)
    {
        gm = GetComponent<GridManager>();
        Texture2D noise2 = noise;
        TextureScale.Bilinear(noise2, gm.gridX, gm.gridZ);

        pixelValues = new Color[noise2.width, noise2.height];
        for (int i = 0; i < noise2.width; i++)
        {
            for (int j = 0; j < noise2.height; j++)
            {
                pixelValues[i, j] = noise2.GetPixel(i, j);
            }
        }

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

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                              incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
}
