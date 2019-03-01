using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridManager : MonoBehaviour
{
    public int gridX = 4;
    public int gridY = 4;
    public int gridZ = 4;

    public List<GameObject> modules = new List<GameObject>();
    public ScriptableObjectList moduleSO;

    public slot[,,] grid;



    void Start()
    {
        modules = moduleSO.list;
        grid = new slot[gridX, gridY,gridZ];
        //initialze grid
        int index = 0;
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {


                    grid[i, k,j] = new slot();
                    grid[i, k,j].posibilitySpace.AddRange(modules);

                    grid[i, k,j].index = index;
                    index++;
                }
            }
        }
        //Assign neighbours
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {


                    try
                    {
                        grid[i, k,j].neighbours[0] = grid[i + 1, k,j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k,j].neighbours[1] = grid[i - 1, k,j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k,j].neighbours[2] = grid[i, k ,j + 1];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k,j].neighbours[3] = grid[i, k , j - 1];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k, j].neighbours[4] = grid[i, k - 1 , j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k, j].neighbours[5] = grid[i, k + 1, j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                }
            }
        }


        grid[5, 0 ,5].collapse(0);
        Build();
    }

    void OnDrawGizmos()
    {
        int index = 0;

        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {


                    try
                    {
                        Handles.Label(new Vector3(i, k, j) , grid[i, k, j].posibilitySpace.Count.ToString());
                        index++;
                    }
                    catch (System.NullReferenceException) { }
                }
            }
        }

    }

    void Build()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {


                    if (grid[i, k,j].posibilitySpace.Count == 1 && grid[i, k, j].collapsed != true)
                    {
                        Instantiate(grid[i, k, j].posibilitySpace[0], grid[i, k, j].posibilitySpace[0].transform.position + new Vector3(i, k, j), grid[i, k, j].posibilitySpace[0].transform.rotation);
                        grid[i, k, j].collapsed = true;
                    }
                }
            }
        }
    }

    public void Collapse()
    {

        int iTmp = 0;
        int kTmp = 0;
        int jTmp = 0;
        int count = 200000;

        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    if (count > grid[i, k,j].posibilitySpace.Count && grid[i, k,j].posibilitySpace.Count > 1)
                    {
                        count = grid[i, k,j].posibilitySpace.Count;
                        iTmp = i;
                        kTmp = k;
                        jTmp = j;
                    }
                }
            }
        }
        grid[iTmp, kTmp,jTmp].collapse();
        Build();
    }

    public void Iterate()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    grid[i, k,j].Iterate();
                }
            }
        }
        Build();
    }



    public void IterateAndCollapse()
    {
        int gridTmp = 0;
        bool shouldIterate = false;

        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    gridTmp = grid[i, k,j].posibilitySpace.Count;
                    grid[i, k,j].Iterate();

                    if (gridTmp != grid[i, k,j].posibilitySpace.Count)
                    {
                        shouldIterate = true;
                    }
                }
            }
        }

        if (shouldIterate)
        {

            IterateAndCollapse();
        }
        else
        {
            Collapse();
            Build();
        }

    }


}
