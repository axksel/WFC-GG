﻿using System.Collections;
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
            mt.startTime = Time.time;
            mt.SummedLength = 0;
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

            mt.firstTripTime = Time.time - mt.startTime;
            mt.firstSummedLength = mt.SummedLength;
        }

        if (other.gameObject.tag == "StartSphere")
        {
            tmp.text = "Follow the red path";
        }

        if (other.gameObject.tag == "StartSphere" && reachedEnd)
        {
            tmp.text = "ggwp";

            mt.secondTripTime = Time.time - mt.startTime;
            mt.secondSummedLength = mt.SummedLength;
            mt.Finished();
        }
    }


}