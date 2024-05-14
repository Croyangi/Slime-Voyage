using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Handler_WarehouseElevatorCutscene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pulley;
    [SerializeField] private GameObject elevator;

    [SerializeField] private GameObject cinemachine;
    [SerializeField] private GameObject fakeSlime;

    [SerializeField] private BootLoader_Warehouse _warehouse;

    [SerializeField] private AudioClip sfx_elevatorStart;
    [SerializeField] private AudioClip sfx_elevatorUp;
    [SerializeField] private AudioClip sfx_elevatorDing;

    const string BASESLIME_IDLE = "BaseSlime_Idle";


    public void InitiateElevatorCutscene()
    {
        cinemachine.SetActive(true);
        Manager_PlayerState.instance.SetInputStall(false);

        StartCoroutine(SetupCutscene());
    }

    private IEnumerator SetupCutscene()
    {
        // SFX
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_elevatorDing, transform, 1f, mixerGroup: Manager_AudioMixer.instance.mixer_sfx);

        // Base Slime stuff
        GameObject baseSlime = Manager_PlayerState.instance.player;
        fakeSlime.transform.position = new Vector2(baseSlime.transform.position.x, fakeSlime.transform.position.y);
        baseSlime.SetActive(false);
        fakeSlime.SetActive(true);
        fakeSlime.GetComponent<Animator>().Play(BASESLIME_IDLE);

        cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = fakeSlime.transform;

        //LeanTween.moveLocal(cinemachine, new Vector3(fakeSlime.transform.position.x, fakeSlime.transform.position.y, cinemachine.transform.position.z), 3f).setEaseInOutQuart();

        yield return new WaitForFixedUpdate();

        StartCoroutine(OnElevatorCutscene());
    }

    private IEnumerator OnElevatorCutscene()
    {
        LeanTween.moveLocalY(elevator, elevator.transform.position.y - 0.5f, 1f).setEaseInBounce();
        yield return new WaitForSeconds(2f);
        //StartCoroutine(ElevatorUpButtonAnimation());

        // SFX
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_elevatorStart, transform, 1f, mixerGroup: Manager_AudioMixer.instance.mixer_sfx);
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_elevatorUp, transform, 0.3f, mixerGroup: Manager_AudioMixer.instance.mixer_sfx);

        LeanTween.rotateAround(pulley, Vector3.forward, 360, 2.5f).setLoopClamp();
        LeanTween.moveLocalY(elevator, elevator.transform.position.y + 100, 10).setEaseInCubic();

        yield return new WaitForSeconds(6f);

        // Transition screen
        _warehouse.OnWarehouseComplete();
    }
}
