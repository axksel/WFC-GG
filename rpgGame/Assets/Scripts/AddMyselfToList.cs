using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMyselfToList : MonoBehaviour
{
    public GameObjectList gameobjectList;
    public bool clear = true;
    private void Awake()
    {
        if (clear)
        {
            gameobjectList.list.Clear();
        }

        gameobjectList.list.Add(this.gameObject);
    }
}
