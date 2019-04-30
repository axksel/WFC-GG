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
    public Material floor;
    public Material path;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RestoreMaterial();
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

    void RestoreMaterial() 
    {
        for (int i = 0; i < mapPrefab.transform.childCount; i++)
        {
            for (int j = 0; j < mapPrefab.transform.GetChild(i).transform.childCount; j++)
            {
                    if(mapPrefab.transform.GetChild(i).transform.GetChild(j).gameObject.GetComponent<Modulescript>().moduleType == Modulescript.ModuleType.Floor)
                    mapPrefab.transform.GetChild(i).transform.GetChild(j).GetComponent<Renderer>().material = floor;
            }
        }
    }
}
