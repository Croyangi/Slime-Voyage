using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpike_Loader : MonoBehaviour, IPlayerCuller
{
    [Header("References")]
    [SerializeField] private GameObject loadedObjects;
    [SerializeField] private Rigidbody2D _rb;


    public void LoadObjects()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        loadedObjects.SetActive(true);
    }

    public void DeloadObjects()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        loadedObjects.SetActive(false);
    }
}
