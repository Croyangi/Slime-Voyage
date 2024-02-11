using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Warehouse_ElevatorPrompt : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D col_prompt;
    [SerializeField] private GameObject canvas_elevator;
    [SerializeField] private GameObject elevatorPanel;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("Building Block References")]
    [SerializeField] private Warehouse_Elevator _WarehouseElevator;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
    }

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.Enable();
    }

    private void OnDestroy()
    {
        //// Unubscribes to Unity's input system
        playerInput.Interact.Interact1.performed -= OnInteractPerformed;
        playerInput.Disable();
    }



    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        _WarehouseElevator.InitiateElevatorPanel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                playerInput.Interact.Interact1.performed += OnInteractPerformed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && CheckExistingObjects() == false && gameObject.activeSelf == true)
            {
                playerInput.Interact.Interact1.performed -= OnInteractPerformed;
                ForceQuitOutOfRange();
            }
        }
    }

    private void ForceQuitOutOfRange()
    {
        StartCoroutine(_WarehouseElevator.EndElevatorPanel());
    }

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_prompt, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(tag_player.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
