using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handler_CheckpointLoader : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string[] ids;
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private GameObject[] checkpointIcons;
    [SerializeField] private ScriptObj_CheckpointQueue _checkpointQueue;
    [SerializeField] private BootLoader_WarehouseDioramaMenu _bootLoader;
    [SerializeField] private Sprite activeCheckpoint;

    [SerializeField] private AudioClip sfx_onPressMode;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_theWarehouse;
    [SerializeField] private string scene_deloadedScene;

    private void Start()
    {
        // Reset
        _checkpointQueue.checkpointId = "";
        //Debug.Log("RESSSETTTING");
        Debug.Log("Checkpoint ID: " + _checkpointQueue.checkpointId);
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (data.checkpointsReached.TryGetValue(ids[i], out bool isReached) && isReached == true)
            {
                checkpointIcons[i].GetComponent<Image>().sprite = activeCheckpoint;
                checkpoints[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
    }

    public void OnCheckpointButtonClicked(int id)
    {
        if (_bootLoader.isTransitioning == false)
        {
            _checkpointQueue.checkpointId = ids[id];
            Debug.Log(_checkpointQueue.checkpointId);

            StartCoroutine(LoadTheWarehouse());
            _bootLoader.isTransitioning = true;
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_onPressMode, transform, 1f, false, Manager_AudioMixer.instance.mixer_music);
        }

    }

    private IEnumerator LoadTheWarehouse()
    {
        Manager_LoadingScreen.instance.CloseLoadingScreen();
        yield return new WaitForSeconds(3f);
        Manager_LoadingScreen.instance.OnLoadSceneTransfer(scene_theWarehouse, scene_deloadedScene);
    }
}
