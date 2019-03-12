using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "floatList", menuName = "FloatList", order = 1)]
public class FloatList : ScriptableObject
{
    public List<float> list = new List<float>();

}