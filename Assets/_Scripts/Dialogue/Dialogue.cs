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

    public string profileName;

    public Sprite profilePicture;

    public AudioClip sfx_speaking;
    public int sfxSpeakingSpacing = 1;

    public Dialogue(Dialogue other)
    {
        this.dialogueText = other.dialogueText;
        this.dialogueSpeed = other.dialogueSpeed;
        this.dialogueStallTime = other.dialogueStallTime;
        this.cleanText = other.cleanText;
        this.stoppingFlag = other.stoppingFlag;
        this.profileName = other.profileName;
        this.profilePicture = other.profilePicture;
        this.sfx_speaking = other.sfx_speaking;
        this.sfxSpeakingSpacing = other.sfxSpeakingSpacing;
    }
}
