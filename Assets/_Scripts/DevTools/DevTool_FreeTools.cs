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
    [SerializeField] private Vector2 cameraSpeedWeight;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float cameraSize;
    [SerializeField] private Rigidbody2D cameraRb;
    [SerializeField] private ScriptObj_DevToolsMovementVariables _movementVars;

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
        _movementVars.devTools.movementSpeed += 1f;
        ResetCameraSettings();
    }

    public void OnSubtractCameraSpeed()
    {
        _movementVars.devTools.movementSpeed -= 1f;
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
        StartCoroutine(FreeToolsUpdate());
    }

    private void FreeRoamUpdate()
    {
        player.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, player.transform.position.z);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //_camera.transform.position = new Vector3(_camera.transform.position.x + (inputMovement.x * cameraSpeed), _camera.transform.position.y + (inputMovement.y * cameraSpeed), _camera.transform.position.z);

        // Math.Sign is because Unity's input can give float values if diagonal movement
        float xTargetSpeed = inputMovement.x * _movementVars.devTools.movementSpeed * cameraSpeedWeight.x;
        float xSpeedDif = xTargetSpeed - cameraRb.velocity.x;
        float xAccelRate = (Mathf.Abs(xTargetSpeed) > 0.01f) ? _movementVars.devTools.acceleration : _movementVars.devTools.deceleration;
        float xMovement = Mathf.Pow(Mathf.Abs(xSpeedDif) * xAccelRate, _movementVars.devTools.velocityPower) * Mathf.Sign(xSpeedDif);

        float yTargetSpeed = inputMovement.y * _movementVars.devTools.movementSpeed * cameraSpeedWeight.y;
        float ySpeedDif = yTargetSpeed - cameraRb.velocity.y;
        float yAccelRate = (Mathf.Abs(yTargetSpeed) > 0.01f) ? _movementVars.devTools.acceleration : _movementVars.devTools.deceleration;
        float yMovement = Mathf.Pow(Mathf.Abs(ySpeedDif) * yAccelRate, _movementVars.devTools.velocityPower) * Mathf.Sign(ySpeedDif);

        cameraRb.AddForce(new Vector2(xMovement, yMovement));
    }

    private IEnumerator FreeToolsUpdate()
    {
        if (isFreeRoamEnabled && player != null)
        {
            FreeRoamUpdate();
        }

        if (isFreeCameraEnabled)
        {
            FreeCameraUpdate();
        }

        yield return new WaitForFixedUpdate();
        if (isFreeCameraEnabled || isFreeRoamEnabled)
        {
            StartCoroutine(FreeToolsUpdate());
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

        StartCoroutine(FreeToolsUpdate());

    }

    private void FreeCameraUpdate()
    {
        player.transform.position = playerPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //_camera.transform.position = new Vector3(_camera.transform.position.x + (inputMovement.x * cameraSpeed), _camera.transform.position.y + (inputMovement.y * cameraSpeed), _camera.transform.position.z);
        // Math.Sign is because Unity's input can give float values if diagonal movement
        float xTargetSpeed = inputMovement.x * _movementVars.devTools.movementSpeed;
        float xSpeedDif = xTargetSpeed - cameraRb.velocity.x;
        float xAccelRate = (Mathf.Abs(xTargetSpeed) > 0.01f) ? _movementVars.devTools.acceleration : _movementVars.devTools.deceleration;
        float xMovement = Mathf.Pow(Mathf.Abs(xSpeedDif) * xAccelRate, _movementVars.devTools.velocityPower) * Mathf.Sign(xSpeedDif);

        float yTargetSpeed = inputMovement.y * _movementVars.devTools.movementSpeed;
        float ySpeedDif = yTargetSpeed - cameraRb.velocity.y;
        float yAccelRate = (Mathf.Abs(yTargetSpeed) > 0.01f) ? _movementVars.devTools.acceleration : _movementVars.devTools.deceleration;
        float yMovement = Mathf.Pow(Mathf.Abs(ySpeedDif) * yAccelRate, _movementVars.devTools.velocityPower) * Mathf.Sign(ySpeedDif);

        cameraRb.AddForce(new Vector2(xMovement, yMovement));
    }
}
