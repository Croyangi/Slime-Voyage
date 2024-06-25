using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_TogglePostUI : MonoBehaviour
{
    [SerializeField] private TagsScriptObj tag_isUI;
    [SerializeField] private List<GameObject> uis;
    [SerializeField] private bool isToggled;

    public void TogglePostProcessing()
    {
        if (uis.Count == 0)
        {
            uis = FindUI();
        }

        foreach (GameObject ui in uis)
        {
            ui.SetActive(isToggled);
        }

        Cursor.visible = isToggled;

        isToggled = !isToggled;
    }

    private List<GameObject> FindUI()
    {
        List<GameObject> uis = new List<GameObject>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(tag_isUI.name) == true)
                {
                    uis.Add(obj);
                }
            }
        }

        return uis;
    }
}
