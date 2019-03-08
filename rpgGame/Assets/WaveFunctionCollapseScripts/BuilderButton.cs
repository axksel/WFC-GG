using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager myScript = (GridManager)target;
        if (GUILayout.Button("Iterate"))
        {
            myScript.Iterate();
        }

        if (GUILayout.Button("Collapse Random"))
        {
            myScript.Collapse();
        }

        if (GUILayout.Button("Iterate and Collapse"))
        {
            myScript.IterateAndCollapse();
        }

        if (GUILayout.Button("New neighbourhood calc"))
        {
            myScript.FindNewNeighbours();
        }
    }
}
#endif