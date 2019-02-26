using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyScript : MonoBehaviour,EnemyIO
{

    public int health;
    public GameObjectList enemiesInRange;
    public GameObjectList loot;
    public GameObject player;
    public float alertDistance = 10f;
    NavMeshAgent enemyAgent;
    float time;
    float attackSpeed = 1;
    public GameObject projectile;

    public GameObject healthBarImage;
    float fullHealth;
    float fillamount;
    Image healthBar;
    Animator anim;
    public intScriptableObject playerHealth;
    int attackDamage = 5;


    Vector3 startPosition;

    public void Start()
    {
        fullHealth = health;
        healthBar = healthBarImage.GetComponent<Image>();

        enemyAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    private void Update()
    {
        if (health / fullHealth != healthBar.fillAmount)
        {
            fillamount = Mathf.Lerp(fillamount, health / fullHealth, 0.05f);
        }
        healthBar.fillAmount = fillamount;



        if (Vector3.Distance(transform.position, player.transform.position)<alertDistance)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            enemyAgent.destination = player.transform.position;
            anim.SetBool("Walk", true);
            anim.SetBool("Idle", false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 0.5f)
        {
            enemyAgent.destination = transform.position;
            anim.SetBool("Punch", true);
            if (time + attackSpeed < Time.time)
            {
                Attack();
            }
        }
        else
        {
            anim.SetBool("Punch", false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) > alertDistance)
        {
            enemyAgent.destination = startPosition;
            if(Vector3.Distance(transform.position, startPosition) < 1f ){
                anim.SetBool("Idle", true);
                anim.SetBool("Walk", false);
            }
        }
    }

    public int TakeDamage(int amount)
    {
        health = health - amount;
        if (health <= 0)
        {
            anim.SetBool("Dead", true);
            enemiesInRange.list.Remove(this.gameObject);
            //Destroy(gameObject, 1);
            GameObject loottmp = Instantiate(loot.list[Random.Range(0, loot.list.Count)], new Vector3(transform.position.x,0.3f,transform.position.z), Quaternion.identity);
            this.enabled = false;
            Collider box = GetComponent<BoxCollider>();
            box.enabled = false;
            enemyAgent.enabled = false;
            healthBar.enabled = false;
        }


        return health - amount;
        
    }


    public void Attack()
    {
        time = Time.time;
        playerHealth.value -= attackDamage;
        /*GameObject proj = Instantiate(projectile, transform.position, transform.rotation);
        proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward * 100);*/
    }
}
