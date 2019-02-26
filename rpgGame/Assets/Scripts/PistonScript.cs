using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonScript : MonoBehaviour
{

    public intScriptableObject PlayerHealth;
    private void OnTriggerEnter(Collider other)
    {

      
        if (other.gameObject.tag == "PlayerDmgCollider")
        {
            
            PlayerHealth.value -= 50;

        }
         if (other.gameObject.tag == "enemy")
        {
            Debug.Log(other.gameObject);
           
            other.gameObject.GetComponent<enemyScript>().TakeDamage(50);

        }




    }
}
