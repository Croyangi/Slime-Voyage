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

    public Sprite profilePicture;
    public string profileName;

    public Dialogue(Dialogue other)
    {
        this.dialogueText = other.dialogueText;
        this.dialogueSpeed = other.dialogueSpeed;
        this.dialogueStallTime = other.dialogueStallTime;
        this.cleanText = other.cleanText;
        this.stoppingFlag = other.stoppingFlag;
        this.profilePicture = other.profilePicture;
        this.profileName = other.profileName;
    }
}
