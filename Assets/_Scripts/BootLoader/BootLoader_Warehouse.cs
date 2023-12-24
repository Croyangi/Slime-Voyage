using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_Warehouse : MonoBehaviour
{
    [SerializeField] private string currentRoom;
    [SerializeField] private string scenesPath = "Scenes";
    [SerializeField] public List<string> warehouseSceneNames = new List<string>();

    private void Awake()
    {
        scenesPath = "Assets/Scenes/Rooms/Warehouse/";

        LoadAllWarehouseScenes();
    }

    private void LoadAllWarehouseScenes()
    {
        string[] scenePaths = Directory.GetFiles(scenesPath, "*.unity");

        foreach (string scenePath in scenePaths)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            warehouseSceneNames.Add(sceneName);
        }
    }
}
