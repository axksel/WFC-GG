using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class slotMachineScript : MonoBehaviour,IsInteracable
{
    public SkillScriptableObject skill;
    public SkillListScriptableObject soldSKill;
    public intScriptableObject playerGold;
    public UnityEvent weaponBought;
    public Transform skillPlacement;
    void Start()
    {
      // Instantiate(skill.skillPrefab, transform.position, Quaternion.identity);
        skill.isLoot = true;
        soldSKill.list.Clear();

        if (!skill.isRanged) {
            GameObject tmp = Instantiate(skill.skillPrefab, skillPlacement.position, skillPlacement.rotation, skillPlacement);
        }
        else
        {
            GameObject tmp = Instantiate(skill.skillPrefab, skillPlacement.position+new Vector3(0.4f,0.2f,0), skillPlacement.rotation, skillPlacement);
        }
       
    }

 


    public string ReturnName()
    {


        return "Buy "+skill.name+" for "+skill.price + " gold";
    }


    public void Interact()
    {

       ;
        if (playerGold.value >= skill.price)
        {
            soldSKill.list.Clear();
            skill.isLoot = false;
            soldSKill.list.Add(skill);
            playerGold.value = playerGold.value - skill.price;
            weaponBought.Invoke();

        }

    }
}
