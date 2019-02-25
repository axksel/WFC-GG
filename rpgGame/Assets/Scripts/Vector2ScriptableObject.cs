using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "vector2", menuName = "vector2", order = 1)]
public class Vector2ScriptableObject : ScriptableObject
{
    [SerializeField]
    public Vector2 value;


}