using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnter : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private string roomName;

    [Header("References")]
    [SerializeField] private GameObject respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Tags>() != null) 
        {
            Tags _tags = collision.GetComponent<Tags>();
            if (_tags.CheckTags("Player") == true)
            {
                Manager_RespawnPoint.instance.respawnPointPosition = respawnPoint.transform.position;
            }
        }
    }

    //private void OnEnable()
    //{
    //    if (respawnPoint == null)
    //    {
    //        Debug.Log("finding!");
    //        FindRespawnPoint();
    //    }
    //}

    //private void FindRespawnPoint() // This is an expensive search, will try to optimize
    //{
    //    Tags _tags;
    //    GameObject[] allObjects = FindObjectsOfType<GameObject>();

    //    foreach (GameObject obj in allObjects)
    //    {
    //        if (obj.GetComponent<Tags>() != null)
    //        {
    //            _tags = obj.GetComponent<Tags>();
    //            if (_tags.CheckTags("RespawnPoint") == true)
    //            {
    //                CheckRespawnPoint(obj);
    //            }
    //        }
    //    }
    //}

    //private void CheckRespawnPoint(GameObject obj) // There's a better way to do this
    //{
    //    if (obj.GetComponent<RespawnPoint>().roomName == roomName)
    //    {
    //        respawnPoint = obj;
    //    }
    //}
}
