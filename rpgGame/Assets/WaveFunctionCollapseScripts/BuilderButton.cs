

using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(GridManager))]
public class ObjectBuilderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager myScript = (GridManager)target;
      

        if (GUILayout.Button("Collapse Random"))
        {
            myScript.Collapse();
        }

        if (GUILayout.Button("Iterate and Collapse"))
        {
            myScript.StartCoroutine(myScript.IterateAndCollapse());
        }

     
    }

}
