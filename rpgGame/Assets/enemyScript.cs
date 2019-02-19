using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour,EnemyIO
{

    public int health;
    public EnemyList enemiesInRange;

    public int TakeDamage(int amount)
    {
        health = health - amount;
        if (health <= 0)
        {
            enemiesInRange.list.Remove(this.gameObject);
            Destroy(gameObject, 1);
        }


        return health - amount;
        
    }

}
