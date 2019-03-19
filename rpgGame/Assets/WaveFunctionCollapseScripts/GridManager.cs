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
    public BuildNavMesh bnm;
    public bool isImproved = true;
    bool isTried = false;

    public slot[,,] grid;
    public int size;
    public GameObjectList loadScreen;
    public GameObjectList progressBar;
    private OnLevelCreated onLevelCreated;
    public MachineLearning mL;
    public Material floor;
    Color[,] noiseValues;

    FitnessFunction fitness;
    Weights weights;
    NoiseMap noiseMap;

    void Start()
    {
        mL = GetComponent<MachineLearning>();
        weights = GetComponent<Weights>();
        fitness = GetComponent<FitnessFunction>();
        onLevelCreated = gameObject.GetComponent<OnLevelCreated>();
        noiseMap = GetComponent<NoiseMap>();

        modules.AddRange(moduleSO.list);
        weights.AssignWeights(modules);
        startBuilding();

        for (int i = 0; i < loadScreen.list[0].transform.childCount; i++)
        {
            loadScreen.list[0].transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void startBuilding()
    {
        bnm = GetComponent<BuildNavMesh>();
        size = gridX * gridY * gridZ;
                
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
                    mL.randomPool.Add(index);
                    grid[i, k, j].index = index;
                    index++;
                }
            }
        }
        mL.randomPooltmp.AddRange(mL.randomPool);

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
        /*for (int i = 0; i < gridX; i++)
        {
            grid[i, 0, 0].collapse(19);
            grid[i, 0, gridZ - 1].collapse(19);
        }
      
        for (int i = 1; i < gridZ-1; i++)
        {
            grid[0, 0, i].collapse(19);
            grid[gridX-1, 0, i].collapse(19);
        }

        grid[gridX / 2, 0, 1].collapse(15);
        grid[gridX / 2, 0, gridZ - 2].collapse(17);*/

        mL.grid = grid;
        noiseMap.grid = grid;
        noiseMap.InitNoiseWeights(gridX, gridY, gridZ);

        StartCoroutine(IterateAndCollapse());       
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
                        StartCoroutine(Progress(size));
                        size--;
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
           // else if (!CheckNumberOfSpecificModules())
                  else if (true)
            {
                LevelGenerationDone();
                yield return null;
            }
            else if (isImproved)
            {
                
                mL.UnCollapse(2,1,2);
                size = size + 4;
                StartCoroutine(IterateAndCollapse());
                yield return null;
            }
            else
            {
                yield return null;
                mL.Revert(2,1,2);
                size = size + 4;
                isTried = true;
                StartCoroutine(IterateAndCollapse());
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
        for (int j = 20; j < 26; j++)
        {
            for (int i = 0; i < weights.moduleParents[j].transform.childCount; i++)
            {
                weights.moduleParents[j].transform.GetChild(i).GetComponent<Renderer>().material = floor;
            }

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

}
