using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour,IsInteracable
{
    public EnemyList inventory;
    public string ReturnName()
    {

        return "Pick up key";
    }

    public void Interact()
    {
        inventory.list.Add(this.gameObject);
        gameObject.SetActive(false);

    }

}
