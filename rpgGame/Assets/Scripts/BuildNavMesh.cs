using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class BuildNavMesh : MonoBehaviour
{

    public NavMeshSurface navmesh;
    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponent<NavMeshSurface>();

        navmesh.BuildNavMesh();
        
        
    }

    public void BuildNavMeshButton()
    {
        navmesh.BuildNavMesh();
    }

  
}
