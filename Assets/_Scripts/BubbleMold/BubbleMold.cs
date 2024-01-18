using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMold : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer bubbleMoldSr;
    [SerializeField] private SpriteRenderer bubbleMoldInsideSr;
    [SerializeField] private GameObject bubbleMold;
    [SerializeField] private bool isUsed;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private ParticleSystem bubbleBurst;
    [SerializeField] private float regenerationTime;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;
    [SerializeField] private TagsScriptObj _currentSlimeTag;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float _amplitude = 0;
    [SerializeField] private float _frequency = 1;
    [SerializeField] private float _amplitudeRotate = 0;
    [SerializeField] private float _frequencyRotate = 1;

    private void FixedUpdate()
    {
        float y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        float rotateZ = Mathf.Sin(Time.time * _frequencyRotate) * _amplitudeRotate;
        bubbleMold.transform.position = new Vector2(bubbleMold.transform.position.x, bubbleMold.transform.position.y + y);
        bubbleMold.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true && isUsed == false && _tags.CheckTags(_currentSlimeTag.name) == false)
            {
                ChangeToGhostSlime();
                bubbleBurst.Play();
                StopCoroutine(RegenerateBubble());
                StartCoroutine(RegenerateBubble());
            }
        }
    }

    private void ChangeToGhostSlime()
    {
        isUsed = true;
        Vector3 playerPos = Manager_PlayerState.instance.player.transform.position;
        GameObject GhostSlime = Instantiate(slimePrefab, playerPos, Quaternion.identity);
        GhostSlime.GetComponent<Rigidbody2D>().velocity = Manager_PlayerState.instance.player.GetComponent<Rigidbody2D>().velocity;

        Destroy(Manager_PlayerState.instance.player);
    }

    private IEnumerator RegenerateBubble()
    {
        float remainingTime = regenerationTime;
        bubbleMoldSr.color = new Color(bubbleMoldSr.color.r, bubbleMoldSr.color.b, bubbleMoldSr.color.g, 0);
        bubbleMoldInsideSr.color = new Color(bubbleMoldSr.color.r, bubbleMoldSr.color.b, bubbleMoldSr.color.g, 0);

        while (remainingTime > 0)
        {

            remainingTime -= Time.deltaTime;

            float currentAlpha = 1 - (remainingTime / regenerationTime);
            bubbleMoldSr.color = new Color(bubbleMoldSr.color.r, bubbleMoldSr.color.b, bubbleMoldSr.color.g, currentAlpha);
            yield return new WaitForFixedUpdate();
        }

        bubbleMoldInsideSr.color = new Color(bubbleMoldSr.color.r, bubbleMoldSr.color.b, bubbleMoldSr.color.g, 1);
        isUsed = false;
    }
}
