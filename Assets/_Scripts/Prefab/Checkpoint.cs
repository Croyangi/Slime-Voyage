using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator checkpointAnimator;
    [SerializeField] private SpriteRenderer sr;

    [Header("Variables")]
    public bool isCheckpointEnabled;
    [SerializeField] private bool isHidden;
    [SerializeField] private bool isUninteractable;

    private void Start()
    {
        ChangeCheckPoint();
        if (isHidden)
        {
            sr.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player") && !isUninteractable)
        {
            SetCheckPoint();
        }
    }

    private void SetCheckPoint()
    {

        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject checkpoint in checkpoints)
        {
            Checkpoint tempScript = checkpoint.GetComponent<Checkpoint>();
            tempScript.isCheckpointEnabled = false;
            tempScript.ChangeCheckPoint();
        }

        isCheckpointEnabled = true;
        ChangeCheckPoint();
    }

    private void ChangeCheckPoint()
    {
        checkpointAnimator.SetBool("IsEnabled", isCheckpointEnabled);
    }
}
