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
        levelEditorObjectSize = levelEditorObject.GetComponent<Renderer>().bounds.size;
        //objectOffset.x = levelEditorObjectSize.x - levelEditorObject.transform.localScale.x;
        //objectOffset.y = levelEditorObjectSize.y - levelEditorObject.transform.localScale.y;
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
