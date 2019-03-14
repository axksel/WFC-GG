using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Modulescript : MonoBehaviour
{
    public enum direction {North= 0, East=1,South=2,West=3};


    public int[] neighbours = new int[6];
    public bool[] flipped = new bool[6];
    public bool[] symmetrical = new bool[6];

    public List<GameObject> neighbourNorth = new List<GameObject>();
    public List<GameObject> neighbourEast = new List<GameObject>();
    public List<GameObject> neighbourSouth = new List<GameObject>();
    public List<GameObject> neighbourWest = new List<GameObject>();
    public List<GameObject> neighbourUp = new List<GameObject>();
    public List<GameObject> neighbourDown = new List<GameObject>();

    public List<GameObject> modules = new List<GameObject>();

    public ScriptableObjectList moduleSO;

    
    public float weight;
    
    public int moduleIndex;

    public enum ModuleType { Corner, InverseCorner, Wall, Door, Floor, FloorPillar, FloorWithEnemy}

    public ModuleType moduleType;

#if UNITY_EDITOR
    public void UpdateNeigboursInANewWay()
    {
        neighbourNorth.Clear();
        neighbourEast.Clear();
        neighbourSouth.Clear();
        neighbourWest.Clear();
        neighbourUp.Clear();
        neighbourDown.Clear();
        Undo.RecordObject(gameObject.GetComponent<Modulescript>(), "Update Neighbours");

        for (int k = 0; k < moduleSO.list.Count; k++)
        {

            if (CheckNeighbour(neighbours[0], moduleSO.list[k].GetComponent<Modulescript>().neighbours[2],symmetrical[0],flipped[0], moduleSO.list[k].GetComponent<Modulescript>().flipped[2]))
            {  
                neighbourNorth.Add(moduleSO.list[k]);
            }
            if (CheckNeighbour(neighbours[1], moduleSO.list[k].GetComponent<Modulescript>().neighbours[3],symmetrical[1],flipped[1], moduleSO.list[k].GetComponent<Modulescript>().flipped[3]))
            {
                neighbourEast.Add(moduleSO.list[k]);
            }
            if (CheckNeighbour(neighbours[2], moduleSO.list[k].GetComponent<Modulescript>().neighbours[0], symmetrical[2], flipped[2], moduleSO.list[k].GetComponent<Modulescript>().flipped[0]))
            {
                neighbourSouth.Add(moduleSO.list[k]);
            }
            if (CheckNeighbour(neighbours[3], moduleSO.list[k].GetComponent<Modulescript>().neighbours[1], symmetrical[3], flipped[3], moduleSO.list[k].GetComponent<Modulescript>().flipped[1]))
            {
                neighbourWest.Add(moduleSO.list[k]);
            }
            if (CheckNeighbour(neighbours[4], moduleSO.list[k].GetComponent<Modulescript>().neighbours[5], symmetrical[4], flipped[4], moduleSO.list[k].GetComponent<Modulescript>().flipped[5]))
            {
                neighbourUp.Add(moduleSO.list[k]);
            }
            if (CheckNeighbour(neighbours[5], moduleSO.list[k].GetComponent<Modulescript>().neighbours[4], symmetrical[5], flipped[5], moduleSO.list[k].GetComponent<Modulescript>().flipped[4]))
            {
                neighbourDown.Add(moduleSO.list[k]);
            }
        }
        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject.GetComponent<Modulescript>());
    }




public bool CheckNeighbour(int value1, int value2, bool sym,bool flipped1,bool flipped2)
{

    if (value1 == value2)
    {
            if (sym)
            {
                if(flipped1 != flipped2)
                {
                    return true;
                }
                return false;
            }
        return true;
    }
    return false;
}

public void UpdateNeighbours()
    {
        neighbourWest.Clear();
        neighbourEast.Clear();
        neighbourSouth.Clear();
        neighbourNorth.Clear();
        neighbourUp.Clear();
        neighbourDown.Clear();

        Undo.RecordObject(gameObject.GetComponent<Modulescript>(), "Update Neighbours");

        modules = moduleSO.list;

        for (int i = 0; i < modules.Count; i++)
        {
            updateOneNeighbour(modules[i].GetComponent<Modulescript>().neighbourNorth, neighbourSouth,  i);
            updateOneNeighbour(modules[i].GetComponent<Modulescript>().neighbourSouth, neighbourNorth, i);
            updateOneNeighbour(modules[i].GetComponent<Modulescript>().neighbourWest, neighbourEast,i);
            updateOneNeighbour(modules[i].GetComponent<Modulescript>().neighbourEast, neighbourWest,i);
            updateOneNeighbour(modules[i].GetComponent<Modulescript>().neighbourUp, neighbourDown, i);
            updateOneNeighbour(modules[i].GetComponent<Modulescript>().neighbourDown, neighbourUp, i);
        }

        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject.GetComponent<Modulescript>());
    }

    public void updateOneNeighbour( List<GameObject> dir, List<GameObject> oppDir, int i)
    {

            for (int k = 0; k <dir.Count; k++)
            {
                if (dir[k] && dir[k].name == gameObject.name)
                {
                    oppDir.Add(modules[i]);
                }
            }

    }


    public void RotateClockWise()
    {

        Undo.RecordObject(gameObject.GetComponent<Modulescript>(), "Update Neighbours");

        List<GameObject> tmpWest = new List<GameObject>();
        List<GameObject> tmpEast = new List<GameObject>();
        List<GameObject> tmpSouth = new List<GameObject>();
        List<GameObject> tmpNorth = new List<GameObject>();

        tmpWest = neighbourWest;
        tmpEast = neighbourEast;
        tmpSouth = neighbourSouth;
        tmpNorth =neighbourNorth;

        neighbourWest = tmpSouth;
        neighbourEast = tmpNorth;
        neighbourSouth = tmpEast;
        neighbourNorth = tmpWest;
        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject.GetComponent<Modulescript>());
    }
#endif

}

