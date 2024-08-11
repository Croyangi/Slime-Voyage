using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_RoomLoader : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms;
    public RoomLoader currentRoom;
    public RoomLoader previousRoom;


    public static Manager_RoomLoader instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one RoomLoader Manager in the scene.");
        }
        instance = this;

        rooms.Clear();
    }

    public void OnLoadRoom(RoomLoader roomAdded)
    {
        if (currentRoom != null)
        {
            previousRoom = currentRoom;
        }
        currentRoom = roomAdded;
        Manager_Cinemachine.instance.OnChangeCinemachine(currentRoom.cinemachine);


        //foreach (GameObject room in rooms)
        //{
        //    room.GetComponent<Room>().UnloadRoom();
        //}
        //rooms.Clear();

        //rooms.Add(roomAdded);
    }

    private void FixedUpdate()
    {
        if (previousRoom != null && currentRoom != null) 
        {
            if (previousRoom.isLoaded && !currentRoom.isLoaded)
            {
                previousRoom.LoadRoom();
            }
        }
    }
}
