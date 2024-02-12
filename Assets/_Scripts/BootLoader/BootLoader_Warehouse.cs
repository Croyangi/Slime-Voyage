using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BootLoader_Warehouse : MonoBehaviour
{
    [SerializeField] private Image closingTransition;

    private void Awake()
    {
        closingTransition.color = new Color(0f, 0f, 0f, 1f);
        LeanTween.color(closingTransition.rectTransform, new Color(0f, 0f, 0f, 0f), 1f).setEaseInCubic();
    }
}
