using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour,EnemyIO
{

    public int health;
    public EnemyList enemiesInRange;
    public GameObject loot;
    public GameObject player;
    public float alertDistance = 10f;
    NavMeshAgent enemyAgent;
    float time;
    float attackSpeed = 2;
    public GameObject projectile;

    public void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        


        
        if (Vector3.Distance(transform.position, player.transform.position)<alertDistance)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            enemyAgent.destination = player.transform.position;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < alertDistance / 2)
        {
            enemyAgent.destination = transform.position;
            if (time + attackSpeed < Time.time)
            {
                Attack();
            }
        }

        if (Vector3.Distance(transform.position, player.transform.position) > alertDistance)
        {
            enemyAgent.destination = transform.position;
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
        Debug.Log("Enemy Attacking");
        time = Time.time;
        GameObject proj = Instantiate(projectile, transform.position, transform.rotation);
        proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward * 100);
    }
}
