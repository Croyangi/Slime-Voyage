using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunkfish_Animator : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject chunkfish;
    [SerializeField] private SpriteRenderer chunkfish_sr;

    [Header("Special References")]
    [SerializeField] private Chunkfish_StateHandler _stateHandler;
    [SerializeField] private Chunkfish_Eyes _eyesHandler;

    [Header("State References")]
    [SerializeField] private string currentState;
    [SerializeField] private int currentPriority;
    [SerializeField] private float currentPriorityTime;

    const string CHUNKFISH_INFLATE = "Chunkfish_Inflate";
    const string CHUNKFISH_DEFLATE = "Chunkfish_Deflate";

    private void Awake()
    {
        RandomSpriteFlip();
    }

    private void FixedUpdate()
    {
        if (_stateHandler.isDetecting && _stateHandler.detectedObject != null)
        {
            FaceDetectedObject();
        }

        if (_stateHandler.isInflated)
        {
            ChangeAnimationState(CHUNKFISH_INFLATE);
        } else
        {
            ChangeAnimationState(CHUNKFISH_DEFLATE);
        }

        if (_stateHandler.isFullyInflated)
        {
            _eyesHandler.ToggleEyes(true);
        } else
        {
            _eyesHandler.ToggleEyes(false);
        }
    }

    private void RandomSpriteFlip()
    {
        if (GetRandomBool())
        {
            chunkfish_sr.flipX = false;
            _eyesHandler.FlipEyes(false);
        }
        else
        {
            chunkfish_sr.flipX = true;
            _eyesHandler.FlipEyes(true);
        }
    }

    private bool GetRandomBool()
    {
        return (Random.value > 0.5f);
    }

    private void ChangeAnimationState(string newState, int newPriority = 0)
    {
        if (currentState == newState)
        {
            return;
        }

        if (currentPriority > newPriority && currentPriorityTime > 0)
        {
            return;
        }

        _animator.Play(newState);
        currentState = newState;
        currentPriority = newPriority;
    }

    private void FaceDetectedObject()
    {
        if (_stateHandler.detectedObject.transform.position.x < chunkfish.transform.position.x)
        {
            chunkfish_sr.flipX = true;
            _eyesHandler.FlipEyes(true);
        }
        else
        {
            chunkfish_sr.flipX = false;
            _eyesHandler.FlipEyes(false);
        }
    }
}
