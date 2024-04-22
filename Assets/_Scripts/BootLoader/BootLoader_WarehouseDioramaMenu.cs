using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BootLoader_WarehouseDioramaMenu : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private AudioClip audioClip_employeesLament;

    [Header("Screen Transition")]
    [SerializeField] private float screenTransitionDelay;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_warehousePrologue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_warehouseDioramaMenu;

    private void Awake()
    {
        if (Manager_LoadingScreen.instance == null)
        {
            _sceneQueue.LoadScene(scene_loadingScreen, true);
        }
        StartCoroutine(DelayedAwake());
    }

    private void Start()
    {
        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_employeesLament, transform, 0.5f, false, Manager_AudioMixer.instance.mixer_music);
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.OpenLoadingScreen();
    }

    public void StartScreenTransition()
    {
        StartCoroutine(LoadWarehousePrologue());
    }

    private IEnumerator LoadWarehousePrologue()
    {
        yield return new WaitForSeconds(screenTransitionDelay);
        Manager_LoadingScreen.instance.CloseLoadingScreen();
        yield return new WaitForSeconds(3);
        Manager_LoadingScreen.instance.OnLoadSceneTransfer(scene_warehousePrologue, scene_warehouseDioramaMenu);
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
