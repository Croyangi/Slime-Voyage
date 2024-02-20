using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseJunk : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private string spriteFolderPath = "Sprites/Warehouse/WarehouseJunk/<insert rarity>"; // The folder path within the Resources folder.
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private LayerMask warehouseJunkLayers;

    [Header("Fade Out Settings")]
    [SerializeField] private float alpha;
    [SerializeField] private float fadeOutTime = 3;
    [SerializeField] private float time = 0f;

    private void Awake()
    {
        LoadSprites();
        RandomizeJunk();
        gameObject.AddComponent<PolygonCollider2D>().excludeLayers = warehouseJunkLayers;
    }

    private void LoadSprites()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(spriteFolderPath);

        foreach (Sprite sprite in loadedSprites)
        {
            spriteList.Add(sprite);
        }
    }

    public void RandomizeJunk()
    {
        sr.sprite = spriteList[Random.Range(0, spriteList.Count)];
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        alpha = Mathf.Lerp(1f, 0f, time / fadeOutTime);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
