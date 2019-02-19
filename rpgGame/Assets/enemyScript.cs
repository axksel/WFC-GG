using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour,EnemyIO
{

    public int health;
    public EnemyList enemiesInRange;
    public GameObject loot;

    public int TakeDamage(int amount)
    {
        health = health - amount;
        if (health <= 0)
        {
            enemiesInRange.list.Remove(this.gameObject);
            Destroy(gameObject, 1);
            GameObject loottmp = Instantiate(loot, transform.position + loot.transform.position, Quaternion.identity);
            loottmp.GetComponent<goldscript>().amount = Random.Range(50, 100);
        }


        return health - amount;
        
    }

}
