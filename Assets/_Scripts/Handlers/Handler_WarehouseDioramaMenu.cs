using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handler_WarehouseDioramaMenu : MonoBehaviour
{
    [SerializeField] private BootLoader_WarehouseDioramaMenu _bootLoader;
    [SerializeField] private ScriptObj_CheckpointQueue _checkpointQueue;

    [SerializeField] private Image ticketButton;
    [SerializeField] private Sprite playTicketHolePunched;
    [SerializeField] private GameObject ticketButtonHolePunch;

    [SerializeField] private GameObject[] tabs;
    [SerializeField] private int currentTabId;
    [SerializeField] private GameObject[] menus;

    [SerializeField] private AudioClip sfx_onPressMode;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_deloadedScene;
    [SerializeField] private string scene_loadedScene;

    private void Awake()
    {
        currentTabId = 0;
        SetTabButton();
        SetMenu();
    }

    public void OnPressTicketButton()
    {
        if (!_bootLoader.isTransitioning)
        {
            ApplyForceTicketButton();

            _checkpointQueue.checkpointId = "WarehouseIntro";

            TransitionToScene();
            _bootLoader.isTransitioning = true;
            ticketButton.sprite = playTicketHolePunched;
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_onPressMode, transform, 1f, false, Manager_AudioMixer.instance.mixer_music);
        }
    }

    private void TransitionToScene()
    {
        Manager_LoadingScreen.instance.InitiateLoadSceneTransfer(scene_loadedScene, scene_deloadedScene);
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

    public void OnClickTabButton(int id)
    {
        if (currentTabId != id)
        {
            currentTabId = id;
            SetTabButton();
            SetMenu();
        }
    }

    private void SetTabButton()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (currentTabId == i)
            {
                SetActiveTabVFX(tabs[i]);
            } else
            {
                SetDeactiveTabVFX(tabs[i]);
            }
        }
    }

    private void SetMenu()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (currentTabId == i)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }

    private void SetActiveTabVFX(GameObject tab)
    {
        LeanTween.moveX(tab.GetComponent<RectTransform>(), 0, 0.5f).setEaseInBack().setEaseOutBounce();
    }

    private void SetDeactiveTabVFX(GameObject tab)
    {
        LeanTween.moveX(tab.GetComponent<RectTransform>(), 40, 0.5f).setEaseInBack().setEaseOutBounce();
    }

    public void OnReturnToMenuButtonPressed()
    {
        scene_loadedScene = "MainMenu";
        TransitionToScene();
        _bootLoader.isTransitioning = true;
    }
}
