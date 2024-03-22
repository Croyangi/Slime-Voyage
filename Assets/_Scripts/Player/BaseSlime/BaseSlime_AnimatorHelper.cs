using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_AnimatorHelper : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;

    [Header("Technical References")]
    //[SerializeField] private float currentHighestImpactVelocityY;
    //[SerializeField] private float uniqueIdleAnimationTimer;
    //[SerializeField] private float uniqueIdleAnimationCooldown;

    [Header("State References")]

    //[SerializeField] private bool isSpriteFlippedX;
    [SerializeField] private string currentState;
    //[SerializeField] private int currentPriority;
    //[SerializeField] private float currentPriorityTime;

    public string BASESLIME_COMPRESS = "BaseSlime_Compress";
    public string BASESLIME_IDLE = "BaseSlime_Idle";
    public string BASESLIME_IDLE_SLIMEPILLED = "BaseSlime_Idle_SlimePilled";
    public string BASESLIME_IDLE_SPIN = "BaseSlime_Idle_Spin";
    public string BASESLIME_IDLE_STRETCH = "BaseSlime_Idle_Stretch";
    public string BASESLIME_LIGHTSPLAT = "BaseSlime_LightSplat";
    public string BASESLIME_LOOKINGUP = "BaseSlime_LookingUp";
    public string BASESLIME_MIDAIR = "BaseSlime_Midair";
    public string BASESLIME_MOVING = "BaseSlime_Moving";
    public string BASESLIME_FALLING = "BaseSlime_Falling";
    public string BASESLIME_RISING = "BaseSlime_Rising";
    public string BASESLIME_SPLAT = "BaseSlime_Splat";
    public string BASESLIME_STICK = "BaseSlime_Stick";
    public string BASESLIME_ONEDGE = "BaseSlime_OnEdge";



    [Header("Building Block References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;

    private void Awake()
    {
        //isSpriteFlippedX = sr.flipX;
    }

    public void ChangeAnimationState(string newState, int newPriority = 0)
    {
        if (currentState == newState)
        {
            return;
        }

        /*if (currentPriority > newPriority && currentPriorityTime > 0)
        {
            return;
        }*/

        animator.Play(newState);
        currentState = newState;
        //currentPriority = newPriority;
    }

    public void FlipSprite(bool flipDirection)
    {
        sr.flipX = flipDirection;
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

        if (_helper._movementVars.processedInputMovement.x == 1)
        {
            FlipSprite(false);
        }
        else if (_helper._movementVars.processedInputMovement.x == -1)
        {
            FlipSprite(true);
        }
    }
}
