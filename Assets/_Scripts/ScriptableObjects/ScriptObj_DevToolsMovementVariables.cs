using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement Scriptable Object", menuName = "Cro's ScriptObjs/Dev Tools Movement Vars")]
public class ScriptObj_DevToolsMovementVariables : ScriptableObject
{
    public DevTools devTools;

    [Serializable]
    public class DevTools
    {
        [Header("Velocity")]
        public float acceleration;
        public float deceleration;
        public float velocityPower;
        public float movementSpeed;
    }
}
