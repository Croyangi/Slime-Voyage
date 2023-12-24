using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BootLoader_WarehouseDioramaMenu : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject _camera;

    [Header("Screen Transition")]
    [SerializeField] private float screenTransitionTimer;
    [SerializeField] private bool pressedTicketButton;
    [SerializeField] private Image ticketButton;
    [SerializeField] private Sprite playTicketHolePunched;
    [SerializeField] private GameObject ticketButtonHolePunch;

    [Header("Screen Follow Mouse References")]
    [SerializeField] private Vector3 cameraRotationOffset;
    [SerializeField] private Vector2 parallaxOriginPoint;
    [SerializeField] private float parallaxScale;
    [SerializeField] private float slerpScale;
    [SerializeField] private Vector2 offset;
    [SerializeField] private RoomQueue _roomQueue;

    private void Awake()
    {
        parallaxOriginPoint = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    private void FixedUpdate()
    {
        PressTicketButtonUpdate();
        ScreenFollowMouseUpdate();
    }

    private void PressTicketButtonUpdate()
    {
        if (pressedTicketButton)
        {
            screenTransitionTimer -= Time.deltaTime;
            if (screenTransitionTimer < 0)
            {
                LoadWarehouseBootloader();
            }
        }
    }

    private void ScreenFollowMouseUpdate()
    {
        GetParallax();

        float rotationX = offset.y * parallaxScale * -1;
        float rotationY = offset.x * parallaxScale;
        Vector3 parallaxRotation = new Vector3(rotationX + cameraRotationOffset.x, rotationY + cameraRotationOffset.y, cameraRotationOffset.z);

        Quaternion desiredRotation = Quaternion.Slerp(Quaternion.Euler(parallaxRotation), _camera.transform.rotation, slerpScale);

        _camera.transform.rotation = desiredRotation;
    }

    private void GetParallax()
    {
        Vector3 mousePosition = Input.mousePosition;
        offset = GetOffsetFromCenterScreen(parallaxOriginPoint, mousePosition);
    }

    private Vector2 GetOffsetFromCenterScreen(Vector2 pos1, Vector2 pos2)
    {
        float distanceX = pos2.x - pos1.x;
        float distanceY = pos2.y - pos1.y;
        Vector2 distance = new Vector2(distanceX, distanceY);

        return distance;
    }

    public void PressTicketButton()
    {
        if (!pressedTicketButton)
        {
            ApplyForceTicketButton();
        }

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

    private void LoadWarehouseBootloader()
    {
        _roomQueue.LoadRoom("BootLoader_Warehouse");
    }
}
