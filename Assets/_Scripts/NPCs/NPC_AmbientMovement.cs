using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_AmbientMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector3 originPoint;

    private void Awake()
    {
        originPoint = gameObject.transform.localPosition;
    }

    private void Start()
    {
        StartCoroutine(AmbientMovementRandomChance());
    }

    private IEnumerator AmbientMovementRandomChance()
    {
        gameObject.transform.localPosition = new Vector2(originPoint.x, originPoint.y);

        float random = Random.Range(1f, 3f);
        yield return new WaitForSeconds(random);
        JumpAround();

        StopAllCoroutines();
        StartCoroutine(AmbientMovementRandomChance());
    }

    private void JumpAround()
    {
        LeanTween.moveLocalY(gameObject, originPoint.y + 0.2f, 0.2f);
        LeanTween.moveLocalY(gameObject, originPoint.y, 0.1f).setDelay(0.3f);
        LeanTween.moveLocalY(gameObject, originPoint.y + 0.2f, 0.2f).setDelay(0.6f);
        LeanTween.moveLocalY(gameObject, originPoint.y, 0.1f).setDelay(0.9f);
    }
}
