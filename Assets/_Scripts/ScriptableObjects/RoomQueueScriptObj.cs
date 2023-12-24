using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Queue Scriptable Object", menuName = "Cro's Scriptable Objs/New Room Queue Scriptable Obj")]
public class RoomQueueScriptObj : ScriptableObject
{
    public List<string> currentQueuedRooms;

    public List<string> warehouseRooms;
    public List<string> miscellaneousRooms;
}
