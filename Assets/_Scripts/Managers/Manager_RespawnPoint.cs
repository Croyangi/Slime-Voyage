using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_RespawnPoint : MonoBehaviour
{
    [Header("Variables")]
    public Vector2 respawnPointPosition;

    public static Manager_RespawnPoint instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Respawn Point Manager in the scene.");
        }
        instance = this;
    }


}