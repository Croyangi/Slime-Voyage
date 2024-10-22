using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Manager_PlayerState : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject prefab_baseSlime;

    [Header("Variables")]
    public bool isDead;
    public bool isInvincible;
    public bool isInputStall;
    public bool isResetDeathOn;

    [Header("Actions")]
    [SerializeField] public Action onPlayerMoldChanged;

    [Header("Building Blocks")]
    [SerializeField] private Warehouse_DeathTransition_Animator deathTransition_animator;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    public static Manager_PlayerState instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Player State Manager in the scene.");
        }
        instance = this;

        FindPlayer();

        playerInput = new PlayerInput(); // Instantiate new Unity's Input System

        isResetDeathOn = true;
    }

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.Interact.ResetDeath.performed += OnResetDeath;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        //// Unubscribes to Unity's input system
        playerInput.Interact.ResetDeath.performed -= OnResetDeath;
        playerInput.Disable();
    }

    private void OnResetDeath(InputAction.CallbackContext value)
    {
        Debug.Log("ON RESET DEATH");
        if (isResetDeathOn && Time.timeScale > 0f)
        {
            Debug.Log("INITIATING DEATH");
            InitiatePlayerDeath();
        }
    }

    private void FindPlayer()
    {
        Tags _tags;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Tags>() != null)
            {
                _tags = obj.GetComponent<Tags>();
                if (_tags.CheckTags("Player") == true && obj != player)
                {
                    player = obj;
                    onPlayerMoldChanged?.Invoke();
                    return;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (player == null) { FindPlayer(); }
    }

    public void SetInputStall(bool state)
    {
        isInputStall = state;
        player.GetComponent<IMovementProcessor>().SetInputStall(state);
    }

    public void SetResetDeath(bool state)
    {
        isResetDeathOn = state;
    }

    public void InitiatePlayerDeath()
    {
        if (!isInvincible)
        {
            isDead = true;
            isResetDeathOn = false;

            player.GetComponentInChildren<IPlayerProcessor>().InitiatePlayerDeath();
            deathTransition_animator.PlayDeathTransitionClose();
            StartCoroutine(WaitForDeathTransition());
        }
    }

    private IEnumerator WaitForDeathTransition()
    {
        yield return new WaitForSeconds(0.5f);
        if (isDead)
        {
            EndPlayerDeath();
            yield return new WaitForSeconds(0.5f);
            isResetDeathOn = true;
        }
    }

    private void EndPlayerDeath()
    {
        deathTransition_animator.PlayDeathTransitionOpen();
        isDead = false;

        // Re-instantiate player
        GameObject baseSlimeInstance = Instantiate(prefab_baseSlime, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
        player = baseSlimeInstance;

        player.transform.position = Manager_RespawnPoint.instance.respawnPointPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // Data save
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
    }

    public void SaveData(ref GameData data)
    {
    }
}
