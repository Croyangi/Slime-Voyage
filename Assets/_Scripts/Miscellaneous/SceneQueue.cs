using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ScriptObj_SceneQueue;

public class SceneQueue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public ScriptObj_SceneQueue scriptObj_roomQueue;

    public void UnqueueAllScenes()
    {
        scriptObj_roomQueue.queuedScenes.Clear();
    }

    public void QueueScene(SceneAsset scene, bool isAdditive = false)
    {
        QueuedScenes queuedScenes = new QueuedScenes();
        queuedScenes.scene = scene;
        queuedScenes.isAdditive = isAdditive;

        scriptObj_roomQueue.queuedScenes.Add(queuedScenes);
    }

    public void LoadSceneWithAsset(SceneAsset scene, bool isAdditive = false)
    {
        if (!isAdditive)
        {
            SceneManager.LoadScene(scene.name);
        }
        else
        {
            SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
        }
    }

    public void LoadQueuedScenes()
    {
        if (scriptObj_roomQueue.queuedScenes.Count > 0)
        {
            if (!scriptObj_roomQueue.queuedScenes[0].isAdditive)
            {
                SceneManager.LoadScene(scriptObj_roomQueue.queuedScenes[0].scene.name);
            } else
            {
                SceneManager.LoadScene(scriptObj_roomQueue.queuedScenes[0].scene.name, LoadSceneMode.Additive);
            }
            scriptObj_roomQueue.queuedScenes.RemoveAt(0);
        } else
        {
            SceneManager.LoadScene("LogoMenu");
        }
    }
}
