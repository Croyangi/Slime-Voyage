using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBreaking : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    [Header("References")]
    [SerializeField] private float fadeOutSpeed;
    [SerializeField] private SpriteRenderer sr_fan;
    [SerializeField] private SpriteRenderer sr_fanBox;
    [SerializeField] private GameObject fan;
    [SerializeField] private bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true && isUsed == false)
            {
                isUsed = true;

                // Reset player velocity
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 5f);

                // Apply velocity
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;

                rb.velocity = Vector2.zero;
                float randomX = Random.Range(-7f, -3f);
                float randomY = Random.Range(5f, 10f);
                rb.AddForce(new Vector2(randomX, randomY), ForceMode2D.Impulse);

                StartCoroutine(FadeOut());
                Destroy(gameObject, 2f);
            }
        }
    }

    private IEnumerator FadeOut()
    {
        float newAlpha = Mathf.MoveTowards(sr_fan.color.a, 0f, fadeOutSpeed * Time.deltaTime);
        sr_fan.color = new Color(sr_fan.color.r, sr_fan.color.g, sr_fan.color.b, newAlpha);
        sr_fanBox.color = new Color(sr_fanBox.color.r, sr_fanBox.color.g, sr_fanBox.color.b, newAlpha);
        yield return new WaitForFixedUpdate();
        StartCoroutine(FadeOut());
    }
}
