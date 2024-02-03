using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_WarehouseMusic : MonoBehaviour
{
    [Header("References")]
    public AudioSource as_music;

    public static Manager_WarehouseMusic instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Cinemachine Manager in the scene.");
        }
        instance = this;
    }
}

