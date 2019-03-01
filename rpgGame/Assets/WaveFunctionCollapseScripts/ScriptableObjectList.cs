using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "list", menuName = "List", order = 1)]
public class ScriptableObjectList : ScriptableObject
{

    public List<GameObject> list = new List<GameObject>();

}