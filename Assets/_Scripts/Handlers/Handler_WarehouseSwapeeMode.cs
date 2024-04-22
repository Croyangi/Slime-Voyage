using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_WarehouseSwapeeMode : MonoBehaviour
{
    [SerializeField] private GameObject swapeeModeEnabledGroup;
    [SerializeField] private bool isSwapeeMode;
    [SerializeField] private GameObject swapeeModeSpawnPoint;

    private void Awake()
    {
        swapeeModeEnabledGroup.SetActive(false);
    }

    [ContextMenu("Enable Swapee Mode")]
    public void EnableSwapeeMode()
    {
        swapeeModeEnabledGroup.SetActive(true);
        isSwapeeMode = true;

        GameObject baseSlime = Manager_PlayerState.instance.player;
        baseSlime.transform.position = swapeeModeSpawnPoint.transform.position;
        Manager_Jukebox.instance.SetVolume(0f);
        Manager_Jukebox.instance.PlayBreakingProtocol();
    }

}
