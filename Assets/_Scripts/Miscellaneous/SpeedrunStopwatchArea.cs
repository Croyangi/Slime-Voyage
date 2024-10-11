using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunStopwatchArea : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                Manager_SpeedrunTimer.instance.EndSpeedrunTimer();
            }
        }
    }
}
