using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBounds : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private Collider2D col_deathBounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                Manager_PlayerState.instance.InitiatePlayerDeath();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color for the Gizmos line

        // Draw a line to visualize the raycast in the Scene view
        Gizmos.DrawWireCube(transform.position + (Vector3) col_deathBounds.offset, new Vector3(col_deathBounds.bounds.size.x, col_deathBounds.bounds.size.y, 0f));
    }
}
