using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_AnimatorHelper : MonoBehaviour
{
    [Header("General References")]
    public Animator baseSlime_animator;
    public Animator eyes_animator;
    [SerializeField] private SpriteRenderer baseSlime_sr;
    [SerializeField] private SpriteRenderer eyes_sr;
    [SerializeField] private GameObject eyes;
    [SerializeField] private GameObject baseSlime;
    public Vector2 eyesOffset { get; private set; }

    [Header("Technical References")]
    //[SerializeField] private float uniqueIdleAnimationTimer;
    //[SerializeField] private float uniqueIdleAnimationCooldown;

    [Header("State References")]

    //[SerializeField] private bool isSpriteFlippedX;
    public string currentState;
    //[SerializeField] private int currentPriority;
    //[SerializeField] private float currentPriorityTime;

    public string BASESLIME_COMPRESS = "BaseSlime_Compress";
    public string BASESLIME_IDLE = "BaseSlime_Idle";
    public string BASESLIME_LIGHTSPLAT = "BaseSlime_LightSplat";
    public string BASESLIME_LOOKINGUP = "BaseSlime_LookingUp";
    public string BASESLIME_MIDAIR = "BaseSlime_Midair";
    public string BASESLIME_MOVING = "BaseSlime_Moving";
    public string BASESLIME_FALLING = "BaseSlime_Falling";
    public string BASESLIME_RISING = "BaseSlime_Rising";
    public string BASESLIME_SPLAT = "BaseSlime_Splat";
    public string BASESLIME_STICKING = "BaseSlime_Sticking";
    public string BASESLIME_STICKINGTRANSITION = "BaseSlime_StickingTransition";
    public string BASESLIME_ONEDGE = "BaseSlime_OnEdge";

    public string EYES_MOVING = "BaseSlime_Eyes_Moving";
    public string EYES_IDLE = "BaseSlime_Eyes_Idle";
    public string EYES_STICKING = "BaseSlime_Eyes_Sticking";
    public string EYES_LOOKINGUP = "BaseSlime_Eyes_LookingUp";
    public string EYES_ONEDGE = "BaseSlime_Eyes_OnEdge";
    public string EYES_COMPRESSED = "BaseSlime_Eyes_Compressed";
    public string EYES_AIRBORNE = "BaseSlime_Eyes_Airborne";
    public string EYES_SCARED = "BaseSlime_Eyes_Scared";

    public string BASESLIME_UNIQUEIDLE_SLIMEPILLED = "BaseSlime_UniqueIdle_SlimePilled";
    public string BASESLIME_UNIQUEIDLE_SPIN = "BaseSlime_UniqueIdle_Spin";
    public string BASESLIME_UNIQUEIDLE_STRETCH = "BaseSlime_UniqueIdle_Stretch";

    [Header("Building Block References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;

    private void Awake()
    {
        //isSpriteFlippedX = sr.flipX;
    }

    public void ChangeAnimationState(string newState, Animator controller, int newPriority = 0)
    {
        if (currentState == newState)
        {
            return;
        }

        /*if (currentPriority > newPriority && currentPriorityTime > 0)
        {
            return;
        }*/

        controller.Play(newState);
        currentState = newState;
        //currentPriority = newPriority;
    }

    public void FlipSprite(bool flipDirection)
    {
        baseSlime_sr.flipX = flipDirection;
        eyes_sr.flipX = flipDirection;
    }

    private void FixedUpdate()
    {
        SpriteFlipUpdate();
    }

    private void SpriteFlipUpdate()
    {
        if (_helper.rb.velocity.x > 0.1)
        {
            FlipSprite(false);
        }
        else if (_helper.rb.velocity.x < -0.1)
        {
            FlipSprite(true);
        }

        if (_helper.facingDirection == 1)
        {
            FlipSprite(false);
        }
        else if (_helper.facingDirection == -1)
        {
            FlipSprite(true);
        }
    }

    public void SetEyesActive(bool isActive)
    {
        eyes_sr.enabled = isActive;
    }

    public void SetEyesOffset(Vector2 offset)
    {
        eyesOffset = offset;
    }
}
