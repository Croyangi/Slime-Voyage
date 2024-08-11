using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunModeButtonTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tm_speedrunModeState;
    [SerializeField] private bool isSpeedrunModeOn;
    [SerializeField] private Handler_WarehouseDioramaMenu _menu;
    [SerializeField] private BootLoader_WarehouseDioramaMenu _bootLoader;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private ScriptObj_SceneName scene_movementGym;

    public void OnToggleResetDataClicked()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void TransitionToScene()
    {
        if (!_bootLoader.isTransitioning)
        {
            _bootLoader.isTransitioning = true;
            Manager_LoadingScreen.instance.InitiateLoadSceneTransfer(scene_movementGym.name);
        }
    }
}
