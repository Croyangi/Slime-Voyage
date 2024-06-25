using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_Death : MonoBehaviour
{
    public void InitiateDeath()
    {
        Manager_PlayerState.instance.InitiatePlayerDeath();
    }
}
