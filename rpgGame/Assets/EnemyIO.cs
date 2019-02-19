using UnityEngine;
using System.Collections;

//This is a basic interface with a single required
//method.
public interface EnemyIO
{
    
    int TakeDamage(int amount);
    void setIndex(int index);
    int getIndex();
}