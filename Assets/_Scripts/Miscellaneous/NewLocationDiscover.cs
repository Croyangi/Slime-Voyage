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
        string fieldName = _areaId.name + "_newLocationsDiscovered";
        SerializableDictionary<string, bool> newLocationsDiscovered = (SerializableDictionary<string, bool>)data.GetType().GetField(fieldName).GetValue(data);
        Debug.Log(newLocationsDiscovered);

        newLocationsDiscovered.TryGetValue(id, out isDiscovered);
    }

    public void SaveData(ref GameData data)
    {
        string fieldName = _areaId.name + "_newLocationsDiscovered";
        SerializableDictionary<string, bool> newLocationsDiscovered = (SerializableDictionary<string, bool>)data.GetType().GetField(fieldName).GetValue(data);
        if (newLocationsDiscovered.ContainsKey(id))
        {
            newLocationsDiscovered.Remove(id);
        }
        newLocationsDiscovered.Add(id, isDiscovered);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + (Vector3)col_discoverBounds.offset, col_discoverBounds.size);

    }
}
