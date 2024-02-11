using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOSlimeMold : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private PlayerStateScriptObj _playerStateScriptObj;
    [SerializeField] private GameObject ufoSlimePrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Tags _tags;
    [SerializeField] private GameObject ufoSmokePrefab;

    [Header("Variables")]
    [SerializeField] private bool isUsed;
    [SerializeField] private int smokeAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.GetComponent<Tags>() != null)
        {
            _tags = collidedObject.GetComponent<Tags>();

            if (_tags.CheckTags("Player") == true && !isUsed)
            {
                player = collision.gameObject;

                if (_tags.CheckTags("Player_GhostSlime") == false)
                {
                    SpawnUFOSlime();
                }
            }

        }
    }

    private void SpawnUFOSlime()
    {
        isUsed = true; // Prevents infinite loop please dont delete this
        Vector2 currentVelocity = player.GetComponent<Rigidbody2D>().velocity;
        Destroy(player);
        GameObject ufoSlime = Instantiate(ufoSlimePrefab, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
        ufoSlime.GetComponent<Rigidbody2D>().velocity = currentVelocity;

        for (int i = 0; i < smokeAmount; i++)
        {
            SpawnSmoke();
        }

        SetCurrentMold();
    }

    private void SpawnSmoke()
    {
        GameObject ufoSlimeSmoke = Instantiate(ufoSmokePrefab, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
        ufoSlimeSmoke.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f)), ForceMode2D.Impulse);
        ufoSlimeSmoke.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-30f, 30f));
    }

    private void SetCurrentMold()
    {
        //_playerStateScriptObj.SetCurrentMold("UFOSlime");
    }
}
