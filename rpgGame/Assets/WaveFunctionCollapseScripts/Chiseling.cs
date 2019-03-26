using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiseling : MonoBehaviour
{
    GridManager gridManager;
    public slot[,,] grid;
    List<slot> fixedPoints = new List<slot>();
    List<slot> allPoints = new List<slot>();
    int tries = 0;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    public void AssignFixedPoints()
    {
        for (int i = 0; i < gridManager.gridX; i++)
        {
            for (int k = 0; k < gridManager.gridY; k++)
            {
                for (int j = 0; j < gridManager.gridZ; j++)
                {
                    allPoints.Add(grid[i, k, j]);
                    grid[i, k, j].isFixed = false;
                }
            }
        }

        fixedPoints.Add(grid[0, 0, 0]);
        grid[0, 0, 0].isFixed = true;
        fixedPoints.Add(grid[30, 0, 19]);
        grid[30, 0, 19].isFixed = true;
        fixedPoints.Add(grid[0, 0, 45]);
        grid[0, 0, 45].isFixed = true;
        fixedPoints.Add(grid[19, 0, 25]);
        grid[19, 0, 25].isFixed = true;
        StartCoroutine(TryToRemove());
    }

    public IEnumerator TryToRemove()
    {
        int itemToRemove = Random.Range(0, allPoints.Count);
        if (allPoints[itemToRemove].isFixed)
        {
            yield return null;
            StartCoroutine(TryToRemove());
        }
        allPoints[itemToRemove].isPath = false;

        if (CheckFixedPoints())
        {
            tries = 0;
            allPoints.RemoveAt(itemToRemove);
        }
        else
        {
            tries++;
            allPoints[itemToRemove].isPath = true;
        }

        if (tries > 100)
        {
            gridManager.startBuilding();
        }
        else
        {
            Reset();
            yield return null;
            StartCoroutine(TryToRemove());
        }
    }



    void Visit(slot slot)
    {
            if (slot.isPath && !slot.isVisited)
            {
                slot.isVisited = true;
                if (slot.x < gridManager.gridX - 1)
                    Visit(grid[slot.x + 1, slot.y, slot.z]);
                if (slot.x > 0)
                    Visit(grid[slot.x - 1, slot.y, slot.z]);
                if (slot.z > 0)
                    Visit(grid[slot.x, slot.y, slot.z - 1]);
                if (slot.z < gridManager.gridZ - 1)
                    Visit(grid[slot.x, slot.y, slot.z + 1]);
            }
    }

    private void Reset()
    {
            for (int i = 0; i < gridManager.gridX; i++)
            {
                for (int k = 0; k < gridManager.gridY; k++)
                {
                    for (int j = 0; j < gridManager.gridZ; j++)
                    {
                        grid[i, k, j].isVisited = false;
                    }
                }
            }
        }

    private bool CheckFixedPoints()
    {
        Visit(fixedPoints[0]);
        for (int i = 0; i < fixedPoints.Count; i++)
        {
            if (!fixedPoints[i].isVisited)
            {
                return false;
            }
        }
        return true;
    }
}
