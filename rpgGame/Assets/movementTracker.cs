using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementTracker : MonoBehaviour
{

    Vector3 position;
    public float summedLenght =0;
    public float time =0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startTracking());
        
    }

    IEnumerator startTracking()
    {
        yield return new WaitForSeconds(0.1f);
        summedLenght = 0;
        time = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        summedLenght+=Vector3.Distance(transform.position, position);
        position = transform.position;


    }
}
