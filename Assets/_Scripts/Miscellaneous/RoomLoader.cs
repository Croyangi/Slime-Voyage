using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_roomLoader;
    [SerializeField] private Collider2D roomLoaderCollider;

    [Header("Inside References")]
    [SerializeField] private GameObject deloads;
    [SerializeField] private GameObject cinemachine;

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

    private void LoadRoom()
    {
        deloads.SetActive(true);
        Manager_Cinemachine.instance.OnChangeCinemachine(cinemachine);
    }

    private void UnloadRoom()
    {
        deloads.SetActive(false);
    }
}
