using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Checkpoint Queuer Scriptable Object", menuName = "Cro's Scriptable Objs/Checkpoint Queuer Scriptable Obj")]
public class ScriptObj_CheckpointQueue : ScriptableObject
{
    public string checkpointId = "";

    public void ClearCheckpoints()
    {
        checkpointId = string.Empty;
    }
}
