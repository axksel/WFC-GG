using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weights : MonoBehaviour
{
    public FloatList weights;
    public FloatList summedWeights;
    [HideInInspector]
    public ScriptableObjectList moduleSO;
    [HideInInspector]
    public GameObject[] moduleParents;

    public void AssignWeights(List<GameObject> modules)
    {
        moduleParents = new GameObject[moduleSO.list.Count];
        for (int i = 0; i < modules.Count; i++)
        {
            moduleParents[i] = Instantiate(new GameObject(modules[i].name), transform.position, Quaternion.identity, gameObject.transform);
            modules[i].GetComponent<Modulescript>().moduleIndex = i;
        }

        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].GetComponent<Modulescript>().weight = summedWeights.list[(int)modules[i].GetComponent<Modulescript>().moduleType];
        }
    }

    public void CalculateWeights()
    {
        for (int i = 0; i < moduleParents.Length; i++)
        {
            weights.list[i] = moduleParents[i].transform.childCount;
        }

        for (int i = 0; i < moduleSO.list.Count; i++)
        {
            summedWeights.list[(int)moduleSO.list[i].GetComponent<Modulescript>().moduleType] = (0.5f * summedWeights.list[(int)moduleSO.list[i].GetComponent<Modulescript>().moduleType]) + (0.5f * weights.list[i]);
        }
    }
}