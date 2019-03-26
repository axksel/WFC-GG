using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiseling : MonoBehaviour
{
    GridManager gridManager;
    public slot[,,] grid;
    List<slot> fixedPoints = new List<slot>();
    List<slot> allPoints = new List<slot>();

    private void Start()
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
        fixedPoints.Add(grid[9, 0, 9]);
        grid[9, 0, 9].isFixed = true;
        fixedPoints.Add(grid[0, 0, 9]);
        grid[0, 0, 9].isFixed = true;
        fixedPoints.Add(grid[9, 0, 0]);
        grid[9, 0, 0].isFixed = true;
        StartCoroutine(TryToRemove());
    }

    public IEnumerator TryToRemove()
    {
        Debug.Log(allPoints.Count);

        int itemToRemove = Random.Range(0, allPoints.Count);
        if (allPoints[itemToRemove].isFixed)
        {
            yield return null;
            StartCoroutine(TryToRemove());
        }
        allPoints[itemToRemove].isPath = false;

        if (CheckFixedPoints())
        {
            allPoints.RemoveAt(itemToRemove);
        }
        else
        {
            allPoints[itemToRemove].isPath = true;
        }

        if (fixedPoints.Count == allPoints.Count)
        {
            Debug.Log("Finished");
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
                if (slot.x < gridManager.gridX - 1 && slot.z < gridManager.gridZ - 1)
                    Visit(grid[slot.x + 1, slot.y, slot.z + 1]);
                if (slot.x > 0 && slot.z > 0)
                    Visit(grid[slot.x - 1, slot.y, slot.z - 1]);
                if (slot.x < gridManager.gridX - 1 && slot.z > 0)
                    Visit(grid[slot.x + 1, slot.y, slot.z - 1]);
                if (slot.x > 0 && slot.z < gridManager.gridZ - 1)
                    Visit(grid[slot.x - 1, slot.y, slot.z + 1]);
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
        foreach (var item in fixedPoints)
        {
            Visit(item);
            foreach (var item2 in fixedPoints)
            {
                if (!item2.isVisited)
                {
                    return false;
                }
            }
            Reset();
        }

        return true;
    }
}
