using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayScript : MonoBehaviour,IsInteracable
{

   
    public GameObjectList inventory;
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
                transform.position = new Vector3(0, 0, -1000);
                inventory.list.Remove(item);
                Destroy(this.gameObject,0.02f);
            }
        }

    }
}
