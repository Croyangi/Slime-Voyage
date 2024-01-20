using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_PlayerState : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] public GameObject player;

    [Header("Variables")]
    public bool isDead;

    [Header("Death Count Test")]
    [SerializeField] private TMP_Text test;
    [SerializeField] private int deathCount;

    [Header("Actions")]
    [SerializeField] public Action onPlayerMoldChanged;

    public static Manager_PlayerState instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Player State Manager in the scene.");
        }
        instance = this;

        FindPlayer();
    }

    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
        DeathCount();
    }

    public void SaveData(ref GameData data) 
    { 
        data.deathCount = this.deathCount;
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
        player.GetComponent<IMovementProcessor>().SetInputStall(state);
    }

    public void InitiatePlayerDeath()
    {
        isDead = true;
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        isDead = false;
        player.transform.position = Manager_RespawnPoint.instance.respawnPointPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.deathCount++;
        DeathCount();
        DataPersistenceManager.instance.SaveGame();
    }

    private void DeathCount()
    {
        test.text = "Death Count: " + this.deathCount;
    }
}
