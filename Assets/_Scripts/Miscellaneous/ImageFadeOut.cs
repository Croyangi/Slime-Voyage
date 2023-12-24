using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeOut : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float startingAlpha;
    [SerializeField] private float targetAlpha;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, startingAlpha);
        StartCoroutine(FadeImage());
    }

    private IEnumerator FadeImage()
    {
        Color originalColor = image.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);

        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            image.color = Color.Lerp(originalColor, targetColor, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;
    }
}
