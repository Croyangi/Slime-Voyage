using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ReturnToMenuButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RoomQueue _roomQueue;

    public void OnReturnToMenuPress()
    {
        ReturnToMenu();
    }



    public void ReturnToMenu()
    {
        _roomQueue.LoadRoom("WarehouseDioramaMenu");
    }
}
