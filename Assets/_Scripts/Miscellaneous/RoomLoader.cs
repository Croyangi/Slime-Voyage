using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLoader : Room
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_roomLoader;
    [SerializeField] private Collider2D roomLoaderCollider;
    public bool isLoaded;

    [Header("Inside References")]
    [SerializeField] private GameObject deloads;
    public GameObject cinemachine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_roomLoader.name) == true)
            {
                LoadRoom();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_roomLoader.name) == true && CheckUnload() == false)
            {
                UnloadRoom();
            }
        }
    }

    private bool CheckUnload()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(roomLoaderCollider, new ContactFilter2D(), colliders);

        if (colliders.Count > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
                {
                    if (_tags.CheckTags(tag_roomLoader.name) == true)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override void LoadRoom()
    {
        isLoaded = true;
        deloads.SetActive(true);
        Manager_RoomLoader.instance.OnLoadRoom(GetComponent<RoomLoader>());
    }

    public override void UnloadRoom()
    {
        isLoaded = false;

        deloads.SetActive(false);
    }
}
