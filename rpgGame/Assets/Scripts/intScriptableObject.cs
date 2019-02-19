using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "int", menuName = "int", order = 1)]
public class intScriptableObject : ScriptableObject
{
    [SerializeField]
    public int value;


}