using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private BoxCollider2D col_hitbox;
    [SerializeField] private BoxCollider2D col_grounded;

    private void OnDrawGizmos()
    {
        Vector3 hitbox = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitbox, col_hitbox.size);

        Vector3 grounded = new Vector3(transform.position.x + col_grounded.offset.x, transform.position.y + col_grounded.offset.y, 0);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(grounded, col_grounded.size);
    }
}
