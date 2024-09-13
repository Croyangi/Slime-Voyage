using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepSpike_Detect : GeneralCullerCommunicator
{
    [SerializeField] private ParticleSystem snoring;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tags _tags;
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.GetComponent<Tags>() != null)
        {
            _tags = collidedObject.GetComponent<Tags>();
            if (_tags.CheckTags(tag_player.name) == true)
            {
                Manager_PlayerState.instance.InitiatePlayerDeath();
            }
        }
    }

    private IEnumerator StartSnoreDelay()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 2f));
        snoring.Play();
    }

    public override void OnLoad()
    {
        StartCoroutine(StartSnoreDelay());
    }


    public override void OnCull()
    {
        snoring.Stop();
    }
}
