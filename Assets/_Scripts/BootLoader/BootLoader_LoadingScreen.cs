using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader_LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject chunkfishDisk;

    private void Awake()
    {
        LeanTween.rotateZ(chunkfishDisk, 0, 0).setIgnoreTimeScale(true);
        LeanTween.rotateAroundLocal(chunkfishDisk, Vector3.forward, -360, 4f).setLoopClamp().setIgnoreTimeScale(true).setDelay(0.1f);
    }
}
