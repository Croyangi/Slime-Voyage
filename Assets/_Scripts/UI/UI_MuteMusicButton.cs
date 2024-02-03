using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_MuteMusicButton : MonoBehaviour
{

    public void OnClickMuteMusicButton()
    {
        ToggleMuteMusicButton();
    }

  

    private void ToggleMuteMusicButton()
    {
        if (Manager_WarehouseMusic.instance.as_music.mute == true)
        {
            Manager_WarehouseMusic.instance.as_music.mute = false;
        } else
        {
            Manager_WarehouseMusic.instance.as_music.mute = true;
        }
    }
}
