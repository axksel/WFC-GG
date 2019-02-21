using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonScript : MonoBehaviour
{

    public intScriptableObject PlayerHealth;
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("lol");
        if (other.gameObject.tag == "PlayerDmgCollider")
        {
            
            PlayerHealth.value -= 50;

        }

       


    }
}
