using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_WarehouseDioramaMenu : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private AudioClip audioClip_employeesLament;
    [SerializeField] private ScriptObj_CheckpointQueue _checkpointQueue;

    [Header("Screen Transition")]
    public bool isTransitioning;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_activeScene;

    private void Awake()
    {
        _checkpointQueue.ClearCheckpoints();

        StartCoroutine(LoadLoadingScreen());
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene));
        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_employeesLament, transform, 0.5f, true, Manager_AudioMixer.instance.mixer_music);
    }

    private IEnumerator LoadLoadingScreen()
    {
        // Transition screen
        if (Manager_LoadingScreen.instance != null)
        {
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_loadingScreen, LoadSceneMode.Additive);

            // Wait until the asynchronous scene loading is complete
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("Finished");
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }
    }

    //private void ScreenFollowMouseUpdate()
    //{
    //    GetParallax();

    //    float rotationX = offset.y * parallaxScale * -1;
    //    float rotationY = offset.x * parallaxScale;
    //    Vector3 parallaxRotation = new Vector3(rotationX + cameraRotationOffset.x, rotationY + cameraRotationOffset.y, cameraRotationOffset.z);

    //    Quaternion desiredRotation = Quaternion.Slerp(Quaternion.Euler(parallaxRotation), _camera.transform.rotation, slerpScale);

    //    _camera.transform.rotation = desiredRotation;
    //}

    //private void GetParallax()
    //{
    //    Vector3 mousePosition = Input.mousePosition;
    //    offset = GetOffsetFromCenterScreen(parallaxOriginPoint, mousePosition);
    //}

    //private Vector2 GetOffsetFromCenterScreen(Vector2 pos1, Vector2 pos2)
    //{
    //    float distanceX = pos2.x - pos1.x;
    //    float distanceY = pos2.y - pos1.y;
    //    Vector2 distance = new Vector2(distanceX, distanceY);

    //    return distance;
    //}
}
