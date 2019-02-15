using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    public SkillScriptableObject equippedSkill;
    public GameObject EquippedPosition;
    void Start()
    {
        GameObject tmpSkill = Instantiate(equippedSkill.skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation);
        tmpSkill.transform.SetParent(gameObject.transform);
    }

   
}
