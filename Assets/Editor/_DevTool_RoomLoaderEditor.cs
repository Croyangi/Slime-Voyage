using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _DevTool_RoomLoaderEditor : EditorWindow
{
    [SerializeField] private string sceneFolderPath = ""; // The folder path within the Resources folder.
    [SerializeField] public List<string> warehouseSceneNames = new List<string>();

    private void OnFocus()
    {
        sceneFolderPath = "Assets/Scenes/Rooms/Warehouse/";

        LoadAllWarehouseScenes();
    }

    private void LoadAllWarehouseScenes()
    {
        string[] scenePaths = Directory.GetFiles(sceneFolderPath, "*.unity");

        foreach (string scenePath in scenePaths)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            warehouseSceneNames.Add(sceneName);
        }
    }

    [MenuItem("Tools/Croyangi's Room Loader")]
    public static void ShowWindow()
    {
        GetWindow(typeof(_DevTool_RoomLoaderEditor));
        GetWindow(typeof(_DevTool_RoomLoaderEditor)).titleContent = new GUIContent("Room Loader");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Load All Rooms"))
        {
            LoadAllRooms();
        }

        if (GUILayout.Button("Deload All Rooms"))
        {
            DeloadAllRooms();
        }

        if (GUILayout.Button("Remove All Rooms"))
        {
            RemoveAllRooms();
        }
    }
    public void LoadAllRooms()
    {
        foreach (var room in warehouseSceneNames)
        {
            EditorSceneManager.OpenScene(sceneFolderPath + room + ".unity", OpenSceneMode.Additive);
        }
    }

    public void DeloadAllRooms()
    {
        foreach (var room in warehouseSceneNames)
        {
            EditorSceneManager.CloseScene(SceneManager.GetSceneByName(room), false);
        }
    }

    public void RemoveAllRooms()
    {
        foreach (var room in warehouseSceneNames)
        {
            EditorSceneManager.CloseScene(SceneManager.GetSceneByName(room), true);
        }
    }
}
