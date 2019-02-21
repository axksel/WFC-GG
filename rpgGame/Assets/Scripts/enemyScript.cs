using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour,EnemyIO
{

    public int health;
    public EnemyList enemiesInRange;
    public GameObject loot;
    public GameObject player;
    public float alertDistance =1.5f;


    

    private void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x,transform.position.y, player.transform.position.z));


        
        if (Vector3.Distance(transform.position, player.transform.position)<alertDistance)
        {
            Attack();
            
        }  
    }

    public int TakeDamage(int amount)
    {
        health = health - amount;
        if (health <= 0)
        {
            enemiesInRange.list.Remove(this.gameObject);
            Destroy(gameObject, 1);
            GameObject loottmp = Instantiate(loot, new Vector3(transform.position.x,0.3f,transform.position.z), Quaternion.identity);
            loottmp.GetComponent<goldscript>().amount = Random.Range(50, 100);
        }


        return health - amount;
        
    }


    public void Attack()
    {
        Debug.Log("lol");

    }
}
