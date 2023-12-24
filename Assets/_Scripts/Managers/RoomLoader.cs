using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    [SerializeField] private Collider2D roomLoaderCollider;
    [SerializeField] private Tags _tags;
    //[SerializeField] private string roomName;
    [SerializeField] private GameObject deloads;
    [SerializeField] private bool sceneHasBeenLoaded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tags.CheckGameObjectTags(collision.gameObject, "RoomLoader"))
        {
            LoadRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_tags.CheckGameObjectTags(collision.gameObject, "RoomLoader"))
        {
            if (CheckUnload() == false) // Returns false if no other "RoomLoader" objects are within, thus unloading the room
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
                GameObject collidedObject = collider.gameObject;
                if (_tags.CheckGameObjectTags(collidedObject, "RoomLoader"))
                {
                    return true;
                }

            }
        }
        return false;
    }

    private void LoadRoom()
    {
        if (sceneHasBeenLoaded == false)
        {
            sceneHasBeenLoaded = true;
            //SceneManager.LoadScene(roomName, LoadSceneMode.Additive);
            deloads.SetActive(true);
        }
    }

    private void UnloadRoom()
    {
        //SceneManager.UnloadSceneAsync(roomName);
        sceneHasBeenLoaded = false;
        deloads.SetActive(false);
    }
}
