using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loading Screen Tips", menuName = "Cro's Scriptable Objs/New Flavor Text")]
public class ScriptObj_FlavorText : ScriptableObject
{
    [Header("Tips")]

    [TextArea]
    public List<string> flavorText;
}
