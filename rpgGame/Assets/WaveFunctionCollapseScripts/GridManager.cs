using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;
//Hej Jonas
public class GridManager : MonoBehaviour
{
    public int gridX = 4;
    public int gridY = 4;
    public int gridZ = 4;


    public List<GameObject> modules = new List<GameObject>();
    public ScriptableObjectList moduleSO;
    public GameObject floorGO;
    public GameObject solidGO;
    public List<GameObject> walls = new List<GameObject>();
    public BuildNavMesh bnm;
    public bool isImproved = true;
    bool isTried = false;
    public bool enableChiseling;

    public slot[,,] grid;
    public Point[,,] pointgrid;
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

    //BlendWeights
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh tmpMesh;
    Material tmpMaterial;
    MeshFilter meshFilter;
    Bounds bounds;
    GGManager gg;

    //Point system
    public List<Point> allPoints = new List<Point>();
    Vector3 offset = new Vector3(0,0,0);

    void Start()
    {
         mL = GetComponent<MachineLearning>();
        weights = GetComponent<Weights>();
        fitness = GetComponent<FitnessFunction>();
        onLevelCreated = gameObject.GetComponent<OnLevelCreated>();
        noiseMap = GetComponent<NoiseMap>();
        chiseling = GetComponent<Chiseling>();
        bnm = GetComponent<BuildNavMesh>();
        gg = GetComponent<GGManager>();

        

        modules.AddRange(moduleSO.list);
        weights.AssignWeights(modules);
        //startBuilding();

        for (int i = 0; i < loadScreen.list[0].transform.childCount; i++)
        {
            loadScreen.list[0].transform.GetChild(i).gameObject.SetActive(true);
        }

        //Chiseling();
    }


    public void InitializePoints()
    {

        size = gridX * gridY * gridZ;
        grid = new slot[gridX, gridY, gridZ];
        pointgrid = new Point[gridX+1, gridY, gridZ+1];

        for (int i = 0; i < gridX + 1; i++)
        {
            for (int k = 0; k < gridZ + 1; k++)
            {
                Point tmpPoint = new Point(new Vector3(i * 2, 0, k * 2), i * 2 + UnityEngine.Random.Range(-0.5f, 0.5f), k * 2 + UnityEngine.Random.Range(-0.5f, 0.5f));
                tmpPoint.offsetPos = gg.points[i, k].position;
                allPoints.Add(tmpPoint);
                pointgrid[i, 0, k] = tmpPoint;
            }
        }
    }



    public void Chiseling()
    {
        int index = 0;
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    slot tmpSlot = new slot(pointgrid[i,k,j], pointgrid[i+1, k, j], pointgrid[i, k, j+1], pointgrid[i+1, k, j+1]);
                    grid[i, k, j] = tmpSlot;
                    if (enableChiseling) grid[i, k, j].isPath = true;
                    grid[i, k, j].index = index;
                    grid[i, k, j].pos = new Vector3(i, k, j);
                    index++;
                    mL.randomPool.Add(index);
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



        if (enableChiseling)
        {
            chiseling.grid = grid;
            chiseling.AssignFixedPoints();
        }
        else
        {
            startBuilding();
        }
    }

