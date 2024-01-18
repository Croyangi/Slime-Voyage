using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostSlime_ControlBeam : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject controlBeam_raycastAnchor;
    [SerializeField] private Camera mainCamera;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("Building Block References")]

    [Header("Tags")]
    const string IS_GHOST_CONTROLLABLE = "IsGhostControllable";

    [Header("Variables")]
    [SerializeField] private Vector2 mousePosition;

    [SerializeField] private GameObject controlBeam_controlledObject;
    [SerializeField] private Vector2 controlBeam_controlledObjectOffset;
    [SerializeField] private bool controlBeam_isBeamTravelling;
    [SerializeField] private bool controlBeam_isBeamCaptured;
    [SerializeField] private float controlBeam_beamMaxCaptureDistance;
    [SerializeField] private float controlBeam_controlTension;
    [SerializeField] private float controlBeam_beamMaxHoldDistance;
    [SerializeField] private float controlBeam_maxAngularVelocity;

    [SerializeField] private bool stasisBeam_isActive;
    [SerializeField] private bool stasisBeam_isPerformed;
    [SerializeField] private float stasisBeam_timer;
    [SerializeField] private float stasisBeam_timeLimit; // How long you have to hold down the button for to recognize it
    [SerializeField] private Vector3 stasisBeam_savedPosition;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
        mainCamera = FindCamera();

        if (controlBeam_beamMaxCaptureDistance > controlBeam_beamMaxHoldDistance)
        {
            Debug.LogWarning("Control Beam's Max Capture Distance is longer than Max Hold Distance");
        }
    }

    private Camera FindCamera()
    {
        Tags _tags;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Tags>() != null)
            {
                _tags = obj.GetComponent<Tags>();
                if (_tags.CheckTags("MainCamera") == true)
                {
                    return obj.GetComponent<Camera>();
                }
            }
        }
        return null;
    }

    private void OnEnable()
    {
        playerInput.GhostSlime.Beam.performed += OnBeamPerformed;
        playerInput.GhostSlime.Beam.canceled += OnBeamCancelled;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.GhostSlime.Beam.performed -= OnBeamPerformed;
        playerInput.GhostSlime.Beam.canceled -= OnBeamCancelled;
        playerInput.Disable();
    }

    private void OnBeamPerformed(InputAction.CallbackContext value)
    {
        if (controlBeam_isBeamCaptured)
        {
            stasisBeam_isActive = true;
        }
    }

    private void OnBeamCancelled(InputAction.CallbackContext value)
    {
        // Only trigger the toggle beam if didn't hold for too long
        if (stasisBeam_timer < stasisBeam_timeLimit)
        {
            ToggleControlBeam();
        }

        if (controlBeam_controlledObject != null)
        {
            OnStasisBeamCancelled();
        }

        stasisBeam_isActive = false;
        stasisBeam_isPerformed = false;
        stasisBeam_timer = 0f;
    }

    private void ToggleControlBeam()
    {
        if (controlBeam_isBeamCaptured)
        {
            BreakControlBeamConnection();
        }
        else
        {
            controlBeam_isBeamTravelling = true;
        }
    }

    private void OnStasisBeamPerformed()
    {
        Rigidbody2D rb = controlBeam_controlledObject.GetComponent<Rigidbody2D>();
        stasisBeam_savedPosition = controlBeam_controlledObject.transform.position;
        rb.velocity = Vector2.zero;
    }

    private void OnStasisBeamCancelled()
    {

    }

    private void StasisBeamUpdate()
    {
        // When holding the Control Beam button long enough, you can freely move without the controlled object moving
        if (controlBeam_controlledObject != null)
        {
            if (stasisBeam_isPerformed == false) // Runs once until un-held
            {
                stasisBeam_isPerformed = true;
                OnStasisBeamPerformed();
            }

            Rigidbody2D rb = controlBeam_controlledObject.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            controlBeam_controlledObject.transform.position = stasisBeam_savedPosition;

            controlBeam_controlledObjectOffset = controlBeam_controlledObject.transform.position - gameObject.transform.position;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red; // Set the color for the Gizmos line

    //    // Draw a line to visualize the raycast in the Scene view
    //    Gizmos.DrawLine(controlBeam_raycastAnchor.transform.position, mousePosition);
    //}

    private void ControlBeamRaycast()
    {
        Tags _tags;

        // Cast raycast with the angle converted into a Vector2 direction
        float angle = GetRadianBetweenMouse();
        Vector2 raycastDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        //Debug.Log(raycastDirection);

        RaycastHit2D[] hits = Physics2D.RaycastAll(controlBeam_raycastAnchor.transform.position, raycastDirection, controlBeam_beamMaxCaptureDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null) // On Hit
            {
                controlBeam_isBeamTravelling = false;

                GameObject collidedObject = hit.collider.gameObject;
                if (collidedObject.GetComponent<Tags>() != null)
                {
                    _tags = collidedObject.GetComponent<Tags>();
                    if (_tags.CheckTags(IS_GHOST_CONTROLLABLE) == true)
                    {
                        //Debug.Log("Hit tagged object " + hit.collider.gameObject);
                        SetControlledObject(hit.collider.gameObject);
                        return;
                    }
                    else
                    {
                        //Debug.Log("Hit untagged object " + hit.collider.gameObject);
                    }
                }
            }
        }
    }

    private void SetControlledObject(GameObject hit)
    {
        controlBeam_controlledObject = hit;
        controlBeam_controlledObjectOffset = controlBeam_controlledObject.transform.position - gameObject.transform.position;
        controlBeam_isBeamTravelling = false;
        controlBeam_isBeamCaptured = true;
    }

    private float GetRadianBetweenMouse()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        float x1 = controlBeam_raycastAnchor.transform.position.x;
        float x2 = mousePosition.x;
        float y1 = controlBeam_raycastAnchor.transform.position.y;
        float y2 = mousePosition.y;
        float angle = Mathf.Atan2(y2 - y1, x2 - x1);

        // Radians are displayed in the negatives when crossing PI, but doesn't matter, ig Unity converts it just the same

        // ANGLE IS IN RADIANS
        return angle;
    }

    private void ControlBeamMovement()
    {
        if (controlBeam_controlledObject != null)
        {
            // Calculate target position
            Vector2 targetPosition = new Vector2(transform.position.x + controlBeam_controlledObjectOffset.x, transform.position.y + controlBeam_controlledObjectOffset.y);

            // Calculate the desired velocity towards the target position
            Vector2 desiredVelocity = (targetPosition - (Vector2) controlBeam_controlledObject.transform.position) / Time.deltaTime;

            // Smoothly adjust the velocity, controlBeam_controlTension controls how quickly the object moves to the target position
            if (stasisBeam_isPerformed == false)
            {
                controlBeam_controlledObject.GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(controlBeam_controlledObject.GetComponent<Rigidbody2D>().velocity, desiredVelocity, controlBeam_controlTension * Time.deltaTime);
            }

            // Clamping rotation, no crazy spins here... or...?
            controlBeam_controlledObject.GetComponent<Rigidbody2D>().angularVelocity = Mathf.Clamp(controlBeam_controlledObject.GetComponent<Rigidbody2D>().angularVelocity, -(controlBeam_maxAngularVelocity), controlBeam_maxAngularVelocity);

            // Snaps connection if you are too far from controlled object
            if (GetDistanceBetweenTwoPoints(controlBeam_controlledObject.transform.position, controlBeam_raycastAnchor.transform.position) > controlBeam_beamMaxHoldDistance)
            {
                BreakControlBeamConnection();
            }
        }
    }

    private void BreakControlBeamConnection()
    {
        if (controlBeam_controlledObject != null)
        {
            OnStasisBeamCancelled();
        }

        stasisBeam_isActive = false;
        stasisBeam_isPerformed = false;
        stasisBeam_timer = 0f;

        controlBeam_isBeamCaptured = false;
        controlBeam_controlledObject = null;
        controlBeam_controlledObjectOffset = Vector2.zero;

        //Debug.Log("Control Beam connection snapped!");
    }

    private float GetDistanceBetweenTwoPoints(Vector2 pointA, Vector2 pointB)
    {
        float distance;

        float group1 = Mathf.Pow((pointB.x - pointA.x), 2);
        float group2 = Mathf.Pow((pointB.y - pointA.y), 2);
        distance = Mathf.Sqrt(group1 + group2);

        return distance;
    }

    private void FixedUpdate()
    {
        if (controlBeam_isBeamTravelling) { ControlBeamRaycast(); }
        if (controlBeam_isBeamCaptured) { ControlBeamMovement(); }

        if (stasisBeam_isActive) { stasisBeam_timer += Time.deltaTime; }
        if (stasisBeam_timer > stasisBeam_timeLimit) { StasisBeamUpdate(); }

        // Final check if anything weird happens to the object
        if (controlBeam_isBeamCaptured && controlBeam_controlledObject == null) 
        {
            BreakControlBeamConnection(); 
        }
    }
}
