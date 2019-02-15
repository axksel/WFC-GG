using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "skill", menuName = "Skill", order = 1)]
public class SkillScriptableObject : ScriptableObject
{
    public string objectName = "New MyScriptableObject";
    public int dmg;
    public GameObject skillPrefab;
}