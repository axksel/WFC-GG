using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "skill", menuName = "Skill", order = 1)]
public class SkillScriptableObject : ScriptableObject
{
    public string objectName = "New MyScriptableObject";
    public int dmg;
    public GameObject skillPrefab;
    public bool isLoot;
    public int price;
    public Sprite icon;
    public bool isRanged =false;
    public Color hitProjectileColor;

}