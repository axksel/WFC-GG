using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuideText : MonoBehaviour
{
    movementTracker mt;
    public TextMeshProUGUI tmp;
    bool reachedEnd;
    public CreateMap cm;
    bool firstTimeLogged = true;

    void Start()
    {
        mt = GetComponent<movementTracker>();
    }

    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        tmp.text = "";

        if (other.gameObject.tag == "GoalSphere")
        {
            if (firstTimeLogged)
            {
                mt.startTime = Time.time;
                mt.SummedLength = 0;
                firstTimeLogged = false;
            }
        }
        if (other.gameObject.tag == "StartSphere")
        {
            mt.startTime = Time.time;
            mt.SummedLength = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "GoalSphere")
        {
            tmp.text = "Go back to where you started";
            cm.RestoreMaterial();
            reachedEnd = true;

            if (firstTimeLogged)
            {
                mt.firstTripTime = Time.time - mt.startTime;
                mt.firstSummedLength = mt.SummedLength;
            }
        }

        if (other.gameObject.tag == "StartSphere")
        {
            tmp.text = "Follow the red path";
        }

        if (other.gameObject.tag == "StartSphere" && reachedEnd)
        {
            tmp.text = "Well done. Loading next level";

            mt.secondTripTime = Time.time - mt.startTime;
            mt.secondSummedLength = mt.SummedLength;
            mt.Finished();
        }
    }


}
