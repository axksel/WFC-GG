using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "gameObjectList", menuName = "GameObjectList", order = 1)]
public class GameObjectList : ScriptableObject
{
    public List<GameObject> list = new List<GameObject>();

}