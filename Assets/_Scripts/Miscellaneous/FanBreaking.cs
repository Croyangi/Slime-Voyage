using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBreaking : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

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

                Destroy(gameObject, 3f);
            }
        }
    }
}
