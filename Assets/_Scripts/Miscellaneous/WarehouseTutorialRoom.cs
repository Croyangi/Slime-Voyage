using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WarehouseTutorialRoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D playerLight;
    [SerializeField] private ScriptObj_BaseSlimeMovementVariables movementVars;

    [SerializeField] private GameObject[] tutorialBoxes;

    [Header("Variables")]
    [SerializeField] private bool initialPlayerTrigger;

    private void FixedUpdate()
    {
        if ((movementVars.baseSlime.processedInputMovement.x != 0 || movementVars.baseSlime.processedInputMovement.y != 0) && !initialPlayerTrigger)
        {
            initialPlayerTrigger = true;
            playerLight.enabled = true;
            LaunchBoxes();
        }
    }

    private void LaunchBoxes()
    {
        tutorialBoxes[0].GetComponent<Rigidbody2D>().simulated = true;
        tutorialBoxes[1].GetComponent<Rigidbody2D>().simulated = true;
        tutorialBoxes[2].GetComponent<Rigidbody2D>().simulated = true;
        tutorialBoxes[3].GetComponent<Rigidbody2D>().simulated = true;

        tutorialBoxes[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(5f, 18f), ForceMode2D.Impulse);
        tutorialBoxes[1].GetComponent<Rigidbody2D>().AddForce(new Vector2(4, 18f), ForceMode2D.Impulse);
        tutorialBoxes[2].GetComponent<Rigidbody2D>().AddForce(new Vector2(-6f, 18f), ForceMode2D.Impulse);
        tutorialBoxes[3].GetComponent<Rigidbody2D>().AddForce(new Vector2(-5f, 18f), ForceMode2D.Impulse);

        tutorialBoxes[0].GetComponent<Rigidbody2D>().AddTorque(2, ForceMode2D.Impulse);
        tutorialBoxes[1].GetComponent<Rigidbody2D>().AddTorque(-2, ForceMode2D.Impulse);
        tutorialBoxes[2].GetComponent<Rigidbody2D>().AddTorque(3, ForceMode2D.Impulse);
        tutorialBoxes[3].GetComponent<Rigidbody2D>().AddTorque(-3, ForceMode2D.Impulse);
    }


}
