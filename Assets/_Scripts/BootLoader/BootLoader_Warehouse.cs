using Cinemachine;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_Warehouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioClip audioClip_music;
    [SerializeField] private Handler_WarehouseIntro _warehouseIntro;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private SceneAsset scene_overlayLoadingScreen;
    [SerializeField] private SceneAsset scene_bootloaderDevTools;
    [SerializeField] private SceneAsset scene_bootloaderGlobal;

    private void Awake()
    {
        _sceneQueue.LoadSceneWithAsset(scene_bootloaderDevTools, true);
        _sceneQueue.LoadSceneWithAsset(scene_bootloaderGlobal, true);
        _sceneQueue.LoadSceneWithAsset(scene_overlayLoadingScreen, true);
        StartCoroutine(DelayedAwake());

        StartCoroutine(InitiateWarehouseIntro());

        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_music, transform, 1, true, Manager_AudioMixer.instance.mixer_music);
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.OpenLoadingScreen();
    }

    private IEnumerator InitiateWarehouseIntro()
    {
        yield return new WaitForSeconds(4f);
        _warehouseIntro.InitiateOpenGarageDoor();
    }


}