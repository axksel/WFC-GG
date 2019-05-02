using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movementTracker : MonoBehaviour
{

    Vector3 position;
    public float SummedLength = 0;
    public float firstSummedLength =0;
    public float secondSummedLength = 0;
    public float time =0;
    public float startTime;
    public float firstTripTime;
    public float secondTripTime;
    public FloatList results;
    public FloatList scenesToPlay;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        SummedLength+=Vector3.Distance(transform.position, position);
        position = transform.position;
    }

    public void Finished()
    {
        results.list.Add(SceneManager.GetActiveScene().buildIndex);
        results.list.Add(firstTripTime);
        results.list.Add(secondTripTime);
        results.list.Add(firstSummedLength);
        results.list.Add(secondSummedLength);

        if(scenesToPlay.list.Count == 0)
        {
            SceneManager.LoadScene(13);
        }

        int index = (int)scenesToPlay.list[0];
        scenesToPlay.list.RemoveAt(0);
        SceneManager.LoadScene(index);
    }
}
