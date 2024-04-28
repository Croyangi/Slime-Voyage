using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_WarehouseSwapeeMode : MonoBehaviour
{
    [SerializeField] private GameObject swapeeIntroGroup;
    [SerializeField] private GameObject swapeeModeOnGroup;
    [SerializeField] private GameObject swapeeModeSpawnPoint;

    private void Awake()
    {
        swapeeModeOnGroup.SetActive(false);
        swapeeIntroGroup.SetActive(false);

        EnableSwapeeMode();
    }

    [ContextMenu("Enable Swapee Mode")]
    public void EnableSwapeeMode()
    {
        swapeeIntroGroup.SetActive(true);

        GameObject baseSlime = Manager_PlayerState.instance.player;
        baseSlime.transform.position = swapeeModeSpawnPoint.transform.position;
    }

    public void EndSwapeeModeIntro()
    {
        swapeeModeOnGroup.SetActive(true);
        swapeeIntroGroup.SetActive(false);
        Manager_Jukebox.instance.PlayJukebox();
    }

}
