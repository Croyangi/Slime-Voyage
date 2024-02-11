using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndingPoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GeneralLevelManager _generalLevelManager;
    [SerializeField] private Tags _tags;

    private void Awake()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject thisObject in allObjects)
        {
            if (_tags.CheckGameObjectTags(thisObject, "LevelManager") == true)
            {
                _generalLevelManager = thisObject.GetComponent<GeneralLevelManager>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tags.CheckGameObjectTags(collision.gameObject, "Player") == true)
        {
            _generalLevelManager.isRoomComplete = true;
        }

    }
}
