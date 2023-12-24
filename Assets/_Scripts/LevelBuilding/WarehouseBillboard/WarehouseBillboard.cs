using System.Collections.Generic;
using UnityEngine;

public class WarehouseBillboard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer billboardImage;
    [SerializeField] private string spriteFolderPath = "Sprites/Warehouse/WarehouseBillboard/Graphics"; // The folder path within the Resources folder.
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();

    private void Start()
    {
        LoadSprites();
        RandomizeBillboard();
    }

    private void LoadSprites()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(spriteFolderPath);

        foreach (Sprite sprite in loadedSprites)
        {
            spriteList.Add(sprite);
        }
    }

    public void RandomizeBillboard()
    {
        billboardImage.sprite = spriteList[Random.Range(0, spriteList.Count)];
    }
}
