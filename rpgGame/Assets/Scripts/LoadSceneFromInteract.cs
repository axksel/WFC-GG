using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromInteract : MonoBehaviour,IsInteracable
{
    public GameObjectList gameManager;
    void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void Interact()
    {
        gameManager.list[0].GetComponent<PlayerManager>().ChangeScene();

    }

    public string ReturnName()
    {

        return "Enter Cave of DOOM";
    }
}
