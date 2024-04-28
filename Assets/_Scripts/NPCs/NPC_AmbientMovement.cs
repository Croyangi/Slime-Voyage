using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_AmbientMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform originPoint;

    private void Awake()
    {
        originPoint = transform;
    }

    private void Start()
    {
        StartCoroutine(AmbientMovementRandomChance());
    }

    private IEnumerator AmbientMovementRandomChance()
    {
        gameObject.transform.position = originPoint.position;

        float random = Random.Range(3f, 5f);
        yield return new WaitForSeconds(random);
        JumpAround();

        StopAllCoroutines();
        StartCoroutine(AmbientMovementRandomChance());
    }

    private void JumpAround()
    {
        LeanTween.moveLocalY(gameObject, originPoint.position.y + 0.2f, 0.2f);
        LeanTween.moveLocalY(gameObject, originPoint.position.y, 0.1f).setDelay(0.3f);
        LeanTween.moveLocalY(gameObject, originPoint.position.y + 0.2f, 0.2f).setDelay(0.6f);
        LeanTween.moveLocalY(gameObject, originPoint.position.y, 0.1f).setDelay(0.9f);
    }
}
