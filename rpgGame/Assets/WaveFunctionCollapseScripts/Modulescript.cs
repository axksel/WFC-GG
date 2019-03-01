using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class Modulescript : MonoBehaviour
{
    public enum direction {North= 0, East=1,South=2,West=3};

    public List<GameObject> neighbourWest = new List<GameObject>();
    public List<GameObject> neighbourEast = new List<GameObject>();
    public List<GameObject> neighbourSouth = new List<GameObject>();
    public List<GameObject> neighbourNorth = new List<GameObject>();
    public List<GameObject> neighbourUp = new List<GameObject>();
    public List<GameObject> neighbourDown = new List<GameObject>();

    public List<GameObject> modules = new List<GameObject>();

    public ScriptableObjectList moduleSO;
    private Modulescript target;

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

}
