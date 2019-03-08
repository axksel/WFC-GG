using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour,IsInteracable
{


    public GameObjectList gameManager;


    public void Interact()
    {
        gameManager.list[0].GetComponent<PlayerManager>().ChangeScene(0);
        
     
    }

    public string ReturnName()
    {

        return "Return to Grok";
    }
}
