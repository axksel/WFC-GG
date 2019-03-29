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
    int size = 0;
    int progress;
    int fixedSlotsVisited;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
        size = gridManager.gridX * gridManager.gridZ;
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
            progress++;
            allPoints.RemoveAt(itemToRemove);
            StartCoroutine(gridManager.Progress(size - (int)(0.1f * progress)));
        }
        else
        {
            tries++;
            allPoints[itemToRemove].isPath = true;
        }

        if (tries > 100)
        {
            gridManager.startBuilding();
            //gridManager.Build();
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
        if (slot.isFixed)
        {
            fixedSlotsVisited++;
        }
        if (fixedSlotsVisited == fixedPoints.Count)
        {
            return;
        }
        if (slot.isPath && !slot.isVisited)
            {
            slot.isVisited = true;
            if (slot.z < gridManager.gridZ - 1)
                Visit(slot.neighbours[0]);
            if (slot.x < gridManager.gridX - 1)
                Visit(slot.neighbours[1]);
            if (slot.z > 0)
                Visit(slot.neighbours[2]);
            if (slot.x > 0)
                Visit(slot.neighbours[3]);
        }
    }

    private void Reset()
    {
        for (int i = 0; i < allPoints.Count; i++)
        {
            allPoints[i].isVisited = false;
        }
    }

    private bool CheckFixedPoints()
    {
        fixedSlotsVisited = 0;
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
