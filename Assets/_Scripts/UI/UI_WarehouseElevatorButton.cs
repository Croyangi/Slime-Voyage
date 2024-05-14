using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WarehouseElevatorButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private GameObject button;
    [SerializeField] private Image buttonOutline;

    [SerializeField] private Color unpressedColor;
    [SerializeField] private Color pressedColor;

    [SerializeField] private Color outline_hoveredColor = new Color(1f, 0.78f, 0f, 1f);
    [SerializeField] private Color outline_unhoveredColor = new Color(1f, 1f, 1f, 1f);

    [SerializeField] private Vector3 unpressedScale;
    [SerializeField] private Vector3 pressedScale;
    [SerializeField] private float scaleSpeed;

    [SerializeField] private AudioClip sfx_generalUIHover;


    public void OnPointerDown(PointerEventData eventData)
    {
        // Change color
        ChangeColor(button, pressedColor);

        // LeanTween scale
        LeanTween.scale(button, pressedScale, scaleSpeed).setIgnoreTimeScale(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Change color
        ChangeColor(button, unpressedColor);

        // LeanTween scale
        LeanTween.scale(button, unpressedScale, scaleSpeed).setIgnoreTimeScale(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sfx_generalUIHover != null && Manager_SFXPlayer.instance != null)
        {
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_generalUIHover, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.3f, isUnaffectedByTime: true);
        }

        buttonOutline.color = outline_hoveredColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonOutline.color = outline_unhoveredColor;
    }

    private void ChangeColor(GameObject gameObj, Color color)
    {
        Image image = gameObj.GetComponent<Image>();
        image.color = color;
    }
}
