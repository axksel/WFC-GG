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
        transform.position = new Vector3(0, 0, -1000);
        playerGold.value += amount;
        Destroy(gameObject, 0.05f);

    }
}
