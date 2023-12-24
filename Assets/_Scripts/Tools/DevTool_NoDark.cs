using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_NoDark : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject devTool_globalLight;
    [SerializeField] private GameObject _globalLight;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _globalLightTag;

    private void FindGlobalLight()
    {
        Tags _tags;

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject thisObject in allObjects)

            if (thisObject.GetComponent<Tags>() != null)
            {
                _tags = thisObject.GetComponent<Tags>();
                if (_tags.CheckTags(_globalLightTag.name) == true)
                {
                    //Debug.Log("Successfully found GameObject with tag: " + tag);
                    _globalLight = thisObject;
                }
            }
    }

    public void ToggleNoDark()
    {
        if (devTool_globalLight.activeSelf == true) 
        { 
            OnDisableNoDark();
        } else
        {
            if (_globalLight == null) 
            {
                FindGlobalLight();
            }
            OnEnableNoDark();
        }
    }

    private void OnDisableNoDark()
    {
        devTool_globalLight.SetActive(false);
        _globalLight.SetActive(true);
    }

    private void OnEnableNoDark()
    {
        _globalLight.SetActive(false);
        devTool_globalLight.SetActive(true);

    }
}
