using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public int gridX = 4;
    public int gridY = 4;
    public int gridZ = 4;

    public List<GameObject> modules = new List<GameObject>();
    public ScriptableObjectList moduleSO;
    public BuildNavMesh bnm;

    public slot[,,] grid;
    int size;
    public GameObjectList progressBar;
    private OnLevelCreated onLevelCreated;

    void Start()
    {
        onLevelCreated = gameObject.GetComponent<OnLevelCreated>();
        StartCoroutine(startBuilding());
    }

    public IEnumerator startBuilding()
    {
        bnm = GetComponent<BuildNavMesh>();
        size = gridX * gridY * gridZ;
        modules = moduleSO.list;
        grid = new slot[gridX, gridY, gridZ];
        //initialze grid
        int index = 0;
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {


                    grid[i, k, j] = new slot();
                    grid[i, k, j].posibilitySpace.AddRange(modules);

                    grid[i, k, j].index = index;
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
                        grid[i, k, j].neighbours[0] = grid[i, k, j + 1];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k, j].neighbours[1] = grid[i + 1, k, j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k, j].neighbours[2] = grid[i, k, j - 1];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k, j].neighbours[3] = grid[i - 1, k, j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k, j].neighbours[4] = grid[i, k + 1, j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                    try
                    {
                        grid[i, k, j].neighbours[5] = grid[i, k - 1, j];
                    }
                    catch (System.IndexOutOfRangeException) { }
                }
            }
        }


        grid[5, 0, 0].collapse(14);
        StartCoroutine(IterateAndCollapse());
        yield return null;
    }

    /*void OnDrawGizmos()
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
                        Handles.Label(new Vector3(i*2, k*2, j*2) , grid[i, k, j].posibilitySpace.Count.ToString());
                        index++;
                    }
                    catch (System.NullReferenceException) { }
                }
            }
        }

    }*/

    public IEnumerator Build()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {


                    if (grid[i, k,j].posibilitySpace.Count == 1 && grid[i, k, j].collapsed != true)
                    {
                        StartCoroutine(Progress(size));
                        size--;
                        Instantiate(grid[i, k, j].posibilitySpace[0], grid[i, k, j].posibilitySpace[0].transform.position + new Vector3(i * 2, k * 2, j * 2), grid[i, k, j].posibilitySpace[0].transform.rotation, transform);
                        grid[i, k, j].collapsed = true;
                    }
                }
            }
        }
        yield return null;
    }

    public IEnumerator Collapse()
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
        StartCoroutine(Build());
        yield return null;
    }

    public IEnumerator Iterate()
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
        StartCoroutine(Build());
        yield return null;
    }


    public IEnumerator IterateAndCollapse()
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

            yield return null;
            StartCoroutine(IterateAndCollapse());

        }
        else
        {
            StartCoroutine(Collapse());
            if (size > 0)
            {
                StartCoroutine(IterateAndCollapse());
                yield return null;
            }
            else
            {
                onLevelCreated.DeactivateEnemies();
                bnm.BuildNavMeshButton();
                onLevelCreated.ActivateEnemies();
                onLevelCreated.ActivatePlayer();
                yield return null;
            }
        }
    }

    IEnumerator Progress(int progress)
    {
        progressBar.list[0].GetComponent<Image>().fillAmount = map(progress, (gridX * gridZ), 0, 0, 1);
        Canvas.ForceUpdateCanvases();
        yield return null;    
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    #if UNITY_EDITOR
    public void FindNewNeighbours()
    {
        for (int i = 0; i < moduleSO.list.Count; i++)
        {
            moduleSO.list[i].GetComponent<Modulescript>().UpdateNeigboursInANewWay();
        }

    }
#endif
}
