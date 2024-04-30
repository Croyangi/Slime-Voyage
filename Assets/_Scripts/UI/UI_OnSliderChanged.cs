using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OnSliderChanged : MonoBehaviour
{
    [SerializeField] private AudioClip generalUIHover;
    [SerializeField] private float currentSFXSpacing = 0;
    [SerializeField] private float sfxSpacingAmount = 10;

    public void OnSliderChanged()
    {
        if (currentSFXSpacing <= 0)
        {
            Manager_SFXPlayer.instance.PlaySFXClip(generalUIHover, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.3f, 0, 0, 0, true);
            currentSFXSpacing = sfxSpacingAmount;
        } else
        {
            currentSFXSpacing--;
        }
    }
}
