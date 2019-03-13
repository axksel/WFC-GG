using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{ 
    public void ClickMenu()
    {
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Additive);
    }
}
