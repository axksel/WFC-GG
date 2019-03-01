using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healPotion : MonoBehaviour,IsInteracable
{

    public intScriptableObject playerHealth;
    public intScriptableObject playerMaxHealth;
    int healAmount;

    private void Start()
    {
        healAmount = Random.Range(30, 60);
    }
    public string ReturnName()
    {

        return "Heal " + healAmount + " points";
    }

    public void Interact()
    {
        if (playerHealth.value + healAmount > playerMaxHealth.value)
        {
            playerHealth.value = playerMaxHealth.value;
        }
        else
        {
            playerHealth.value += healAmount;
        }

        transform.position = new Vector3(0, 0, -1000);
        Destroy(gameObject, 0.05f);
    }
}