    public void startBuilding()
    {
        //initialze grid
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
                }
            }
        }
        mL.randomPooltmp.AddRange(mL.randomPool);

        /*for (int i = 0; i < gridX; i++)
        {
            grid[i, 0, gridZ - 1].posibilitySpace.Clear();
            grid[i, 0, gridZ - 1].posibilitySpace.AddRange(walls);

            grid[i, 0, 0].posibilitySpace.Clear();
            grid[i, 0, 0].posibilitySpace.AddRange(walls);
        }
        for (int j = 0; j < gridZ; j++)
        {
            grid[gridX - 1, 0, j].posibilitySpace.Clear();
            grid[gridX - 1, 0, j].posibilitySpace.AddRange(walls);

            grid[0, 0, j].posibilitySpace.Clear();
            grid[0, 0, j].posibilitySpace.AddRange(walls);
        }*/


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
                    //if(grid[i, k, j].isPath && grid[i, k, j].IsInstantiated != true)
                    {
                        size--;
                        StartCoroutine(Progress(size));
                        GameObject tmpGo = Instantiate(grid[i, k, j].posibilitySpace[0], new Vector3(i * 2, k * 2, j * 2), grid[i, k, j].posibilitySpace[0].transform.rotation, weights.moduleParents[grid[i, k, j].posibilitySpace[0].GetComponent<Modulescript>().moduleIndex].transform);
                        grid[i, k, j].instantiatedModule = tmpGo;
                        if (grid[i, k, j].isPath)
                        {
                            grid[i, k, j].instantiatedModule.GetComponent<Renderer>().material.color = Color.red;
                            grid[i, k, j].instantiatedModule.GetComponent<Modulescript>().isPath = true;
                        }
                        grid[i, k, j].IsInstantiated = true;
                        grid[i, k, j].instantiatedModule.GetComponent<Modulescript>().pos = grid[i, k, j].pos;
                        grid[i, k, j].instantiatedModule.transform.position = (grid[i, k, j].points[0].offsetPos + grid[i, k, j].points[1].offsetPos + grid[i, k, j].points[2].offsetPos + grid[i, k, j].points[3].offsetPos) / 4;
                        UpdatePointOffsets(i, k, j);
                        SetBlendWeights(grid[i, k, j].instantiatedModule, i, k, j);
                        //CreateStaticMesh(grid[i, k, j].instantiatedModule);


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

        //bnm.BuildNavMeshButton();
        onLevelCreated.ActivateEnemies();
        onLevelCreated.ActivatePlayer(20,0.5f,4);
        onLevelCreated.DeactiveLoadScreen();
        //weights.CalculateWeights();


        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {                       
                        //if (grid[i, k, j].isPath) grid[i, k, j].instantiatedModule.GetComponent<Renderer>().material = floor;    
                }
            }
        }
    }

    void SetBlendWeights(GameObject module, int i, int k, int j)
    {
        skinnedMeshRenderer = module.GetComponent<SkinnedMeshRenderer>();

        if (module.transform.rotation.eulerAngles.y == 0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(2, (grid[i, k, j].points[0].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(3, (grid[i, k, j].points[0].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(0, (grid[i, k, j].points[1].offsetX) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(1, (grid[i, k, j].points[1].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(6, (grid[i, k, j].points[2].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(7, (grid[i, k, j].points[2].offsetZ) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(4, (grid[i, k, j].points[3].offsetX) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(5, (grid[i, k, j].points[3].offsetZ) * -50);
        }
        else if (module.transform.rotation.eulerAngles.y == 90)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, (grid[i, k, j].points[0].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(1, (grid[i, k, j].points[0].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(4, (grid[i, k, j].points[1].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(5, (grid[i, k, j].points[1].offsetX) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(2, (grid[i, k, j].points[2].offsetZ) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(3, (grid[i, k, j].points[2].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(6, (grid[i, k, j].points[3].offsetZ) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(7, (grid[i, k, j].points[3].offsetX) * -50);
        }
        else if (module.transform.rotation.eulerAngles.y == 180)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(4, (grid[i, k, j].points[0].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(5, (grid[i, k, j].points[0].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(6, (grid[i, k, j].points[1].offsetX) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(7, (grid[i, k, j].points[1].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(0, (grid[i, k, j].points[2].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(1, (grid[i, k, j].points[2].offsetZ) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(2, (grid[i, k, j].points[3].offsetX) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(3, (grid[i, k, j].points[3].offsetZ) * -50);
        }
        else if (module.transform.rotation.eulerAngles.y == 270)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(6, (grid[i, k, j].points[0].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(7, (grid[i, k, j].points[0].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(2, (grid[i, k, j].points[1].offsetZ) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(3, (grid[i, k, j].points[1].offsetX) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(4, (grid[i, k, j].points[2].offsetZ) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(5, (grid[i, k, j].points[2].offsetX) * 50);
            skinnedMeshRenderer.SetBlendShapeWeight(0, (grid[i, k, j].points[3].offsetZ) * -50);
            skinnedMeshRenderer.SetBlendShapeWeight(1, (grid[i, k, j].points[3].offsetX) * -50);
        }
    }

    void CreateStaticMesh(GameObject module)
    {
        skinnedMeshRenderer = module.GetComponent<SkinnedMeshRenderer>();
        tmpMesh = new Mesh();
        meshFilter = module.GetComponent<MeshFilter>();

        skinnedMeshRenderer.BakeMesh(tmpMesh);
        bounds = meshFilter.mesh.bounds;
        bounds.Expand(new Vector3(1, 1, 1));
        tmpMesh.bounds = bounds;
        meshFilter.mesh = tmpMesh;
        tmpMaterial = skinnedMeshRenderer.material;
        Destroy(skinnedMeshRenderer);
        module.AddComponent<MeshRenderer>();
        module.GetComponent<MeshRenderer>().material = tmpMaterial;
        meshFilter.mesh.RecalculateNormals();
        module.AddComponent<MeshCollider>();
    }

    void UpdateSlotPositions()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {


                }
            }
        }
    }

    void UpdatePointOffsets(int i, int k, int j)
    {
        Vector3 tmpVec = new Vector3(0, 0, 0);

                        Vector3 offsetMiddle = (grid[i, k, j].instantiatedModule.transform.position + new Vector3(-0.5f, 0, 0.5f));
                        Vector3 positionMiddle = (grid[i, k, j].points[0].position + grid[i, k, j].points[1].position + grid[i, k, j].points[2].position + grid[i, k, j].points[3].position) / 4;

                        tmpVec = grid[i, k, j].points[0].offsetPos - (grid[i, k, j].instantiatedModule.transform.position + new Vector3(-1f, 0, -1f));
                        grid[i, k, j].points[0].offsetX = tmpVec.x;
                        grid[i, k, j].points[0].offsetZ = tmpVec.z;


                        tmpVec = grid[i, k, j].points[1].offsetPos - (grid[i, k, j].instantiatedModule.transform.position + new Vector3(1f, 0, -1f));
                        grid[i, k, j].points[1].offsetX = tmpVec.x;
                        grid[i, k, j].points[1].offsetZ = tmpVec.z;


                        tmpVec = grid[i, k, j].points[2].offsetPos - (grid[i, k, j].instantiatedModule.transform.position + new Vector3(-1f, 0, 1f));
                        grid[i, k, j].points[2].offsetX = tmpVec.x;
                        grid[i, k, j].points[2].offsetZ = tmpVec.z;


                        tmpVec = grid[i, k, j].points[3].offsetPos - (grid[i, k, j].instantiatedModule.transform.position + new Vector3(1f, 0, 1f));
                        grid[i, k, j].points[3].offsetX = tmpVec.x;
                        grid[i, k, j].points[3].offsetZ = tmpVec.z;  

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

    void OnDrawGizmos()
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
        Gizmos.color = Color.blue;

        Gizmos.color = Color.red;
        for (int i = 0; i < allPoints.Count; i++)
        {
            //Gizmos.DrawSphere(allPoints[i].position + offset, 0.1f);
            //Handles.Label(allPoints[i].position + offset, Math.Round(allPoints[i].offsetX, 2).ToString() + " and " + Math.Round(allPoints[i].offsetZ, 2).ToString());

        }

        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    Gizmos.color = Color.green;
                    //Gizmos.DrawSphere(grid[i, k, j].instantiatedModule.transform.position + new Vector3(-1f, 0, -1f), 0.1f);
                   // Gizmos.DrawSphere(grid[i, k, j].points[0].upVector, 0.1f);
                    //Gizmos.color = Color.magenta;
                    //Gizmos.DrawSphere(grid[i, k, j].instantiatedModule.transform.position + new Vector3(1f, 0, -1f), 0.1f);
                   // Gizmos.DrawSphere(grid[i, k, j].points[1].upVector, 0.1f);
                    //Gizmos.color = Color.yellow;
                    //Gizmos.DrawSphere(grid[i, k, j].instantiatedModule.transform.position + new Vector3(-1f, 0, 1f), 0.1f);
                   // Gizmos.DrawSphere(grid[i, k, j].points[2].upVector, 0.1f);
                    //Gizmos.color = Color.black;
                    //Gizmos.DrawSphere(grid[i, k, j].instantiatedModule.transform.position + new Vector3(1f, 0, 1f), 0.1f);
                   // Gizmos.DrawSphere(grid[i, k, j].points[3].upVector, 0.1f);
                    //Gizmos.color = Color.red;
                    //Gizmos.DrawSphere(grid[i, k, j].instantiatedModule.transform.position, 0.1f);





                    Gizmos.color = Color.red;
                    /*Gizmos.DrawLine(grid[i, k, j].points[0].offsetPos + offset, grid[i, k, j].points[1].offsetPos + offset);
                    Gizmos.DrawLine(grid[i, k, j].points[1].offsetPos + offset, grid[i, k, j].points[3].offsetPos + offset);
                    Gizmos.DrawLine(grid[i, k, j].points[2].offsetPos + offset, grid[i, k, j].points[3].offsetPos + offset);
                    Gizmos.DrawLine(grid[i, k, j].points[2].offsetPos + offset, grid[i, k, j].points[0].offsetPos + offset);*/
                }
            }   
        }
    }
}
