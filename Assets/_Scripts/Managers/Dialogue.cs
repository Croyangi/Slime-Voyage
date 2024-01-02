using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [TextArea]
    public string dialogueText = "";

    public float dialogueSpeed = 0.1f;
    public float dialogueStallTime = 0f;

    public bool cleanText = false;
    public bool stoppingFlag = false;
}
