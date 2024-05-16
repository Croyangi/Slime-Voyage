using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ExitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private GameObject button;
    [SerializeField] private Image sr_icon;

    [SerializeField] private Color unpressedColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    [SerializeField] private Vector3 unpressedScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 pressedScale = new Vector3(0.95f, 0.95f, 0.95f);
    [SerializeField] private Vector3 hoverScale = new Vector3(1.05f, 1.05f, 1.05f);
    [SerializeField] private float scaleSpeed = 0.1f;

    [SerializeField] private Sprite closedDoor;
    [SerializeField] private Sprite openDoor;

    [Header("SFX References")]
    [SerializeField] private AudioClip generalUIHover;


    public void OnPointerDown(PointerEventData eventData)
    {
        // Change color
        ChangeButtonColor(pressedColor);

        // LeanTween scale
        LeanTween.scale(button, pressedScale, scaleSpeed).setIgnoreTimeScale(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Change color
        ChangeButtonColor(unpressedColor);

        // LeanTween scale
        LeanTween.scale(button, unpressedScale, scaleSpeed).setIgnoreTimeScale(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (generalUIHover != null)
        {
            Manager_SFXPlayer.instance.PlaySFXClip(generalUIHover, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.3f);
        }
        LeanTween.scale(button, hoverScale, scaleSpeed).setIgnoreTimeScale(true);

        sr_icon.sprite = openDoor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(button, unpressedScale, scaleSpeed).setIgnoreTimeScale(true);

        sr_icon.sprite = closedDoor;
    }

    private void ChangeButtonColor(Color color)
    {
        Image[] images = button.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.color = color;
        }
    }
}
