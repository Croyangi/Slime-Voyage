using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifer Mode Scriptable Object", menuName = "Cro's Scriptable Objs/Modifier Mode Scriptable Obj")]
public class ScriptObj_ModifierMode : ScriptableObject
{
    public bool isModified;
    public bool isSwapeeMode;


    // Reset Values
    public void ResetModifiers()
    {
        isModified = false;
        isSwapeeMode = false;
    }
}
