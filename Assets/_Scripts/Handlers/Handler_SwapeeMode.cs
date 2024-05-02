using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handler_SwapeeMode : MonoBehaviour, IDataPersistence
{
    [SerializeField] private BootLoader_WarehouseDioramaMenu _bootLoader;
    [SerializeField] private ScriptObj_ModifierMode _modifierMode;

    [SerializeField] private string swapeeId;
    [SerializeField] private GameObject lockedObjects;

    [SerializeField] private Image ticketButton;
    [SerializeField] private Sprite playTicketHolePunched;
    [SerializeField] private GameObject ticketButtonHolePunch;

    [SerializeField] private AudioClip sfx_onPressMode;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_theWarehouse;
    [SerializeField] private string scene_deloadedScene;

    private void Awake()
    {
        _modifierMode.ResetModifiers();
    }

    public void OnPressSwapeeButton()
    {
        if (!_bootLoader.isTransitioning)
        {
            ApplyForceTicketButton();

            _modifierMode.isModified = true;
            _modifierMode.isSwapeeMode = true;

            LoadTheWarehouse();
            _bootLoader.isTransitioning = true;
            ticketButton.sprite = playTicketHolePunched;
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_onPressMode, transform, 1f, false, Manager_AudioMixer.instance.mixer_music);
        }
    }

    private void ApplyForceTicketButton()
    {
        ticketButtonHolePunch.SetActive(true);
        Rigidbody2D rb = ticketButtonHolePunch.GetComponent<Rigidbody2D>();
        float randomX = Random.Range(-10, 5);
        float randomY = Random.Range(30, 35);
        float randomTorque = Random.Range(-200, 200);

        rb.AddForce(new Vector2(randomX, randomY), ForceMode2D.Impulse);
        rb.AddTorque(randomTorque);
    }

    private void LoadTheWarehouse()
    {
        Manager_LoadingScreen.instance.InitiateLoadSceneTransfer(scene_theWarehouse, scene_deloadedScene);
    }

    private void UnlockSwapeeMode()
    {
        lockedObjects.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        bool isCompleted;
        data.areasCompleted.TryGetValue(swapeeId, out isCompleted);

        if (isCompleted)
        {
            UnlockSwapeeMode();
        }
    }

    public void SaveData(ref GameData data)
    {
        //
    }
}
