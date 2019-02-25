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
    public EnemyList enemiesInRange;
    public GameObject enemu;
    public static GameObject tmpSkill;
    public ParticleSystem attack;
    public SkillScriptableObject equippedAimed;
    PlayerControl playerControl;

    void Start()
    {
        tmpSkill = Instantiate(equippedSkill.list[0].skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 60)), EquippedPosition.transform);
        button.GetComponentInChildren<Text>().text = "";
        enemiesInRange.list.Clear();
        attack = Instantiate(attack, transform);
        playerControl = GetComponent<PlayerControl>();
        button.GetComponent<Image>().sprite = equippedSkill.list[0].icon;

    }

 
    public void Attack()
    {
        if (!attack.isPlaying)
            attack.Play();

        playerControl.anim.SetBool("swordSlash", true);

        foreach (var item in enemiesInRange.list)
        {
            item.GetComponent<EnemyIO>().TakeDamage(equippedSkill.list[0].dmg);
            
        }
           
        
       //Jeg tror angrebet skal ligges her.
    }

    public void ChangeEquippedSkill(SkillListScriptableObject newWeapon)
    {
        Destroy(tmpSkill);
         equippedSkill.list.Clear();
        equippedSkill.list.Add( newWeapon.list[0]);
        tmpSkill = Instantiate(equippedSkill.list[0].skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation * Quaternion.Euler(new Vector3(0,0,60)), EquippedPosition.transform);
        button.GetComponentInChildren<Text>().text = "";
        button.GetComponent<Image>().sprite = equippedSkill.list[0].icon;

    }

    public void RangedAttack(Vector2 attackDir )
    {
        Debug.Log(attackDir);
        attackDir.Normalize();
        GameObject proj = Instantiate(equippedAimed.skillPrefab, transform.position, transform.rotation);
        proj.GetComponent<Rigidbody>().AddForce(new Vector3(attackDir.x,0,attackDir.y) * 100);
    }
    
   
}
