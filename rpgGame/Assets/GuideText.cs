using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    bool reachedEnd;
    public CreateMap cm;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        tmp.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "GoalSphere")
        {
            tmp.text = "Go back to where you started";
            cm.RestoreMaterial();
            reachedEnd = true;
        }
        if (other.gameObject.tag == "StartSphere")
        {
            tmp.text = "Follow the red path";
        }
        if (other.gameObject.tag == "StartSphere" && reachedEnd)
        {
            tmp.text = "ggwp";
        }
    }
}
