using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldscript : MonoBehaviour,IsInteracable
{
    public int amount;
    public intScriptableObject playerGold;


    private void Start()
    {
        if (amount == 0)
        {
            amount = Random.Range(0,30);
        }
    }

    public string ReturnName()
    {

        return "Pick up " + amount + "Gold";
    }
    public void Interact()
    {
        playerGold.value += amount;
        Destroy(gameObject);

    }
}
