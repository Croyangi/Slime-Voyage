using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunkfish_Eyes : Chunkfish_State
{
    [Header("General References")]
    [SerializeField] private GameObject chunkfish;

    [Header("Special References")]
    [SerializeField] private SpriteRenderer _eyes;
    [SerializeField] private Sprite[] _directionalEyes;

    [SerializeField] private Chunkfish_StateHandler _stateHandler;

    public override void FixedUpdateState()
    {
        if (_stateHandler.isDetecting && _stateHandler.detectedObject != null)
        {
            GetEyesAngle();
        }
    }

    private void GetEyesAngle()
    {
        float x1 = chunkfish.transform.position.x;
        float x2 = _stateHandler.detectedObject.transform.position.x;
        float y1 = chunkfish.transform.position.y;
        float y2 = _stateHandler.detectedObject.transform.position.y;
        float angle = Mathf.Atan2(y2 - y1, x2 - x1) * (180 / Mathf.PI);
        angle = ConvertAngleTo360(angle);
        SetEyesDirection(angle);
    }

    private float ConvertAngleTo360(float angle)
    {
        return (angle + 360) % 360;
    }

    private void SetEyesDirection(float angle)
    {

        if (angle >= 337.5 && angle < 22.5) // 1 // right side
        {
            _eyes.sprite = _directionalEyes[0];
        }
        else if (angle >= 22.5 && angle < 67.5) // 2 // right side
        {
            _eyes.sprite = _directionalEyes[1];
        }
        else if (angle >= 67.5 && angle < 112.5) // 3 // stays the same
        {
            _eyes.sprite = _directionalEyes[2];
        }
        else if (angle >= 112.5 && angle < 157.5) // 4 // left side
        {
            _eyes.sprite = _directionalEyes[1];
        }
        else if (angle >= 157.5 && angle < 202.5) // 5 // left side
        {
            _eyes.sprite = _directionalEyes[0];
        }
        else if (angle >= 202.5 && angle < 247.5) // 6 // left side
        {
            _eyes.sprite = _directionalEyes[7];
        }
        else if (angle >= 247.5 && angle < 292.5) // 7 // stays the same
        {
            _eyes.sprite = _directionalEyes[6];
        }
        else if (angle >= 292.5 && angle < 337.5) // 8 // right side
        {
            _eyes.sprite = _directionalEyes[7];
        }
        else
        {
            _eyes.sprite = _directionalEyes[0];
        }


    }
    public void FlipEyes(bool isFlipped)
    {
        if (isFlipped)
        {
            _eyes.flipX = true;
        }
        else
        {
            _eyes.flipX = false;
        }
    }

    public void ToggleEyes(bool toggleState)
    {
        if (toggleState)
        {
            _eyes.enabled = true;
        } else
        {
            _eyes.enabled = false;
        }
    }
}
