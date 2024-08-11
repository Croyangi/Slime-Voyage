using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static Golem_Jiggler;

public class Golem_Jiggler : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private Transform parentAnchorPoint;
    [SerializeField] private string currentStateId;

    [SerializeField] private float globalLerpScale;

    [Serializable]
    public class JiggledObject
    {
        public GameObject obj;
        public float lerpScale;
        public Vector2 offset;

        public JiggledObject(JiggledObject other)
        {
            this.obj = other.obj;
            this.lerpScale = other.lerpScale;
            this.offset = other.offset;
        }
    }

    [SerializeField] private List<JiggledObject> currentJiggledObjects;

    // Presets

    [Serializable]
    public class Preset
    {
        public string id;
        public List<JiggledObject> jiggledObjects;
    }

    [SerializeField] private List<Preset> presets;

    private void Awake()
    {
        SortPreset(currentStateId);
        StartCoroutine(FunnyFaceDemonstration());
    }

    private IEnumerator FunnyFaceDemonstration()
    {
        SortPreset("mouthOpen");
        yield return new WaitForSeconds(3f);
        SortPreset("mouthClosed");
        yield return new WaitForSeconds(3f);
        SortPreset("eyebrowRaised");
        yield return new WaitForSeconds(3f);
        SortPreset("exploded");
        yield return new WaitForSeconds(3f);
        StartCoroutine(FunnyFaceDemonstration());
    }

    private void FixedUpdate()
    {
        foreach (JiggledObject jiggledObj in currentJiggledObjects)
        {
            Vector3 localTargetPosition = transform.parent.InverseTransformPoint(anchorPoint.position);

            float desiredX = Mathf.Lerp(jiggledObj.obj.transform.position.x, localTargetPosition.x + jiggledObj.offset.x + parentAnchorPoint.position.x, jiggledObj.lerpScale * globalLerpScale);
            float desiredY = Mathf.Lerp(jiggledObj.obj.transform.position.y, localTargetPosition.y + jiggledObj.offset.y + parentAnchorPoint.position.y, jiggledObj.lerpScale * globalLerpScale);

            Vector2 desiredPos = new Vector2(desiredX, desiredY);

            jiggledObj.obj.transform.position = desiredPos;
        }
    }

    public void SortPreset(string presetName)
    {
        foreach (Preset preset in presets)
        {
            if (presetName == preset.id)
            {
                SetPreset(preset.jiggledObjects);
                return;
            }
        }
        Debug.LogWarning("No preset with that name found.");
    }
    private void SetPreset(List<JiggledObject> preset)
    {
        currentJiggledObjects = DeepCopyPreset(preset);

        //for (int i = 0; i < currentJiggledObjects.Count; i++)
        //{
        //    JiggledObject currentObj = currentJiggledObjects[i];
        //    JiggledObject newObj = newJiggledObjects[i];
        //    if (currentObj.obj == newObj.obj)
        //    {

        //    }
        //}
    }

    private List<JiggledObject> DeepCopyPreset(List<JiggledObject> preset)
    {
        // Create a new list with deep copy of elements
        List<JiggledObject> newJiggledObjs = new List<JiggledObject>();
        foreach (JiggledObject jiggledObj in preset)
        {
            // Dialogue class has a copy method to create a deep copy
            JiggledObject newJiggledObj = new JiggledObject(jiggledObj);
            newJiggledObjs.Add(newJiggledObj);
        }
        return newJiggledObjs;
    }
}
