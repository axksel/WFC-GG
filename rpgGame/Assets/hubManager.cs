using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hubManager : MonoBehaviour
{

    OnLevelCreated lvlCreated;
    // Start is called before the first frame update
    void Start()
    {
        lvlCreated = GetComponent<OnLevelCreated>();
        lvlCreated.ActivatePlayer(0, 0, 0);
        
    }

}
