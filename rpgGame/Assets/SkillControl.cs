using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Update()
    {
        
        if (count<360)
        {
            EquippedPosition.transform.RotateAround(transform.position, Vector3.up, 1000 * Time.deltaTime);
            count += 1000 * Time.deltaTime;
        }
     
       

    }

    public void Attack()
    {

        count = 0;
    }


   
}
