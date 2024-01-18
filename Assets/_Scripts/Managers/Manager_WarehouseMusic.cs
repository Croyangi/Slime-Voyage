using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_WarehouseMusic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WarehouseMusic piano;
    [SerializeField] private WarehouseMusic bass;
    [SerializeField] private WarehouseMusic sax;
    [SerializeField] private WarehouseMusic synth;
    [SerializeField] private AudioSource throwbackBeat;
    [SerializeField] private AudioSource scratchBeat;

    private void Awake()
    {
        piano.queue.Add(QueueRandomAudioClip(piano));
        piano.queue.Add(QueueRandomAudioClip(piano));
        piano.queue.Add(QueueRandomAudioClip(piano));
        InitiateQueue(piano);

        //bass.queue.Add(QueueRandomAudioClip(bass));
        //bass.queue.Add(QueueRandomAudioClip(bass));
        //bass.queue.Add(QueueRandomAudioClip(bass));
        //InitiateQueue(bass);

        sax.queue.Add(QueueRandomAudioClip(sax));
        sax.queue.Add(QueueRandomAudioClip(sax));
        sax.queue.Add(QueueRandomAudioClip(sax));
        InitiateQueue(sax);

        synth.queue.Add(QueueRandomAudioClip(synth));
        synth.queue.Add(QueueRandomAudioClip(synth));
        synth.queue.Add(QueueRandomAudioClip(synth));
        InitiateQueue(synth);

        throwbackBeat.Play();
        scratchBeat.Play();
    }

    private void InitiateQueue(WarehouseMusic _warehouseMusic)
    {
        if (_warehouseMusic == piano)
        {
            bass.currentAudioClip = bass.queue[0];
            bass.source.clip = bass.queue[0];
            bass.queue.RemoveAt(0);
            bass.source.Stop();
            bass.source.Play();
        }

        _warehouseMusic.currentAudioClip = _warehouseMusic.queue[0];
        _warehouseMusic.source.clip = _warehouseMusic.queue[0];
        _warehouseMusic.queue.RemoveAt(0);
        _warehouseMusic.source.Stop();
        _warehouseMusic.source.Play();
        //StopCoroutine(WaitUntilAudioClipFinish(_warehouseMusic));
        StartCoroutine(WaitUntilAudioClipFinish(_warehouseMusic));
    }

    private AudioClip QueueRandomAudioClip(WarehouseMusic _warehouseMusic)
    {
        int random = Random.Range(0, _warehouseMusic.clips.Count);
        AudioClip clip = _warehouseMusic.clips[random];

        if (_warehouseMusic == piano)
        {
            bass.queue.Add(bass.clips[random]);
        }

        return clip;
    }

    private void OnAudioClipFinished(WarehouseMusic _warehouseMusic)
    {
        _warehouseMusic.queue.Add(QueueRandomAudioClip(_warehouseMusic));
        InitiateQueue(_warehouseMusic);
    }

    private IEnumerator WaitUntilAudioClipFinish(WarehouseMusic _warehouseMusic)
    {
        while (_warehouseMusic.source.time < _warehouseMusic.source.clip.length)
        {
            //Debug.Log($"Playing: {_warehouseMusic.source.clip.name}, Time: {_warehouseMusic.source.time}");
            yield return null;
        }

        OnAudioClipFinished(_warehouseMusic);
    }
}

