using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSize : MonoBehaviour
{
    GridManager gm;

    public int gridX;
    public int gridY;
    public int gridZ;

     public List<float> widthMod = new List<float>();
     public List<float> heightMod = new List<float>();

    public List<float> widthSum = new List<float>();
    public List<float> heightSum = new List<float>();

    public float widthSumTotal =0;
    public float heightSumTotal =0;


    public void Initialize()
    {
        gm = GetComponent<GridManager>();

        gridX = gm.gridX;
        gridZ = gm.gridZ;
        gridY = gm.gridY;

        for (int i = 0; i < gridX; i++)
        {
            widthMod.Add(Random.Range(0.5f, 1.5f));
        }

        for (int i = 0; i < gridZ; i++)
        {
            heightMod.Add(Random.Range(0.5f, 1.5f));
        }

        for (int i = 0; i < widthMod.Count; i++)
        {
            widthSumTotal += widthMod[i];
            widthSum.Add(widthSumTotal);
        }

        for (int i = 0; i < heightMod.Count; i++)
        {
            heightSumTotal += heightMod[i];
            heightSum.Add(heightSumTotal);
        }


    }

    
}
