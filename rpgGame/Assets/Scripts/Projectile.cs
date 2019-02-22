using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public intScriptableObject playerHealth;
    int attackDamage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerDmgCollider")
        {
            playerHealth.value -= attackDamage;
            Destroy(gameObject);
        }
        
    }
}
