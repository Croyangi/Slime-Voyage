using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(LightbulbSwinger))]
public class Editor_LightbulbSwinger : Editor
{
    private float length;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector with all serialized fields
        DrawDefaultInspector();

        LightbulbSwinger swinger = (LightbulbSwinger) target;

        GUILayout.Space(20);

        length = EditorGUILayout.FloatField("Length:", length);

        // Create a button in the Inspector
        if (GUILayout.Button("Inverse"))
        {
            swinger.ToggleLights();
        }

        // Create a button in the Inspector
        if (GUILayout.Button("Set Size"))
        {
            swinger.ChangeLightbulbLength(length);
        }
    }

}
