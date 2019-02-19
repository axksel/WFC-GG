using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldscript : MonoBehaviour,IsInteracable
{
    public int amount;
    public intScriptableObject playerGold;

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
