//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UFOSlimeAnimator : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private UFOSlimeMovement ufoSlimeMovement;
//    [SerializeField] private Animator playerAnimator;
//    //[SerializeField] private PlayerStateScriptObj playerStateScriptObj;
//    [SerializeField] private PlayerMovementScriptObj playerMovementScriptObj;

//    private void FixedUpdate()
//    {
//        // Animations
//        IdleAnimation();

//        MovingAnimation();
//    }

//    private void IdleAnimation()
//    {
//        if (!IsPressingUp() && !IsPressingDown())
//        {
//            playerAnimator.SetBool("UFOSlimeIdle", true);
//        } else
//        {
//            playerAnimator.SetBool("UFOSlimeIdle", false);
//        }
//    }

//    private void MovingAnimation()
//    {
//        playerAnimator.SetBool("UFOSlimeUp", false);
//        playerAnimator.SetBool("UFOSlimeDown", false);

//        if (IsPressingUp())
//        {
//            playerAnimator.SetBool("UFOSlimeUp", true);
//        }
//        else if (IsPressingDown())
//        {
//            playerAnimator.SetBool("UFOSlimeDown", true);
//        }
//    }


//    ////////

//    private bool IsPressingUp()
//    {
//        if (playerMovementScriptObj.ufoSlime.processedInputMovement.y > 0)
//        {
//            return true;
//        }
//        return false;
//    }

//    private bool IsPressingDown()
//    {
//        if (playerMovementScriptObj.ufoSlime.processedInputMovement.y < 0)
//        {
//            return true;
//        }
//        return false;
//    }

//}
