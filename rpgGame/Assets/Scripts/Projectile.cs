using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public int attackDamage = 5;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            other.gameObject.GetComponent<enemyScript>().TakeDamage(attackDamage);
            Destroy(gameObject);
        }

    }
}
