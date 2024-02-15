using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_Warehouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image closingTransition;

    private void Awake()
    {
        SceneManager.LoadScene("Bootloader_Pause", LoadSceneMode.Additive);
        SceneManager.LoadScene("Bootloader_DevTools", LoadSceneMode.Additive);
        SceneManager.LoadScene("Bootloader_Global", LoadSceneMode.Additive);
    }

    private void FadeOpenTransition()
    {
        closingTransition.color = new Color(0f, 0f, 0f, 1f);
        LeanTween.color(closingTransition.rectTransform, new Color(0f, 0f, 0f, 0f), 1f).setEaseInCubic();
    }
}
