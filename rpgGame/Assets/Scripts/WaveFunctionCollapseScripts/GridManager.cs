using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;


public class GridManager : MonoBehaviour
{
    [HideInInspector]
    public int gridX = 4;
    public int gridY = 4;
    [HideInInspector]
    public int gridZ = 4;

    [HideInInspector]
    public List<GameObject> modules = new List<GameObject>();
    public ScriptableObjectList moduleSO;
    [HideInInspector]
    public BuildNavMesh bnm;
    bool isTried = false;
    

    public slot[,,] grid;
    public Point[,,] pointgrid;
    [HideInInspector]
    public int size;
    Color[,] noiseValues;

    FitnessFunction fitness;
    Weights weights;
    
    Chiseling chiseling;
    OnLevelCreated onLevelCreated;
    

    //BlendWeights
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh tmpMesh;
    Material[] tmpMaterial;
    MeshFilter meshFilter;
    Bounds bounds;
    GGManager gg;

    //Point system
    public List<Point> allPoints = new List<Point>();
    Vector3 offset = new Vector3(0,0,0);

    void Start()
    {
        
        weights = GetComponent<Weights>();
        weights.moduleSO = moduleSO;
        fitness = GetComponent<FitnessFunction>();
        onLevelCreated = gameObject.GetComponent<OnLevelCreated>();
        
        chiseling = GetComponent<Chiseling>();
        bnm = GetComponent<BuildNavMesh>();
        gg = GetComponent<GGManager>();

        modules.AddRange(moduleSO.list);
        weights.AssignWeights(modules);
 
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
                Point tmpPoint = new Point(new Vector3(i * 2, 0, k * 2), i * 2 , k * 2 );
                tmpPoint.offsetPos = gg.points[i, k].position;
                allPoints.Add(tmpPoint);
                pointgrid[i, 0, k] = tmpPoint;
            }
        }
    }


    public void InitializeSlots()
    {
        int index = 0;
        for (int i = 0; i < gridX; i++)
        {
            for (int k = 0; k < gridY; k++)
            {
                for (int j = 0; j < gridZ; j++)
                {
                    slot tmpSlot = new slot(pointgrid[i,0,j], pointgrid[i+1, 0, j], pointgrid[i, 0, j+1], pointgrid[i+1, 0, j+1]);
                    grid[i, k, j] = tmpSlot;
                   
                    grid[i, k, j].index = index;
                    grid[i, k, j].pos = new Vector3(i, k, j);
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
        
            startBuilding();
        
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
                    grid[i, k, j].posibilitySpace.AddRange(modules);                  
                }
            }
        }
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
                     
                        GameObject tmpGo = Instantiate(grid[i, k, j].posibilitySpace[0], new Vector3(i * 2, k * 2, j * 2), grid[i, k, j].posibilitySpace[0].transform.rotation, weights.moduleParents[grid[i, k, j].posibilitySpace[0].GetComponent<Modulescript>().moduleIndex].transform);
                        grid[i, k, j].instantiatedModule = tmpGo;                 
                        grid[i, k, j].IsInstantiated = true;
                        grid[i, k, j].instantiatedModule.GetComponent<Modulescript>().pos = grid[i, k, j].pos;
                        grid[i, k, j].instantiatedModule.transform.position = grid[i, k, j].posibilitySpace[0].transform.position + new Vector3(0,k*15,0)+ ((grid[i, k, j].points[0].offsetPos + grid[i, k, j].points[1].offsetPos + grid[i, k, j].points[2].offsetPos + grid[i, k, j].points[3].offsetPos) / 4);
                        UpdatePointOffsets(i, k, j);
                        if (grid[i, k, j].instantiatedModule.GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            
                            SetBlendWeights(grid[i, k, j].instantiatedModule, i, k, j);
                            CreateStaticMesh(grid[i, k, j].instantiatedModule);
                        }
                       


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
                        grid[i, k, j].Iterate();

                        if (grid[i, k, j].Contradiction() == true)
                        {
                           // mL.UnCollapseWithPosition(6, 1, 6, i, k, j);
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

              
                yield return null;

            }
        }
    }

  

    void SetBlendWeights(GameObject module, int i, int k, int j)
    {
        skinnedMeshRenderer = module.GetComponent<SkinnedMeshRenderer>();

        if (module.transform.rotation.eulerAngles.y == 180)
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
        else if (module.transform.rotation.eulerAngles.y == 270)
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
        else if (module.transform.rotation.eulerAngles.y == 0)
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
        else if (module.transform.rotation.eulerAngles.y == 90)
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
        module.AddComponent<MeshFilter>();
        meshFilter = module.GetComponent<MeshFilter>();

        skinnedMeshRenderer.BakeMesh(tmpMesh);
        bounds = meshFilter.mesh.bounds;
        bounds.Expand(new Vector3(1, 1, 1));
        tmpMesh.bounds = bounds;
        meshFilter.mesh = tmpMesh;
        tmpMaterial = skinnedMeshRenderer.materials;
        Destroy(skinnedMeshRenderer);
        module.AddComponent<MeshRenderer>();
        module.GetComponent<MeshRenderer>().materials = tmpMaterial;
        meshFilter.mesh.RecalculateNormals();
        module.AddComponent<MeshCollider>();
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


  


}
