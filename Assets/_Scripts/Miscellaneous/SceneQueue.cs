using UnityEditor;
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

    public void QueueScene(string sceneName, bool isAdditive = false)
    {
        QueuedScenes queuedScenes = new QueuedScenes();
        queuedScenes.scene = sceneName;
        queuedScenes.isAdditive = isAdditive;

        scriptObj_roomQueue.queuedScenes.Add(queuedScenes);
    }

    public void LoadScene(string sceneName, bool isAdditive = false)
    {
        if (!isAdditive)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void LoadQueuedScenes()
    {
        if (scriptObj_roomQueue.queuedScenes.Count > 0)
        {
            if (!scriptObj_roomQueue.queuedScenes[0].isAdditive)
            {
                SceneManager.LoadScene(scriptObj_roomQueue.queuedScenes[0].scene);
            } else
            {
                SceneManager.LoadScene(scriptObj_roomQueue.queuedScenes[0].scene, LoadSceneMode.Additive);
            }
            scriptObj_roomQueue.queuedScenes.RemoveAt(0);
        } else
        {
            Debug.Log("Couldn't access scene.");
            SceneManager.LoadScene("LogoMenu");
        }
    }
}
