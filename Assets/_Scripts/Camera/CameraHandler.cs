using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject player;

    [Header("Variables")]
    [SerializeField] public bool followX;
    [SerializeField] public bool followY;
    [SerializeField] public float cameraX;
    [SerializeField] public float cameraY;
    [SerializeField] public float playerOffsetX;
    [SerializeField] public float playerOffsetY;

    [SerializeField] public float[] clampX;
    [SerializeField] public float[] clampY;
    [SerializeField] public bool isClampX;
    [SerializeField] public bool isClampY;

    [SerializeField] private Vector3 cameraPos;

    private void Start()
    {
        FindPlayer();
        SelectTarget(player);
    }

    public void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void SelectTarget(GameObject gameObject)
    {
        target = gameObject;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            FindPlayer();
            SelectTarget(player);
        }
        else
        {
            CameraTracking();
        }
    }

    private void CameraTracking()
    {
        // Locks onto player
        if (followX) { cameraX = target.transform.position.x; } else { cameraX = transform.position.x; }
        if (followY) { cameraY = target.transform.position.y; } else { cameraY = transform.position.y; }

        // Adds offsets
        cameraX += playerOffsetX;
        cameraY += playerOffsetY;


        // Clamps
        if (isClampX) { cameraX = Mathf.Clamp(cameraX, clampX[0], clampX[1]); }
        if (isClampX && clampX.Length == 0) { Debug.LogWarning("Please implement clamps in the array when enabling ClampX"); }

        if (isClampY) { cameraY = Mathf.Clamp(cameraY, clampY[0], clampY[1]); }
        if (isClampY && clampY.Length == 0) { Debug.LogWarning("Please implement clamps in the array when enabling ClampY"); }


        cameraPos = new Vector3(cameraX, cameraY, transform.position.z);
        transform.position = cameraPos;
    }
}
