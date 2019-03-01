using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{  
    public int attackDamage = 5;
    public ParticleSystem hit;
    [HideInInspector]
    public SkillControl skillcontrol;
    

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            other.gameObject.GetComponent<enemyScript>().TakeDamage(attackDamage);
            if (!skillcontrol.hit.isPlaying)
            {
                skillcontrol.hit.transform.position = other.transform.position + new Vector3(0, 0.4f, 0);
                skillcontrol.hit.Play();
            }
                Destroy(gameObject);
        }
        else if(other.tag != "Player" && other.tag != "Interactable")
        {
            if (!skillcontrol.hit.isPlaying)
            {
                skillcontrol.hit.transform.position = other.transform.position;
                skillcontrol.hit.Play();
            }
                Destroy(gameObject);
        }
    }
}
