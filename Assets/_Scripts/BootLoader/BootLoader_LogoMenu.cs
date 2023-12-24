using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader_LogoMenu : MonoBehaviour
{
    [SerializeField] private RoomQueue _roomQueue;

    public void LoadMainMenu()
    {
        _roomQueue.LoadRoom("WarehouseDioramaMenu");
    }
}
