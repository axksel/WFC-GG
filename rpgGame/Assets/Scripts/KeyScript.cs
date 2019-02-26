using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour,IsInteracable
{
    public GameObjectList inventory;
    public string ReturnName()
    {

        return "Pick up key";
    }

    public void Interact()
    {
        transform.position = new Vector3(0, 0, -1000);
        inventory.list.Add(this.gameObject);
        GetComponent<MeshRenderer>().enabled = false;

    }

}
