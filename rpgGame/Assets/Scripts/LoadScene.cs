using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public FloatList scenesToPlay;

    void Start()
    {
        
    }

    public void LoadTest()
    {
        int index = (int)scenesToPlay.list[0];
        scenesToPlay.list.RemoveAt(0);
        SceneManager.LoadScene(index);
    }
}
