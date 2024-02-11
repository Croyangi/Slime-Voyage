using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WarehouseGeneralButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private GameObject button;
    [SerializeField] private Image buttonOutline;

    [SerializeField] private Color unpressedColor;
    [SerializeField] private Color pressedColor;

    [SerializeField] private Vector3 unpressedScale;
    [SerializeField] private Vector3 pressedScale;
    [SerializeField] private float scaleSpeed;


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
        buttonOutline.color = new Color(1f, 0.78f, 0f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonOutline.color = new Color(1f, 1f, 1f, 1f);
    }

    private void ChangeButtonColor(Color color)
    {
        Image image = button.GetComponent<Image>();
        image.color = color;
    }
}
