using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ButtonScript), true )]
public class ButtonScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        ButtonScript myTarget = (ButtonScript)target;

        if(GUILayout.Button("Build Object"))
        {
            myTarget.BuildObject();
        }
    }
}
