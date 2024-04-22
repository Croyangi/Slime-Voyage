using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Handler_WarehouseIntro : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject baseSlime;

    [SerializeField] private GameObject slimeBox;
    [SerializeField] private GameObject[] boxes;

    [SerializeField] private Vector2 slimeBoxLaunchVelocity;
    [SerializeField] private float slimeBoxAngularVelocity;
    [SerializeField] private LayerMask slimeBoxLayerMask;
    [SerializeField] private GameObject garageDoor;

    [SerializeField] private GameObject cinemachine;
    [SerializeField] private float zoomInTransitionMultiplier = 1;

    [SerializeField] private GameObject lamp;

    [SerializeField] private AudioSource audioSource_warehouseIntro;
    [SerializeField] private TextMeshProUGUI tm_escapeText;

    public IEnumerator InitiateWarehouseIntro()
    {
        cinemachine.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        audioSource_warehouseIntro.Play();
        yield return new WaitForSeconds(6f);
        InitiateOpenGarageDoor();
    }

    [ContextMenu("Open Garage Door")]
    private void OpenGarageDoor()
    {
        LeanTween.moveLocalY(garageDoor, -2.5f, 0f);
        LeanTween.moveLocalY(garageDoor, 1.5f, 2f).setEaseOutBack().setDelay(1f);
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
        StartCoroutine(LaunchBoxes());
    }

    [ContextMenu("Initiate Close Garage Door")]
    public void InitiateCloseGarageDoor()
    {
        CloseGarageDoor();
    }

    private IEnumerator FadeInEscapeText(float targetAlpha)
    {
        Color textColor = tm_escapeText.color;
        float newAlpha = Mathf.MoveTowards(textColor.a, targetAlpha, Time.deltaTime / 8f);
        tm_escapeText.color = new Color(textColor.r, textColor.g, textColor.b, newAlpha);

        if (newAlpha < targetAlpha)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(FadeInEscapeText(targetAlpha));
        }
    }

    private IEnumerator LaunchSlimeBox()
    {
        slimeBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        slimeBox.GetComponent<Rigidbody2D>().AddForce(slimeBoxLaunchVelocity, ForceMode2D.Impulse);
        slimeBox.GetComponent<Rigidbody2D>().angularVelocity = slimeBoxAngularVelocity;
        yield return new WaitForSeconds(0.5f);
        slimeBox.GetComponent<BoxCollider2D>().excludeLayers = slimeBoxLayerMask;
    }

    private IEnumerator LaunchBoxes()
    {
        OpenGarageDoor();
        yield return new WaitForSeconds(5f);

        foreach (GameObject box in boxes)
        {
            box.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            box.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(22f, 25f), 15f), ForceMode2D.Impulse);
            box.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(160f, 260f);
            yield return new WaitForSeconds(0.5f);
            box.GetComponent<BoxCollider2D>().excludeLayers = slimeBoxLayerMask;

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(3f);
        StartCoroutine(LaunchSlimeBox());
        yield return new WaitForSeconds(1f);
        CloseGarageDoor();
        yield return new WaitForSeconds(3f);
        ZoomInOnSlimeBox();
        yield return new WaitForSeconds(6f);
        StartCoroutine(FadeInEscapeText(150f));
    }

    private void ZoomInOnSlimeBox()
    {
        slimeBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        StartCoroutine(SlimeBoxFidget());


        LeanTween.moveLocal(cinemachine, new Vector3(slimeBox.transform.position.x, slimeBox.transform.position.y, cinemachine.transform.position.z), 4f).setEaseInOutQuart();
        StartCoroutine(ZoomInOnSlimeBoxTransition()); // Start temporary update loop
        StartCoroutine(WaitForSlimeEscape());
    }

    private IEnumerator SlimeBoxFidget()
    {
        LeanTween.rotateZ(slimeBox, -4, 0.05f);
        LeanTween.rotateZ(slimeBox, 4, 0.1f).setDelay(0.05f);
        LeanTween.rotateZ(slimeBox, -4, 0.1f).setDelay(0.15f);
        LeanTween.rotateZ(slimeBox, 0, 0.05f).setDelay(0.25f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(SlimeBoxFidget());
    }

    private IEnumerator ZoomInOnSlimeBoxTransition()
    {
        // Zoom in
        float size = cinemachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        cinemachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = Mathf.Lerp(size, 4f, Time.deltaTime * zoomInTransitionMultiplier); // Lerp is NOT exponential, not self-referenced value

        yield return new WaitForFixedUpdate();
        StartCoroutine(ZoomInOnSlimeBoxTransition());
    }

    private IEnumerator WaitForSlimeEscape()
    {
        if (Input.anyKeyDown)
        {
            StopAllCoroutines();
            StartCoroutine(EndWarehouseIntro());
            Manager_SpeedrunTimer.instance.StartSpeedrunTimer();
        }

        /*
        if (baseSlime.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            StopAllCoroutines();
            StartCoroutine(EndWarehouseIntro());
            Manager_SpeedrunTimer.instance.StartSpeedrunTimer();
        }
        */
        yield return null;
        StartCoroutine(WaitForSlimeEscape());
    }

    private IEnumerator EndWarehouseIntro()
    {
        cinemachine.SetActive(false);
        tm_escapeText.gameObject.SetActive(false);

        baseSlime.transform.position = slimeBox.transform.position;
        baseSlime.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        baseSlime.GetComponent<Rigidbody2D>().AddForce(new Vector2(20f, 25f), ForceMode2D.Impulse);

        Manager_Jukebox.instance.PlayBreakingProtocol();

        yield return new WaitForSeconds(1f);
        lamp.GetComponent<WarehouseLamp>().FlickerOn();
    }
}
