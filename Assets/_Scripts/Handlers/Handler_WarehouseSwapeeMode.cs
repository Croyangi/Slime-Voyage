using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_WarehouseSwapeeMode : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private GameObject swapeeIntroGroup;
    [SerializeField] private GameObject swapeeModeOnGroup;
    [SerializeField] private GameObject swapeeModeSpawnPoint;

    [SerializeField] private bool isSpeedrunModeOn;

    private void Awake()
    {
        swapeeModeOnGroup.SetActive(false);
        swapeeIntroGroup.SetActive(false);

        EnableSwapeeMode();
    }

    private void Start()
    {
        if (isSpeedrunModeOn)
        {
            Manager_SpeedrunTimer.instance.OpenSpeedrunTimer();
        }
    }

    [ContextMenu("Enable Swapee Mode")]
    public void EnableSwapeeMode()
    {
        swapeeIntroGroup.SetActive(true);

        GameObject baseSlime = Manager_PlayerState.instance.player;
        baseSlime.transform.position = swapeeModeSpawnPoint.transform.position;
    }

    // Setup, when Swapee cutscene ends
    public void EndSwapeeModeIntro()
    {
        swapeeModeOnGroup.SetActive(true);
        swapeeIntroGroup.SetActive(false);
        Manager_Jukebox.instance.PlayJukebox();
        Manager_SpeedrunTimer.instance.StartSpeedrunTimer();
    }

    public void LoadData(GameData data)
    {
        isSpeedrunModeOn = data.isSpeedrunModeOn;
    }

    public void SaveData(ref GameData data)
    {
    }

}
