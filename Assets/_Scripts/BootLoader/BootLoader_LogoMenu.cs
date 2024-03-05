using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BootLoader_LogoMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private SceneAsset scene_warehouseDioramaMenu;

    public void LoadMainMenu()
    {
        _sceneQueue.LoadSceneWithAsset(scene_warehouseDioramaMenu);
    }
}
