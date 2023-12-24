using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GeneralButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private GameObject button;
    [SerializeField] private bool isMultipleObjects;

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

    private void ChangeButtonColor(Color color)
    {
        if (isMultipleObjects)
        {
            Image[] images = button.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                image.color = color;
            }
        }
        else
        {
            button.GetComponent<Image>().color = color;
        }
    }
}
