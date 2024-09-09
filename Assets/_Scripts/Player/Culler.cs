using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culler : MonoBehaviour
{
    [SerializeField] private LayerMask layer_isCullable;
    [SerializeField] public float activationDistance = 40f;
    [SerializeField] public float totalDistance = 50f;
    [SerializeField] private Camera _camera;
    [SerializeField] private CircleCollider2D col_culler;

    [SerializeField] private Dictionary<GameObject, ICuller> cullerCache = new Dictionary<GameObject, ICuller>();

    private void Update()
    {
        activationDistance = 5 + _camera.orthographicSize * 2;
        col_culler.radius = activationDistance;
        //totalDistance = activationDistance + 10;

        //PerformCulling();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        Debug.Log("LOADED:" + obj.transform.parent.name);

        if (!cullerCache.TryGetValue(obj, out ICuller culler))
        {
            culler = obj.GetComponent<ICuller>();
            cullerCache[obj] = culler;
            culler.LoadObjects();
        } else {
            obj.GetComponent<ICuller>().LoadObjects();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        Debug.Log("DELOADED:" + obj.transform.parent.name);

        if (!cullerCache.TryGetValue(obj, out ICuller culler))
        {
            culler = obj.GetComponent<ICuller>();
            cullerCache[obj] = culler;
            culler.DeloadObjects();
        }
        else
        {
            obj.GetComponent<ICuller>().DeloadObjects();
        }
    }

    private void PerformCulling()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, totalDistance, layer_isCullable);
        foreach (var collider in colliders)
        {
            GameObject obj = collider.gameObject;

            if (!cullerCache.TryGetValue(obj, out ICuller culler))
            {
                culler = obj.GetComponent<ICuller>();
                cullerCache[obj] = culler;
            }

            // Check if the object is within the activation distance
            float distanceToPlayer = Vector3.Distance(obj.transform.position, transform.position);

            if (distanceToPlayer <= activationDistance)
            {
                obj.GetComponent<ICuller>().LoadObjects();
            }
            else
            {
                obj.GetComponent<ICuller>().DeloadObjects();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationDistance);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, totalDistance);
    }
}