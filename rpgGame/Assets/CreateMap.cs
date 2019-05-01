using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public bool restoreMap;
    public Material floor;
    public Material path;

    void Start()
    {
        for (int i = 0; i < mapPrefab.transform.childCount; i++)
        {
            for (int j = 0; j < mapPrefab.transform.GetChild(i).transform.childCount; j++)
            {
                mapPrefab.transform.GetChild(i).transform.GetChild(j).gameObject.AddComponent<MeshCollider>();
                ;
            }
        }

        StartCoroutine(StartLate());
    }

    void Init()
    {
        for (int i = 0; i < mapPrefab.transform.childCount; i++)
        {
            for (int j = 0; j < mapPrefab.transform.GetChild(i).transform.childCount; j++)
            {
                if (restoreMap) UnSkew(mapPrefab.transform.GetChild(i).transform.GetChild(j).gameObject);
                CreateStaticMesh(mapPrefab.transform.GetChild(i).transform.GetChild(j).gameObject);
            }
        }
    }

    private void Update()
    {

    }

    IEnumerator StartLate()
    {
        Init();
        yield return new WaitForSeconds(0.1f);
    }

    void CreateStaticMesh(GameObject module)
    {
        /*skinnedMeshRenderer = module.GetComponent<SkinnedMeshRenderer>();
        tmpMesh = new Mesh();
        meshFilter = module.GetComponent<MeshFilter>();
        meshFilter.mesh.RecalculateNormals();
        skinnedMeshRenderer.sharedMesh.RecalculateNormals();
        module.AddComponent<MeshCollider>();*/


        skinnedMeshRenderer = module.GetComponent<SkinnedMeshRenderer>();
        tmpMesh = new Mesh();
        meshFilter = module.GetComponent<MeshFilter>();

        skinnedMeshRenderer.BakeMesh(tmpMesh);
        bounds = meshFilter.mesh.bounds;
        bounds.Expand(new Vector3(5, 5, 5));
        tmpMesh.bounds = bounds;
        meshFilter.mesh = tmpMesh;
        tmpMaterial = skinnedMeshRenderer.material;
        Destroy(skinnedMeshRenderer);
        module.AddComponent<MeshRenderer>();
        module.GetComponent<MeshRenderer>().material = tmpMaterial;
        meshFilter.mesh.RecalculateNormals();
        module.AddComponent<MeshCollider>();
        module.GetComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;
    }

    void UnSkew(GameObject module)
    {
        module.transform.position = module.GetComponent<Modulescript>().pos * 2;
        for (int i = 0; i < 8; i++)
        {
            module.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, 0);
        }
    }

    public void RestoreMaterial() 
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

    void CombineMeshes(Transform module)
    {
        MeshFilter[] meshFilters = module.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        module.gameObject.AddComponent<MeshFilter>();
        module.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        module.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        module.gameObject.AddComponent<MeshRenderer>();
        module.transform.gameObject.SetActive(true);
    }

    /*void SaveAsset(Transform module, string index)
    {
        MeshFilter mf = module.GetComponent<MeshFilter>();
        if (mf)
        {
            var savePath = "Assets/SavedMeshes/" + "TestMesh" + index + ".asset";
            Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(mf.mesh, savePath);
        }
    }*/
}
