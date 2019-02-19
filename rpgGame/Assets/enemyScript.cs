using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour,EnemyIO
{

    public int health;
    int index;
    public int TakeDamage(int amount)
    {
        health = health - amount;
        return health - amount;
        
    }

    public void setIndex( int index)
    {

        this.index = index;
    }

    public int getIndex()
    {

        return index;
    }
}
