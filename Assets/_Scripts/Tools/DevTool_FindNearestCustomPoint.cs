using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_FindNearestCustomPoints : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_customPoint;

    public void OnFindNearestCustomPoints()
    {
        Transform[] customPointPositions = FindAllCustomPoints();
        GameObject player = Manager_PlayerState.instance.player;
        GameObject closestCustomPoint = FindClosestCustomPoint(customPointPositions, player.transform);

        player.transform.position = (Vector2) closestCustomPoint.transform.position;
    }

    private Transform[] FindAllCustomPoints()
    {
        List<Transform> allTransforms = new List<Transform>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(tag_customPoint.name) == true)
                {
                    allTransforms.Add(obj.transform);
                }
            }
        }

        return allTransforms.ToArray();
    }

    private GameObject FindClosestCustomPoint(Transform[] tDialoguePrompts, Transform tPlayer)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity; // Any number compared will be smaller

        Vector2 currentPos = new Vector2(tPlayer.transform.position.x, tPlayer.transform.position.y);
        foreach (Transform t in tDialoguePrompts)
        {
            float dist = Vector2.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin.gameObject;
    }
}
