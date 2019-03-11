using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromInteract : MonoBehaviour,IsInteracable
{
    public GameObjectList gameManager;

    void Start()
    {

    }

    public void Interact()
    {
        
        gameManager.list[0].GetComponent<PlayerManager>().ChangeScene(3);

    }

    public string ReturnName()
    {

        return "Enter Cave of DOOM";
    }
}
