using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationVolumeFader : MonoBehaviour
{
    [SerializeField] private bool isInversed;

    [SerializeField] private BoxCollider2D boxCol_area;
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                // We get world space coordinates between the left and right sides of the collider, than find the percentage (decimal) within said ranges
                float posX = collision.transform.position.x;
                float leftBounds = (transform.position.x - (boxCol_area.size.x / 2) + boxCol_area.offset.x);
                //float RightBounds = transform.position.x + (boxCol_area.size.x / 2);

                float posDiff = posX - leftBounds;

                float percentBetween = posDiff / boxCol_area.size.x;
                if (isInversed)
                {
                    percentBetween = 1f - percentBetween;
                }


                Debug.Log(percentBetween);
                Manager_Jukebox.instance.SetVolume(percentBetween);
            }
        }
    }
}
