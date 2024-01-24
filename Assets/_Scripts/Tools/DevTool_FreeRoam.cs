using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevTool_FreeRoam : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject player;
    [SerializeField] private DevToolsInput _devToolsInput = null;
    [SerializeField] private Vector2 inputMovement;
    [SerializeField] private bool isFreeRoamEnabled;

    [Header("Camera References")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private float movementSpeed;
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
    }

    private void OnDisable()
    {
        _devToolsInput.CardinalDirections.Movement.performed -= OnMovementPerformed;
        _devToolsInput.CardinalDirections.Movement.canceled -= OnMovementCancelled;
        _devToolsInput.Disable();
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
        isFreeRoamEnabled = false;
        _camera.SetActive(false);
    }

    private void OnEnableFreeRoam()
    {
        if (player == null)
        {
            FindPlayer();
        }

        isFreeRoamEnabled = true;
        _camera.SetActive(true);
        _camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, _camera.transform.position.z);
    }

    private void FreeRoamUpdate()
    {
        player.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, player.transform.position.z);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _camera.transform.position = new Vector3(_camera.transform.position.x + (inputMovement.x * movementSpeed), _camera.transform.position.y + (inputMovement.y * movementSpeed), _camera.transform.position.z);
    }

    private void FixedUpdate()
    {
        if (isFreeRoamEnabled && player != null)
        {
            FreeRoamUpdate();
        }
    }
}
