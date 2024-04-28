using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_SwapeeEnd : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Collider2D col_detect;
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] private GameObject dialoguePrompt;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_loadedScene;
    [SerializeField] private string scene_deloadedScene;

    private void OnDialogueEnd()
    {
        if (Manager_DialogueHandler.instance._dialoguePackage == _dialoguePackage)
        {
            Manager_PlayerState.instance.SetInputStall(false);
            dialoguePrompt.SetActive(false);
            StartCoroutine(LoadWarehouseDioramaMenu());
        }
    }

    private IEnumerator LoadWarehouseDioramaMenu()
    {
        _sceneQueue.LoadScene("WarehouseDioramaMenu");
        yield return null;

        /*Manager_PauseMenu.instance.isUnpausable = true;
        Manager_LoadingScreen.instance.CloseLoadingScreen();
        yield return new WaitForSeconds(3f);
        Manager_PauseMenu.instance.isUnpausable = false;
        Manager_LoadingScreen.instance.OnLoadSceneTransfer(scene_loadedScene, scene_deloadedScene);
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                Manager_DialogueHandler.instance.onDialogueEnd += OnDialogueEnd;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && CheckExistingObjects() == false)
            {
                Manager_DialogueHandler.instance.onDialogueEnd -= OnDialogueEnd;
            }
        }
    }

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_detect, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(tag_player.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
