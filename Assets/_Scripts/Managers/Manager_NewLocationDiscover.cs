using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_NewLocationDiscover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator_areaName;
    [SerializeField] private Animator animator_flavorText;
    [SerializeField] private Animator animator_extender;
    [SerializeField] private GameObject enabledGroup;

    [SerializeField] private TextMeshProUGUI tm_areaName;

    [Header("State References")]
    const string NLC_AREANAMEOPENCLOSE = "NewLocationDiscover_AreaNameOpenClose";
    const string NLC_FLAVORTEXTOPENCLOSE = "NewLocationDiscover_FlavorTextOpenClose";
    const string NLC_EXTENDEROPENCLOSE = "NewLocationDiscover_ExtenderOpenClose";

    public static Manager_NewLocationDiscover instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Manager New Location Discover in the scene.");
        }
        instance = this;
    }

    private void PlayAnimationState(string newState, Animator _animator)
    {
        _animator.Play(newState);

    }

    [ContextMenu("New Location Discover VFX")]
    public void NewLocationDiscoverVFX()
    {
        Debug.Log("NEW");
        PlayAnimationState(NLC_AREANAMEOPENCLOSE, animator_areaName);
        PlayAnimationState(NLC_FLAVORTEXTOPENCLOSE, animator_flavorText);
        PlayAnimationState(NLC_EXTENDEROPENCLOSE, animator_extender);
    }

    public void EnterLocationVFX()
    {
        Debug.Log("ENTER");
        PlayAnimationState(NLC_AREANAMEOPENCLOSE, animator_areaName);
        PlayAnimationState(NLC_EXTENDEROPENCLOSE, animator_extender);
    }

    public void ChangeAreaName(string name)
    {
        tm_areaName.text = name;
    }
}
