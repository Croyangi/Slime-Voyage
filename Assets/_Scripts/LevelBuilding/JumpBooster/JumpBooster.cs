using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpBooster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private GameObject particle;

    [SerializeField] private Sprite jumpBooster;
    [SerializeField] private Sprite outline;

    [Header("Variables")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float regenerationTimer;
    [SerializeField] private bool isAvailable;

    [Header("Rise/Fall Settings")]
    [SerializeField] private float _amplitude = 0;
    [SerializeField] private float _frequency = 1;


    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerInput.BaseSlime.Jump.performed += OnJumpPerformed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            playerInput.BaseSlime.Jump.performed -= OnJumpPerformed;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (isAvailable)
        {
            UseJumpBooster();
        }
        //rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    private void UseJumpBooster()
    {
        playerRb.velocity = (Vector2.up * jumpStrength);
        regenerationTimer = 5f;
        sr.sprite = outline;
        isAvailable = false;
        Instantiate(particle, transform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        float y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        transform.position = new Vector2(transform.position.x, transform.position.y + y);

        if (regenerationTimer > 0)
        {
            regenerationTimer -= Time.deltaTime;

            if (regenerationTimer < 0f)
            {
                sr.sprite = jumpBooster;
                isAvailable = true;
            }
        }
    }
}
