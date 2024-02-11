using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevTool_FreeTools : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject player;
    [SerializeField] private DevToolsInput _devToolsInput = null;
    [SerializeField] private Vector2 inputMovement;
    [SerializeField] private bool isFreeRoamEnabled;
    [SerializeField] private bool isFreeCameraEnabled;
    [SerializeField] private Vector3 playerPosition;

    [Header("Camera References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float cameraSize;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    private void Awake()
    {
        _devToolsInput = new DevToolsInput();
        inputMovement.x = 0;
        inputMovement.y = 0;
    }

    private void OnEnable()
    {
        _devToolsInput.CardinalDirections.Movement.performed += OnMovementPerformed;
        _devToolsInput.CardinalDirections.Movement.canceled += OnMovementCancelled;
        _devToolsInput.Enable();

        if (player != null)
        {
            _camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, _camera.transform.position.z);
        }

        _camera.orthographicSize = cameraSize;
    }

    private void OnDisable()
    {
        Manager_PlayerState.instance.isInvincible = false;
        _devToolsInput.CardinalDirections.Movement.performed -= OnMovementPerformed;
        _devToolsInput.CardinalDirections.Movement.canceled -= OnMovementCancelled;
        _devToolsInput.Disable();

        OnDisableFreeRoam();
        OnDisableFreeCamera();
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        inputMovement.x = 0;
        inputMovement.y = 0;
    }

    public void OnAddCameraSize()
    {
        cameraSize += 1f;
        ResetCameraSettings();
    }

    public void OnSubtractCameraSize()
    {
        cameraSize -= 1f;
        ResetCameraSettings();
    }

    public void OnAddCameraSpeed()
    {
        cameraSpeed += 0.3f;
        ResetCameraSettings();
    }

    public void OnSubtractCameraSpeed()
    {
        cameraSpeed -= 0.3f;
        ResetCameraSettings();
    }

    private void ResetCameraSettings()
    {
        cameraSpeed = Mathf.Clamp(cameraSpeed, 1, 9999);
        cameraSize = Mathf.Clamp(cameraSize, 1, 9999);
        _camera.orthographicSize = cameraSize;
    }


    private void FindPlayer()
    {
        Tags _tags;

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject thisObject in allObjects)

            if (thisObject.GetComponent<Tags>() != null)
            {
                _tags = thisObject.GetComponent<Tags>();
                if (_tags.CheckTags(_playerTag.name) == true)
                {
                    //Debug.Log("Successfully found GameObject with tag: " + tag);
                    player = thisObject;
                }
            }
    }

    public void ToggleFreeRoam()
    {
        if (isFreeRoamEnabled)
        {
            OnDisableFreeRoam();
        } else
        {
            OnEnableFreeRoam();
        }
    }

    private void OnDisableFreeRoam()
    {
        Manager_PlayerState.instance.isInvincible = false;
        isFreeRoamEnabled = false;
        _camera.enabled = false;
    }

    private void OnEnableFreeRoam()
    {
        if (player == null)
        {
            FindPlayer();
        }

        OnDisableFreeCamera();
        Manager_PlayerState.instance.isInvincible = true;
        isFreeRoamEnabled = true;
        _camera.enabled = true;
        _camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, _camera.transform.position.z);
    }

    private void FreeRoamUpdate()
    {
        player.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, player.transform.position.z);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _camera.transform.position = new Vector3(_camera.transform.position.x + (inputMovement.x * cameraSpeed), _camera.transform.position.y + (inputMovement.y * cameraSpeed), _camera.transform.position.z);
    }

    private void FixedUpdate()
    {
        if (isFreeRoamEnabled && player != null)
        {
            FreeRoamUpdate();
        }

        if (isFreeCameraEnabled)
        {
            FreeCameraUpdate();
        }
    }

    //
    // Toggle Free Camera
    //
    public void ToggleFreeCamera()
    {
        if (isFreeCameraEnabled)
        {
            OnDisableFreeCamera();
        }
        else
        {
            OnEnableFreeCamera();
        }
    }

    private void OnDisableFreeCamera()
    {
        Manager_PlayerState.instance.isInvincible = false;
        isFreeCameraEnabled = false;
        _camera.enabled = false;
    }

    private void OnEnableFreeCamera()
    {
        if (player == null)
        {
            FindPlayer();
        }

        OnDisableFreeRoam();
        Manager_PlayerState.instance.isInvincible = true;
        isFreeCameraEnabled = true;
        _camera.enabled = true;
        _camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, _camera.transform.position.z);

        // Keeps the player still, Vector2.zero does not prevent gravity
        playerPosition = player.transform.position;

    }

    private void FreeCameraUpdate()
    {
        player.transform.position = playerPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _camera.transform.position = new Vector3(_camera.transform.position.x + (inputMovement.x * cameraSpeed), _camera.transform.position.y + (inputMovement.y * cameraSpeed), _camera.transform.position.z);
    }
}
