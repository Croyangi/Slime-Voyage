using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WarehouseLamp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite lampOn;
    [SerializeField] private Sprite lampOff;
    [SerializeField] private Light2D[] lights;
    [SerializeField] private bool isOn = true;

    private void Awake()
    {
        if (isOn)
        {
            ToggleLights(lights, true);
        } else
        {
            ToggleLights(lights, false);
        }
    }

    [ContextMenu("Initiate Flicker On")]
    public void FlickerOn()
    {
        ToggleLights(lights, false);
        isOn = true;
        StartCoroutine(FlickerOnAnimation());
    }

    private IEnumerator FlickerOnAnimation()
    {
        ToggleLights(lights, true);
        yield return new WaitForSeconds(0.3f);
        ToggleLights(lights, false);
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 3; i++)
        {
            ToggleLights(lights, true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            ToggleLights(lights, false);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }

        yield return new WaitForSeconds(0.3f);
        ToggleLights(lights, true);
    }


    [ContextMenu("Initiate Flicker Off")]
    public void FlickerOff()
    {
        ToggleLights(lights, true);
        isOn = false;
        StartCoroutine(FlickerOffAnimation());
    }

    private IEnumerator FlickerOffAnimation()
    {
        ToggleLights(lights, false);
        yield return new WaitForSeconds(0.3f);
        ToggleLights(lights, true);
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 3; i++)
        {
            ToggleLights(lights, false);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            ToggleLights(lights, true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }
        
        yield return new WaitForSeconds(0.3f);
        ToggleLights(lights, false);
    }

    private void ToggleLights(Light2D[] lights, bool state)
    {
        foreach (Light2D light in lights)
        {
            light.enabled = state;
            if (state == true)
            {
                sr.sprite = lampOn;
            } else
            {
                sr.sprite = lampOff;
            }
        }
    }
}
