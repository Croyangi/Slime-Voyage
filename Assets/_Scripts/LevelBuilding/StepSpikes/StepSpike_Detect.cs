using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpike_Detect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StepSpike_StateHandler _stepSpike_stateHandler;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true && _stepSpike_stateHandler.isGrounded == true)
            {
                Manager_PlayerState.instance.InitiatePlayerDeath();
            }
        }
    }

}
