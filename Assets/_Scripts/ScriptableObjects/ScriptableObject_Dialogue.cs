using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tag", menuName = "Cro's Scriptable Objs/New Dialogue")]
public class ScriptableObject_Dialogue : ScriptableObject
{
    [Header("Formatting")]
    public bool isDialogueOnTop = false;
    public bool isDialogueMini = false;
    public bool isPlayerInputStall = false;

    [Header("Dialogue")]
    public List<Dialogue> _dialogues;
}
