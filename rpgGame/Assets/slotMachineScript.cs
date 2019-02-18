using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slotMachineScript : MonoBehaviour
{
    public SkillScriptableObject skill;
    public intScriptableObject playerGold;
    void Start()
    {
       Instantiate(skill.skillPrefab, transform.position, Quaternion.identity);
        skill.isLoot = true; 

    }

    public void Interact()
    {
        if (playerGold.value >= skill.price)
        {


        }

    }
}
