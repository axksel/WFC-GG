using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public GameObject mapPrefab;

    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh tmpMesh;
    Material tmpMaterial;
    MeshFilter meshFilter;
    Bounds bounds;
    GGManager gg;
    public bool isSkewed;

    void Start()
    {
        for (int i = 0; i < mapPrefab.transform.childCount; i++)
        {
            for (int j = 0; j < mapPrefab.transform.GetChild(i).transform.childCount; j++)
            {
                if (!isSkewed) UnSkew(mapPrefab.transform.GetChild(i).transform.GetChild(j).gameObject);
                CreateStaticMesh(mapPrefab.transform.GetChild(i).transform.GetChild(j).gameObject);
            }
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

    void UnSkew(GameObject module)
    {
        module.transform.position = module.GetComponent<Modulescript>().pos * 2;
        for (int i = 0; i < 8; i++)
        {
            module.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, 0);
        }
    } 

    void RestoreMaterial() //set size of for loop TODO
    {
        for (int i = 0; i < 1; i++)
        {
            for (int k = 0; k < 1; k++)
            {
                for (int j = 0; j < 1; j++)
                {
                    //if (grid[i, k, j].isPath) grid[i, k, j].instantiatedModule.GetComponent<Renderer>().material = floor;    
                }
            }
        }
    }
}
