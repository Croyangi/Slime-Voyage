using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "Cro's Scriptable Objs/New Level Editor Toolbox Object")]
public class LevelEditorObjectScriptObj : ScriptableObject
{
    public GameObject levelEditorObject;
    public Vector3 objectOffset = new Vector3(0f, 0f, 0f);
    public Vector2 levelEditorObjectSize = new Vector2(1f, 1f);
    public string levelEditorObjectName;

    public List<string> tags;

    private void OnValidate()
    {
        // If the parent has a Renderer, use its bounds size
        if (levelEditorObject.GetComponent<Renderer>() != null)
        {
            levelEditorObjectSize = levelEditorObject.GetComponent<Renderer>().bounds.size;
        }
        else
        {
            // Initialize variables to store the minimum and maximum bounds
            Vector3 minBounds = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxBounds = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // Get all Renderers in the parent and its children
            Renderer[] renderers = levelEditorObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                // Expand the min and max bounds to include the renderer's bounds
                minBounds = Vector3.Min(minBounds, renderer.bounds.min);
                maxBounds = Vector3.Max(maxBounds, renderer.bounds.max);
            }

            // Calculate the overall size based on the min and max bounds
            levelEditorObjectSize = maxBounds - minBounds;
        }
    }

    //[SerializeField] public bool hasEditableAttributes;

    //public List<AttributeModifiers> attributeModifiers = new List<AttributeModifiers>();

    //[Serializable]
    //public class AttributeModifiers
    //{
    //    public string name;
    //    public string description;
    //    public string inputField;
    //}
}
