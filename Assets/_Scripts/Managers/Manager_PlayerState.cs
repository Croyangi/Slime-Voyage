using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_PlayerState : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("Variables")]
    public bool isDead;

    [Header("Death Count Test")]
    [SerializeField] private TMP_Text test;
    [SerializeField] private int deathCount;

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
                if (_tags.CheckTags("Player") == true)
                {
                    player = obj;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (player == null) { FindPlayer(); }


    }

    public void InitiatePlayerDeath()
    {
        isDead = true;
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        isDead = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads current scene, might have to change it in the future
        player.transform.position = Manager_RespawnPoint.instance.respawnPointPosition;
        this.deathCount++;
        DeathCount();
        DataPersistenceManager.instance.SaveGame();
    }

    private void DeathCount()
    {
        test.text = "Death Count: " + this.deathCount;
    }
}
