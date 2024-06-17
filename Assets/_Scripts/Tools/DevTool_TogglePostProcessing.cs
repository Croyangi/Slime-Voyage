using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_TogglePostProcessing : MonoBehaviour
{
    [SerializeField] private TagsScriptObj tag_globalVolume;
    [SerializeField] private GameObject globalVolume;

    public void TogglePostProcessing()
    {
        if (globalVolume != null)
        {
            globalVolume.SetActive(!globalVolume.activeSelf);
        } else
        {
            globalVolume = FindGlobalVolume();
        }
    }

    private GameObject FindGlobalVolume()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(tag_globalVolume.name) == true)
                {
                    obj.SetActive(!obj.activeSelf);
                    return obj;
                }
            }
        }


        Debug.Log("Could not find GameObject tagged with: " + tag_globalVolume.name + ".");
        return null;
    }
}
