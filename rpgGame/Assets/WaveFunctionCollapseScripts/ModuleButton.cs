using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Modulescript))]
[ExecuteInEditMode]
public class BuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Modulescript myScript = (Modulescript)target;
        if (GUILayout.Button("Update"))
        {
            myScript.UpdateNeighbours();
            
        }

        if (GUILayout.Button("RotateClockwise"))
        {
            myScript.RotateClockWise();

        }


        if (GUILayout.Button("Add new neighbours"))
        {
            myScript.UpdateNeigboursInANewWay();

        }


    }
}
