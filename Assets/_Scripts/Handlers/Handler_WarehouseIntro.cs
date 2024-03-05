using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_WarehouseIntro : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject slimeBox;
    [SerializeField] private Vector2 slimeBoxLaunchVelocity;
    [SerializeField] private float slimeBoxAngularVelocity;
    [SerializeField] private LayerMask slimeBoxLayerMask;
    [SerializeField] private GameObject garageDoor;
    [SerializeField] private GameObject baseSlime;

    [ContextMenu("Open Garage Door")]
    private void OpenGarageDoor()
    {
        LeanTween.moveLocalY(garageDoor, -2.5f, 0f);
        LeanTween.moveLocalY(garageDoor, 1.5f, 1f).setEaseOutBack().setDelay(1f);
    }

    [ContextMenu("Close Garage Door")]
    private void CloseGarageDoor()
    {
        LeanTween.moveLocalY(garageDoor, 1.5f, 0f);
        LeanTween.moveLocalY(garageDoor, -2.5f, 1f).setEaseInBack().setEaseOutBounce().setDelay(1f);
    }

    [ContextMenu("Initiate Open Garage Door")]
    public void InitiateOpenGarageDoor()
    {
        StartCoroutine(LaunchSlimeBox());
    }

    [ContextMenu("Initiate Close Garage Door")]
    public void InitiateCloseGarageDoor()
    {
        CloseGarageDoor();
    }

    private IEnumerator LaunchSlimeBox()
    {
        OpenGarageDoor();

        yield return new WaitForSeconds(4f);
        slimeBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        slimeBox.GetComponent<Rigidbody2D>().AddForce(slimeBoxLaunchVelocity, ForceMode2D.Impulse);
        slimeBox.GetComponent<Rigidbody2D>().angularVelocity = slimeBoxAngularVelocity;
        yield return new WaitForSeconds(0.5f);
        slimeBox.GetComponent<BoxCollider2D>().excludeLayers = slimeBoxLayerMask;
        yield return new WaitForSeconds(1);

        CloseGarageDoor();
    }
}
