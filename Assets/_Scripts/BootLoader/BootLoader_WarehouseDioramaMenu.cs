using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BootLoader_WarehouseDioramaMenu : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject _camera;

    [Header("Screen Transition")]
    [SerializeField] private float screenTransitionDelay;
    [SerializeField] private bool pressedTicketButton;

    [SerializeField] private Image ticketButton;
    [SerializeField] private Sprite playTicketHolePunched;
    [SerializeField] private GameObject ticketButtonHolePunch;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_warehousePrologue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_warehouseDioramaMenu;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_loadingScreen, true);
        StartCoroutine(DelayedAwake());
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.OpenLoadingScreen();
    }

    public void OnPressTicketButton()
    {
        if (!pressedTicketButton)
        {
            ApplyForceTicketButton();
        }

        StartCoroutine(OnPressTicketButtonVFX());
        pressedTicketButton = true;
        ticketButton.sprite = playTicketHolePunched;
    }

    private void ApplyForceTicketButton()
    {
        ticketButtonHolePunch.SetActive(true);
        Rigidbody2D rb = ticketButtonHolePunch.GetComponent<Rigidbody2D>();
        float randomX = Random.Range(-20, 20);
        float randomY = Random.Range(40, 60);
        float randomTorque = Random.Range(-200, 200);

        rb.AddForce(new Vector2(randomX, randomY), ForceMode2D.Impulse);
        rb.AddTorque(randomTorque);
    }

    private IEnumerator OnPressTicketButtonVFX()
    {
        yield return new WaitForSeconds(screenTransitionDelay);
        StartCoroutine(LoadWarehousePrologue());
    }

    private IEnumerator LoadWarehousePrologue()
    {
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
