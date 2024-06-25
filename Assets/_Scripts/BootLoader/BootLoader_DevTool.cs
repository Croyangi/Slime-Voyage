using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BootLoader_DevTool : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private DevToolsInput _devToolsInput = null;

    [Header("Dev Tool References")]
    [SerializeField] private int toggleDevToolsCombination;
    [SerializeField] private GameObject enabledGroup;
    [SerializeField] private bool isDevToolsEnabled;


    private void Awake()
    {
        _devToolsInput = new DevToolsInput();
        enabledGroup.SetActive(false);
    }

    private void OnEnable()
    {
        _devToolsInput.Toggle.CombinationD.performed += OnToggleDevToolsPerformed;
        _devToolsInput.Toggle.CombinationD.canceled += OnToggleDevToolsCancelled;
        _devToolsInput.Toggle.CombinationE.performed += OnToggleDevToolsPerformed;
        _devToolsInput.Toggle.CombinationE.canceled += OnToggleDevToolsCancelled;
        _devToolsInput.Toggle.CombinationV.performed += OnToggleDevToolsPerformed;
        _devToolsInput.Toggle.CombinationV.canceled += OnToggleDevToolsCancelled;
        _devToolsInput.Enable();
    }

    private void OnDisable()
    {
        _devToolsInput.Toggle.CombinationD.performed -= OnToggleDevToolsPerformed;
        _devToolsInput.Toggle.CombinationD.canceled -= OnToggleDevToolsCancelled;
        _devToolsInput.Toggle.CombinationE.performed -= OnToggleDevToolsPerformed;
        _devToolsInput.Toggle.CombinationE.canceled -= OnToggleDevToolsCancelled;
        _devToolsInput.Toggle.CombinationV.performed -= OnToggleDevToolsPerformed;
        _devToolsInput.Toggle.CombinationV.canceled -= OnToggleDevToolsCancelled;

        _devToolsInput.Disable();
    }

    private void OnToggleDevToolsPerformed(InputAction.CallbackContext value)
    {
        toggleDevToolsCombination += 1;
        if (toggleDevToolsCombination >= 3)
        {
            ToggleDevTools();
        }
    }

    private void OnToggleDevToolsCancelled(InputAction.CallbackContext value)
    {
        toggleDevToolsCombination -= 1;
    }

    private void ToggleDevTools()
    {
        if (isDevToolsEnabled)
        {
            enabledGroup.SetActive(false);
            isDevToolsEnabled = false;
        }
        else
        {
            enabledGroup.SetActive(true);
            isDevToolsEnabled = true;
        }
    }
}
