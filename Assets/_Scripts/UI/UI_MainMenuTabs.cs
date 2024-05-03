using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MainMenuTabs : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Handler_MainMenu _mainMenu;
    [SerializeField] private int id;

    [Header("References")]
    [SerializeField] private GameObject button;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Color body_unhoveredColor = new Color(0, 0, 0, 130);
    [SerializeField] private Color body_hoveredColor = new Color(255, 255, 255, 255);

    [SerializeField] private Color text_unhoveredColor = new Color(255, 255, 255);
    [SerializeField] private Color text_hoveredColor = new Color(0, 0, 0);

    [SerializeField] private Vector3 unpressedScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 pressedScale = new Vector3(0.95f, 0.95f, 0.95f);

    [SerializeField] private Vector3 hoveredScale = new Vector3(1.05f, 1.05f, 1.05f);
    [SerializeField] private Vector3 unhoveredScale = new Vector3(1f, 1f, 1f);

    [SerializeField] private float scaleSpeed = 0.1f;

    [SerializeField] private AudioClip sfx_onHover;

    private void Awake()
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.scale(button, pressedScale, scaleSpeed).setIgnoreTimeScale(true);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LeanTween.scale(button, unpressedScale, scaleSpeed).setIgnoreTimeScale(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(button);
        _mainMenu.SetActiveTabVFX(id);
        LeanTween.scale(button, hoveredScale, scaleSpeed).setIgnoreTimeScale(true);

        button.GetComponent<Image>().color = body_hoveredColor;
        text.color = text_hoveredColor;

        if (sfx_onHover != null && Manager_SFXPlayer.instance != null)
        {
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_onHover, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.3f, isUnaffectedByTime: true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(button);
        _mainMenu.SetDeactiveTabVFX(id);
        LeanTween.scale(button, unhoveredScale, scaleSpeed).setIgnoreTimeScale(true);

        button.GetComponent<Image>().color = body_unhoveredColor;
        text.color = text_unhoveredColor;
    }

    //private void ChangeButtonColor(Color color)
    //{
    //    if (isMultipleObjects)
    //    {
    //        Image[] images = button.GetComponentsInChildren<Image>();
    //        foreach (Image image in images)
    //        {
    //            image.color = color;
    //        }
    //    }
    //    else
    //    {
    //        button.GetComponent<Image>().color = color;
    //    }
    //}
}
