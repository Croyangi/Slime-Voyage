using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLocationDiscover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private BoxCollider2D col_discoverBounds;
    [SerializeField] private bool isDiscovered;

    [Header("Area Discovery")]
    [SerializeField] private string areaName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && isDiscovered == false)
            {

                Manager_NewLocationDiscover.instance.ChangeAreaName(areaName);
                Manager_NewLocationDiscover.instance.NewLocationDiscoverVFX();
                /*
                if (isDiscovered == true)
                {
                    Manager_NewLocationDiscover.instance.EnterLocationVFX();
                } else
                {
                    Manager_NewLocationDiscover.instance.NewLocationDiscoverVFX();
                }
                */
                isDiscovered = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + (Vector3)col_discoverBounds.offset, col_discoverBounds.size);

    }
}
