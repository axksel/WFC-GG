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


    public List<GameObject> modules = new List<GameObject>();
    public ScriptableObjectList moduleSO;
    public GameObject floorGO;
    public BuildNavMesh bnm;
    public bool isImproved = true;
    bool isTried = false;

    public slot[,,] grid;
    public int size;
    public GameObjectList loadScreen;
    public GameObjectList progressBar;
    public Material floor;
    Color[,] noiseValues;

    FitnessFunction fitness;
    Weights weights;
    NoiseMap noiseMap;
    Chiseling chiseling;
    OnLevelCreated onLevelCreated;
    MachineLearning mL;

    void Start()
    {
        mL = GetComponent<MachineLearning>();
        weights = GetComponent<Weights>();
        fitness = GetComponent<FitnessFunction>();
        onLevelCreated = gameObject.GetComponent<OnLevelCreated>();
        noiseMap = GetComponent<NoiseMap>();
        chiseling = GetComponent<Chiseling>();
        bnm = GetComponent<BuildNavMesh>();

        size = gridX * gridY * gridZ;
        grid = new slot[gridX, gridY, gridZ];

        modules.AddRange(moduleSO.list);
        weights.AssignWeights(modules);
        //startBuilding();

        for (int i = 0; i < loadScreen.list[0].transform.childCount; i++)
        {
            loadScreen.list[0].transform.GetChild(i).gameObject.SetActive(true);
        }

        Chiseling();
    }

    public void Chiseling()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    grid[i, k, j] = new slot();
                    grid[i, k, j].posibilitySpace.Add(floorGO);
                    grid[i, k, j].isPath = true;
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

        chiseling.grid = grid;
        chiseling.AssignFixedPoints();
    }

    public void startBuilding()
    {
        //initialze grid
        int index = 0;
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    grid[i, k, j].posibilitySpace.Clear();
                    if(grid[i, k, j].isPath)
                        grid[i, k, j].posibilitySpace.Add(floorGO);
                    else
                        grid[i, k, j].posibilitySpace.AddRange(modules);                  
                    mL.randomPool.Add(index);
                    grid[i, k, j].index = index;
                    index++;
                }
            }
        }
        mL.randomPooltmp.AddRange(mL.randomPool);



        mL.grid = grid;
        noiseMap.grid = grid;
        noiseMap.InitNoiseWeights(gridX, gridY, gridZ);

        StartCoroutine(IterateAndCollapse());       
    }

    public void Build()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    if (grid[i, k, j].posibilitySpace.Count == 1 && grid[i, k, j].IsInstantiated != true)
               
                    {
                        size--;
                        StartCoroutine(Progress(size));
                        GameObject tmpGo = Instantiate(grid[i, k, j].posibilitySpace[0], grid[i, k, j].posibilitySpace[0].transform.position + new Vector3(i * 2, k * 2, j * 2), grid[i, k, j].posibilitySpace[0].transform.rotation, weights.moduleParents[grid[i, k, j].posibilitySpace[0].GetComponent<Modulescript>().moduleIndex].transform);
                        //tmpGo.GetComponentInChildren<Renderer>().material.color = noiseMap.pixelValues[i, j];
                        grid[i, k, j].instantiatedModule = tmpGo;
                        grid[i, k, j].IsInstantiated = true;
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
        grid[iTmp, kTmp, jTmp].SetNeighboursTrue();
        Build();
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
                    if (grid[i, k, j].shouldBeIterated) { 

                        gridTmp = grid[i, k, j].posibilitySpace.Count;
                        //grid[i, k, j].RemoveZeroWeightModules();
                        grid[i, k, j].Iterate();

                        if (grid[i, k, j].Contradiction() == true)
                        {
                            mL.UnCollapseWithPosition(6, 1, 6, i, k, j);
                        }

                        if (gridTmp != grid[i, k, j].posibilitySpace.Count)
                        {
                            grid[i, k, j].SetNeighboursTrue();
                            shouldIterate = true;
                        }
                        else
                        {
                            grid[i, k, j].shouldBeIterated = false;
                        }
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
            Collapse();
            yield return null;
            if (size > 0)
            {
                StartCoroutine(IterateAndCollapse());
            }
            else
            {

                LevelGenerationDone();
                yield return null;

            }
        }
    }

    private void LevelGenerationDone()
    {
        onLevelCreated.DeactivateEnemies();
        bnm.BuildNavMeshButton();
        onLevelCreated.ActivateEnemies();
        onLevelCreated.ActivatePlayer(20,0.5f,4);
        onLevelCreated.DeactiveLoadScreen();
        GetComponent<AudioSource>().Play();
        //weights.CalculateWeights();
        /*for (int j = 20; j < 26; j++)
        {
            for (int i = 0; i < weights.moduleParents[j].transform.childCount; i++)
            {
                weights.moduleParents[j].transform.GetChild(i).GetComponent<Renderer>().material = floor;
            }
        }*/
    }


    public IEnumerator Progress(int progress)
    {
        float pct = MathFunctions.Map(progress, (gridX * gridZ), 0, 0, 1);
        loadScreen.list[0].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt((pct * 100)).ToString() + " %";
        progressBar.list[0].GetComponent<Image>().fillAmount = pct;
        if (Mathf.RoundToInt((pct * 100)) == 100)
        {
            loadScreen.list[0].transform.GetChild(1).gameObject.SetActive(false);
            loadScreen.list[0].transform.GetChild(2).gameObject.SetActive(false);
            loadScreen.list[0].transform.GetChild(4).gameObject.SetActive(false);
            loadScreen.list[0].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Generating NavMesh";
        }
        else
        {
            loadScreen.list[0].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Generating Level";
        }
        yield return null;
    }

    /*void OnDrawGizmos()
{
    for (int i = 0; i < gridX; i++)
    {
        for (int k = 0; k < gridY; k++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                try
                {
                    Handles.Label(new Vector3(i*2, k*2, j*2) , grid[i, k, j].posibilitySpace.Count.ToString());

                }
                catch (System.NullReferenceException) { }
            }
        }
    }
}*/
}
