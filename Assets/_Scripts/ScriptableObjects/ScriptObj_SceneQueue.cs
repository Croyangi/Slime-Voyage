using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Queuer Scriptable Object", menuName = "Cro's Scriptable Objs/Scene Queuer Scriptable Obj")]
public class ScriptObj_SceneQueue : ScriptableObject
{
    [Serializable]
    public class QueuedScenes
    {
        public string scene;
        public bool isAdditive;
        public bool isActiveScene;
    }

    public List<QueuedScenes> queuedScenes;
}
