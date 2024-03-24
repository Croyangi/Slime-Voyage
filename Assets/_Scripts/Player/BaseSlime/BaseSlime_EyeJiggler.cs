using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_EyeJiggler : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private GameObject eyes;

    [SerializeField] private float lerpScale;
    [SerializeField] private float velocityXScale;

    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;

    private void FixedUpdate()
    {
        float desiredX = Mathf.Lerp(eyes.transform.position.x, anchorPoint.position.x + _animator.eyesOffset.x + (-(_helper.rb.velocity.x) * velocityXScale), lerpScale);

        float desiredY = Mathf.Lerp(eyes.transform.position.y, anchorPoint.position.y + _animator.eyesOffset.y, lerpScale);
        Vector2 desiredPos = new Vector2(desiredX, desiredY);

        eyes.transform.position = desiredPos;
    }
}
