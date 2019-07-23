using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTest : MonoBehaviour
{

    public FloatList scenesToPlay;
    List<int> scenes = new List<int>();

    void Start()
    {
        scenesToPlay.list.Add(Random.Range(1, 3));
        scenesToPlay.list.Add(Random.Range(3, 5));
        scenesToPlay.list.Add(Random.Range(5, 7));
        scenesToPlay.list.Add(Random.Range(7, 9));
        scenesToPlay.list.Add(Random.Range(9, 11));
        scenesToPlay.list.Add(Random.Range(11, 13));
    }

    void Update()
    {
        
    }
}
