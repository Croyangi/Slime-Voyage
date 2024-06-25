using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GeneralLoader : MonoBehaviour, IPlayerCuller
{
    [Header("References")]
    [SerializeField] private GameObject loadedObjects;

    public void LoadObjects()
    {
        loadedObjects.SetActive(true);
    }

    public void DeloadObjects()
    {
        loadedObjects.SetActive(false);
    }
}
