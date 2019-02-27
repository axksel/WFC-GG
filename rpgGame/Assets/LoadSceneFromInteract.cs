using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromInteract : MonoBehaviour,IsInteracable
{
    public GameObjectList gameManager;

    public void Interact()
    {
        SceneManager.LoadScene("scene3", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("hub");

    }

    public string ReturnName()
    {

        return "Enter Cave of DOOM";
    }
}
