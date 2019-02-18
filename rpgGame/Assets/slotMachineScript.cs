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
    public bool bob = false;
    void Start()
    {
      // Instantiate(skill.skillPrefab, transform.position, Quaternion.identity);
        skill.isLoot = true;
        soldSKill.list.Clear();

    }

    private void Update()
    {
        if (bob)
        {
            Interact();
            bob = false;
        }
    }


    public string ReturnName()
    {


        return "Buy "+skill.name+" for "+skill.price + " gold";
    }


    public void Interact()
    {

        Debug.Log("heallo");
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
