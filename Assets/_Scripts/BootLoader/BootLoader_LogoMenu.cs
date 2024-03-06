using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader_LogoMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_warehouseDioramaMenu;

    public void LoadMainMenu()
    {
        _sceneQueue.LoadScene(scene_warehouseDioramaMenu);
    }
}
