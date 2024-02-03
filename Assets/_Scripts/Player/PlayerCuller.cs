using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerCuller : MonoBehaviour
{
    [SerializeField] private LayerMask layer_isCullable;
    [SerializeField] private float activationDistance = 40f;

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, activationDistance, layer_isCullable);
        foreach (var collider in colliders)
        {
            GameObject obj = collider.gameObject;

            // Check if the object is within the activation distance
            float distanceToPlayer = Vector3.Distance(obj.transform.position, transform.position);

            if (distanceToPlayer <= activationDistance)
            {
                obj.GetComponent<IPlayerCuller>().LoadObjects();
            }
            else
            {
                obj.GetComponent<IPlayerCuller>().DeloadObjects();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color for the Gizmos line

        // Draw a line to visualize the raycast in the Scene view
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}

