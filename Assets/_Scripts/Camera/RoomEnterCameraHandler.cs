using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterCameraHandler : MonoBehaviour
{
    [SerializeField] private Tags _tags;
    [SerializeField] private GameObject _cinemachine;
    //[SerializeField] private GameObject _camera;
    //[SerializeField] private CinemachineConfiner2D _cinemachineConfiner2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tags.CheckGameObjectTags(collision.gameObject, "CinemachineActivator"))
        {
            _cinemachine.SetActive(true);
        }

        if (_tags.CheckGameObjectTags(collision.gameObject, "Player"))
        {
            _cinemachine.GetComponentInChildren<CinemachineVirtualCamera>().Follow = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_tags.CheckGameObjectTags(collision.gameObject, "CinemachineActivator"))
        {
            _cinemachine.SetActive(false);
        }
    }

    /*private void FindCinemachine()
    {
        string tag = "MainCamera";

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject thisObject in allObjects)
            if (thisObject.GetComponent<Tags>() != null)
            {
                _tags = thisObject.GetComponent<Tags>();
                if (_tags.CheckTags(tag) == true)
                {
                    Debug.Log("Successfully found GameObject with tag: " + tag);
                    _camera = thisObject;
                }
            }

        _cinemachineConfiner2D = _camera.GetComponentInChildren<CinemachineConfiner2D>();
    }

    private void ApplyCinemachineConfiner()
    {
        _cinemachineConfiner2D.m_BoundingShape2D = gameObject.GetComponent<PolygonCollider2D>();
        _cinemachineConfiner2D.InvalidateCache();
    }*/
}
