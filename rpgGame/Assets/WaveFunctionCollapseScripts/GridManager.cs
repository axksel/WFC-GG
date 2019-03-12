using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    public int gridX = 4;
    public int gridY = 4;
    public int gridZ = 4;

    public int xRandom;
    public int yRandom;
    public int zRandom;

    public List<GameObject> modules = new List<GameObject>();
    List<int> randomPooltmp = new List<int>();
    List<int> randomPool = new List<int>();
    public ScriptableObjectList moduleSO;
    public BuildNavMesh bnm;
    public bool isImproved = true;
    bool isTried = false;

    public slot[,,] grid;
    public slot[,,] savedMiniGrid;
    int size;
    public GameObjectList loadScreen;
    public GameObjectList progressBar;
    private OnLevelCreated onLevelCreated;
    FitnessFunction fitness;
    GameObject[] moduleParents;
    public FloatList weights;
    public float[] tmpWeights;


    void Start()
    {
        
        moduleParents = new GameObject[moduleSO.list.Count];
       tmpWeights = new float[System.Enum.GetValues(typeof(Modulescript.ModuleType)).Length];

        for (int i = 0; i < moduleSO.list.Count; i++)
        {
            //weights.list.Add(moduleSO.list[i].GetComponent<Modulescript>().weight);


            tmpWeights[(int)moduleSO.list[i].GetComponent<Modulescript>().moduleType] += weights.list[i];
            //moduleSO.list[i].GetComponent<Modulescript>().weight = weights.list[i]; 
            moduleParents[i] = Instantiate(new GameObject(moduleSO.list[i].name), transform.position, Quaternion.identity, gameObject.transform);
            moduleSO.list[i].GetComponent<Modulescript>().moduleIndex = i;
        }

        for (int i = 0; i < moduleSO.list.Count; i++)
        {
            moduleSO.list[i].GetComponent<Modulescript>().weight = tmpWeights[(int)moduleSO.list[i].GetComponent<Modulescript>().moduleType];
        }

        fitness = GetComponent<FitnessFunction>();
        onLevelCreated = gameObject.GetComponent<OnLevelCreated>();
        StartCoroutine(startBuilding());

        for (int i = 0; i < loadScreen.list[0].transform.childCount; i++)
        {
            loadScreen.list[0].transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public IEnumerator startBuilding()
    {
        bnm = GetComponent<BuildNavMesh>();
        size = gridX * gridY * gridZ;
        modules = moduleSO.list;
        grid = new slot[gridX, gridY, gridZ];
        savedMiniGrid = new slot[2, 1, 2];

        for (int i = 0; i < 2; i++)
        {
            for (int k = 0; k < 1; k++)
            {
                for (int j = 0; j < 2; j++)
                {
                    savedMiniGrid[i, k, j] = new slot();


                }
            }
        }


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
                    randomPool.Add(index);
                    grid[i, k, j].index = index;
                    index++;
                }
            }
        }
        randomPooltmp.AddRange(randomPool);
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


                    if (grid[i, k, j].posibilitySpace.Count == 1 && grid[i, k, j].IsInstantiated != true)
                    {
                        StartCoroutine(Progress(size));
                        size--;
                        GameObject tmpGo = Instantiate(grid[i, k, j].posibilitySpace[0], grid[i, k, j].posibilitySpace[0].transform.position + new Vector3(i * 2, k * 2, j * 2), grid[i, k, j].posibilitySpace[0].transform.rotation, moduleParents[grid[i, k, j].posibilitySpace[0].GetComponent<Modulescript>().moduleIndex].transform);
                        grid[i, k, j].instantiatedModule = tmpGo;
                        grid[i, k, j].IsInstantiated = true;
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
                    if (count > grid[i, k, j].posibilitySpace.Count && grid[i, k, j].posibilitySpace.Count > 1)
                    {
                        count = grid[i, k, j].posibilitySpace.Count;
                        iTmp = i;
                        kTmp = k;
                        jTmp = j;
                    }
                }
            }
        }
        grid[iTmp, kTmp, jTmp].collapse();
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
                    gridTmp = grid[i, k, j].posibilitySpace.Count;
                    grid[i, k, j].Iterate();

                    if (gridTmp != grid[i, k, j].posibilitySpace.Count)
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
            else if (!CheckNumberOfSpecificModules())
            {
                LevelGenerationDone();
                yield return null;
            }
            else if (isImproved)
            {

                UnCollapse();
                size = size + 4;
                StartCoroutine(IterateAndCollapse());
                yield return null;
            }
            else
            {

                yield return null;
                Revert();
                size = size + 4;
                isTried = true;
                StartCoroutine(IterateAndCollapse());

            }

        }
    }

    public void UnCollapse()
    {
        if (randomPool.Count != 0)
        {
            int indexRandom = Random.Range(0, randomPool.Count);
            xRandom = Mathf.Clamp(randomPool[indexRandom] % gridZ, 0, gridX - 2);
            yRandom = 0;
            zRandom = Mathf.Clamp(randomPool[indexRandom] / gridX, 0, gridZ - 2);
            randomPool.RemoveAt(indexRandom);
        }
        else
        {
            randomPool.AddRange(randomPooltmp);

        }


        savedMiniGrid[0, 0, 0].posibilitySpace.Clear();
        savedMiniGrid[0, 0, 0].posibilitySpace.Add(grid[xRandom, yRandom, zRandom].posibilitySpace[0]);
        grid[xRandom, yRandom, zRandom].posibilitySpace.Clear();
        grid[xRandom, yRandom, zRandom].posibilitySpace.AddRange(modules);
        Destroy(grid[xRandom, yRandom, zRandom].instantiatedModule);
        grid[xRandom, yRandom, zRandom].IsInstantiated = false;

        savedMiniGrid[1, 0, 0].posibilitySpace.Clear();
        savedMiniGrid[1, 0, 0].posibilitySpace.Add(grid[xRandom + 1, yRandom, zRandom].posibilitySpace[0]);
        grid[xRandom + 1, yRandom, zRandom].posibilitySpace.Clear();
        grid[xRandom + 1, yRandom, zRandom].posibilitySpace.AddRange(modules);
        Destroy(grid[xRandom + 1, yRandom, zRandom].instantiatedModule);
        grid[xRandom + 1, yRandom, zRandom].IsInstantiated = false;

        savedMiniGrid[0, 0, 1].posibilitySpace.Clear();
        savedMiniGrid[0, 0, 1].posibilitySpace.Add(grid[xRandom, yRandom, zRandom + 1].posibilitySpace[0]);
        grid[xRandom, yRandom, zRandom + 1].posibilitySpace.Clear();
        grid[xRandom, yRandom, zRandom + 1].posibilitySpace.AddRange(modules);
        Destroy(grid[xRandom, yRandom, zRandom + 1].instantiatedModule);
        grid[xRandom, yRandom, zRandom + 1].IsInstantiated = false;

        savedMiniGrid[1, 0, 1].posibilitySpace.Clear();
        savedMiniGrid[1, 0, 1].posibilitySpace.Add(grid[xRandom + 1, yRandom, zRandom + 1].posibilitySpace[0]);
        grid[xRandom + 1, yRandom, zRandom + 1].posibilitySpace.Clear();
        grid[xRandom + 1, yRandom, zRandom + 1].posibilitySpace.AddRange(modules);
        Destroy(grid[xRandom + 1, yRandom, zRandom + 1].instantiatedModule);
        grid[xRandom + 1, yRandom, zRandom + 1].IsInstantiated = false;

    }

    public void Revert()
    {
        grid[xRandom, yRandom, zRandom].posibilitySpace.Clear();
        grid[xRandom, yRandom, zRandom].posibilitySpace.Add(savedMiniGrid[0, 0, 0].posibilitySpace[0]);
        grid[xRandom, yRandom, zRandom].IsInstantiated = false;
        Destroy(grid[xRandom, yRandom, zRandom].instantiatedModule);

        grid[xRandom + 1, yRandom, zRandom].posibilitySpace.Clear();
        grid[xRandom + 1, yRandom, zRandom].posibilitySpace.Add(savedMiniGrid[1, 0, 0].posibilitySpace[0]);
        grid[xRandom + 1, yRandom, zRandom].IsInstantiated = false;
        Destroy(grid[xRandom + 1, yRandom, zRandom].instantiatedModule);

        grid[xRandom, yRandom, zRandom + 1].posibilitySpace.Clear();
        grid[xRandom, yRandom, zRandom + 1].posibilitySpace.Add(savedMiniGrid[0, 0, 1].posibilitySpace[0]);
        grid[xRandom, yRandom, zRandom + 1].IsInstantiated = false;
        Destroy(grid[xRandom, yRandom, zRandom + 1].instantiatedModule);

        grid[xRandom + 1, yRandom, zRandom + 1].posibilitySpace.Clear();
        grid[xRandom + 1, yRandom, zRandom + 1].posibilitySpace.Add(savedMiniGrid[1, 0, 1].posibilitySpace[0]);
        grid[xRandom + 1, yRandom, zRandom + 1].IsInstantiated = false;
        Destroy(grid[xRandom + 1, yRandom, zRandom + 1].instantiatedModule);

    }

    private void LevelGenerationDone()
    {

        onLevelCreated.DeactivateEnemies();
        bnm.BuildNavMeshButton();
        onLevelCreated.ActivateEnemies();
        onLevelCreated.ActivatePlayer();
        onLevelCreated.DeactiveLoadScreen();


        for (int i = 0; i < moduleParents.Length; i++)
        {

            moduleSO.list[i].GetComponent<Modulescript>().weight = moduleParents[i].transform.childCount;
            weights.list[i] = moduleParents[i].transform.childCount;
            Debug.Log("Count: " + moduleParents[i].name + ": " + moduleParents[i].transform.childCount);

        }

    }

    public bool CheckNumberOfSpecificModules()
    {
        int FitnessCount = 0;
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    if (grid[i, k, j].posibilitySpace[0].gameObject.name.Equals("doorOut0") || grid[i, k, j].posibilitySpace[0].gameObject.name.Equals("doorOut1")
                        || grid[i, k, j].posibilitySpace[0].gameObject.name.Equals("doorOut2") || grid[i, k, j].posibilitySpace[0].gameObject.name.Equals("doorOut3"))
                    {
                        FitnessCount++;
                    }
                }
            }
        }

        isImproved = fitness.IsImproved(FitnessCount);
        if (isTried)
        {
            isImproved = true;
            isTried = false;
        }
        return fitness.CalculateDoorFitness(FitnessCount);
    }

    IEnumerator Progress(int progress)
    {
        float pct = MathFunctions.Map(progress, (gridX * gridZ), 0, 0, 1);
        loadScreen.list[0].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt((pct * 100)).ToString() + " %";
        progressBar.list[0].GetComponent<Image>().fillAmount = pct;
        yield return null;
    }




}
