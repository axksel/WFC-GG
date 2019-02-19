using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillControl : MonoBehaviour
{
    public SkillListScriptableObject equippedSkill;
    public GameObject EquippedPosition;
    public GameObject button;
    public float count=0;

    public static GameObject tmpSkill;
    void Start()
    {
         tmpSkill = Instantiate(equippedSkill.list[0].skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation, EquippedPosition.transform);
        button.GetComponentInChildren<Text>().text = equippedSkill.list[0].name;
      
        
 
    }

 
    public void Attack()
    {
       
       //Jeg tror angrebet skal ligges her.
    }

    public void ChangeEquippedSkill(SkillListScriptableObject newWeapon)
    {
        Destroy(tmpSkill);
         equippedSkill.list.Clear();
        equippedSkill.list.Add( newWeapon.list[0]);
        tmpSkill = Instantiate(equippedSkill.list[0].skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation, EquippedPosition.transform);
        button.GetComponentInChildren<Text>().text = equippedSkill.list[0].name;

    }
   
}
