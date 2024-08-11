using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    public abstract void LoadRoom();
    public abstract void UnloadRoom();
}
