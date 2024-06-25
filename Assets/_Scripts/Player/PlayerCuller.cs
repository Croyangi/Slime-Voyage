using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerCuller : MonoBehaviour
{
    [SerializeField] private LayerMask layer_isCullable;
    [SerializeField] public float activationDistance = 40f;
    [SerializeField] public float totalDistance = 50f;

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, totalDistance, layer_isCullable);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, totalDistance);
    }
}

