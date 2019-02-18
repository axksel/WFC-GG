using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "skilllist", menuName = "SkillList", order = 1)]
public class SkillListScriptableObject : ScriptableObject
{
   public  List<SkillScriptableObject> list = new List<SkillScriptableObject>();

}