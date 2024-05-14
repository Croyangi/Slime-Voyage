using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLocationDiscover : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private BoxCollider2D col_discoverBounds;
    [SerializeField] private bool isDiscovered;
    [SerializeField] private bool isDisplayed;
    [SerializeField] private string id;

    [SerializeField] private ScriptObj_AreaId _areaId;

    [Header("Area Discovery")]
    [SerializeField] private string areaName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && isDisplayed == false)
            {

                Manager_NewLocationDiscover.instance.ChangeAreaName(areaName);

                if (isDiscovered == true)
                {
                    Manager_NewLocationDiscover.instance.EnterLocationVFX();
                } else
                {
                    Manager_NewLocationDiscover.instance.NewLocationDiscoverVFX();
                }
                
                isDiscovered = true;
                isDisplayed = true;
            }
        }
    }

    public void LoadData(GameData data)
    {
        // Search up area data based on id
        AreaSet areaSet = data.SearchAreaWithId(_areaId.name);
        SerializableDictionary<string, bool> locationsDiscovered = areaSet.locationsDiscovered;

        locationsDiscovered.TryGetValue(id, out isDiscovered);
    }

    public void SaveData(ref GameData data)
    {
        // Search up area data based on id
        AreaSet areaSet = data.SearchAreaWithId(_areaId.name);
        SerializableDictionary<string, bool> locationsDiscovered = areaSet.locationsDiscovered;
        if (locationsDiscovered.ContainsKey(id))
        {
            locationsDiscovered.Remove(id);
        }
        locationsDiscovered.Add(id, isDiscovered);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + (Vector3)col_discoverBounds.offset, col_discoverBounds.size);

    }
}
