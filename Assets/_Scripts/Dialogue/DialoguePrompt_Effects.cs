using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePrompt_Effects : GeneralCullerCommunicator
{
    [Header("References")]
    [SerializeField] private Vector2 initialDialoguePromptPosition;
    [SerializeField] private GameObject dialoguePrompt;
    [SerializeField] private GameObject activeDialoguePrompt;
    [SerializeField] private GameObject innerCircle;
    [SerializeField] private Color innerCircleInitialColor;
    [SerializeField] private CircleCollider2D col_prompt;
    [SerializeField] private CircleCollider2D col_promptEffects;

    [SerializeField] private bool isMovingEffects;

    [Header("LeanTween Settings")]
    [SerializeField] private Vector2 detectedScale;
    [SerializeField] private Vector2 undetectedScale;
    [SerializeField] private float detectedScaleTime;

    [SerializeField] private Vector2 activeScale;
    [SerializeField] private float activeScaleTime;


    [Header("Math Movement Settings")]
    [SerializeField] private float amplitude = 0;
    [SerializeField] private float frequency = 1;

    [SerializeField] private float yMoveSpeed;
    [SerializeField] private float yOffset;
    [SerializeField] private float projectedYOffset;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    private void Awake()
    {
        initialDialoguePromptPosition = dialoguePrompt.transform.position;

        col_promptEffects.radius = col_prompt.radius;
        col_promptEffects.offset = col_prompt.offset;

        innerCircleInitialColor = innerCircle.GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true)
            {
                OnEnterRadius();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if(_tags.CheckTags(_playerTag.name) == true)
            {
                OnLeaveRadius();
            }
        }
    }

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_prompt, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(_playerTag.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnEnterRadius()
    {
        activeDialoguePrompt.SetActive(true);

        projectedYOffset = 0f;
        LeanTween.scale(dialoguePrompt, detectedScale, detectedScaleTime);
    }

    private void OnLeaveRadius()
    {
        activeDialoguePrompt.SetActive(false);

        projectedYOffset = -0.5f;
        LeanTween.scale(dialoguePrompt, undetectedScale, detectedScaleTime);
    }

    public void OnDialogueStart()
    {
        LeanTween.scale(dialoguePrompt, activeScale, activeScaleTime);
    }

    public void OnDialogueEnd()
    {
        if (CheckExistingObjects() == true)
        {
            OnEnterRadius();
        } else
        {
            OnLeaveRadius();
        }
    }

    public override void FixedUpdateState()
    {
        HoveringEffectUpdate();
    }

    private void HoveringEffectUpdate()
    {
        yOffset = Mathf.Lerp(yOffset, projectedYOffset, Time.deltaTime * yMoveSpeed);

        float y = Mathf.Sin(Time.time * frequency) * amplitude;

        float newY;
        if (isMovingEffects)
        {
            newY = transform.position.y + y + yOffset;
        } else
        {
            newY = initialDialoguePromptPosition.y + y + yOffset;
        }

        dialoguePrompt.transform.position = new Vector2(dialoguePrompt.transform.position.x, newY);
    }

    public void GrayOutInnerCircle()
    {
        innerCircle.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void ColorInnerCircle()
    {
        innerCircle.GetComponent<SpriteRenderer>().color = innerCircleInitialColor;
    }

}
