using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPosScript : MonoBehaviour
{
    // Start is called before the first frame update
   
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        
    }
}
