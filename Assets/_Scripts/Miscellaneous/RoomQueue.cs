using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomQueue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public RoomQueueScriptObj _roomQueueScriptObj;

    public void UnqueueAllRooms()
    {
        _roomQueueScriptObj.currentQueuedRooms.Clear();
    }

    public void QueueRoom(string roomName)
    {
        _roomQueueScriptObj.currentQueuedRooms.Add(roomName);
    }

    public void LoadRoom(string roomName)
    {
        SceneManager.LoadScene(roomName);
    }

    public void LoadQueuedRooms()
    {
        if (_roomQueueScriptObj.currentQueuedRooms.Count > 0)
        {
            SceneManager.LoadScene(_roomQueueScriptObj.currentQueuedRooms[0]);
            _roomQueueScriptObj.currentQueuedRooms.RemoveAt(0);
        } else
        {
            SceneManager.LoadScene("LogoMenu");
        }
    }
}
