using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    [SerializeField] private Collider2D col_platform;

    // Pre-condition that the on trigger layer is only with Player layer
    public void OnTriggerStay2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(collision, col_platform);
        Debug.Log("Ignoring collision with Platform");
    }
}
