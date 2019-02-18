using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillControl : MonoBehaviour
{
    public SkillScriptableObject equippedSkill;
    public GameObject EquippedPosition;
    public GameObject button;
    public float count=0;
    

    void Start()
    {
        GameObject tmpSkill = Instantiate(equippedSkill.skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation);
        tmpSkill.transform.SetParent(EquippedPosition.transform);
        button.GetComponentInChildren<Text>().text = equippedSkill.name;
       
 
    }

 
    public void Attack()
    {
        
       //Jeg tror angrebet skal ligges her.
    }

    public void ChangeEquippedSkill(SkillScriptableObject newWeapon)
    {
        equippedSkill = newWeapon;
        GameObject tmpSkill = Instantiate(equippedSkill.skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation);
        tmpSkill.transform.SetParent(EquippedPosition.transform);
        button.GetComponentInChildren<Text>().text = equippedSkill.name;
    }
   
}
