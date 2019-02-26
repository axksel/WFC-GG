using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMyselfToList : MonoBehaviour
{
    public GameObjectList gameobjectList;
    private void Awake()
    {
        gameobjectList.list.Add(this.gameObject);
    }
}
