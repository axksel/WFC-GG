using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayScript : MonoBehaviour,IsInteracable
{

    public GameObject door;
    public EnemyList inventory;
   public string ReturnName()
    {
        return "Unlock door with key";

    }

  public void Interact()
    {
        foreach (var item in inventory.list)
        {
            if(item.name == "key")
            {
                inventory.list.Remove(item);
                Destroy(door);
            }
        }

    }
}
