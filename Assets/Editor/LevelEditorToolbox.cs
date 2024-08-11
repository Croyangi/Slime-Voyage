using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

public class LevelEditorToolbox : EditorWindow
{
    private GameObject currentParent;

    private string searchString = "";
    private List<string> searchTagList = new List<string>();

    private string currentPrefabName = "";

    private Vector2 scrollPos = Vector2.zero;

    private Vector2 cellCenter;

    private bool isPaintModeEnabled = false;
    private bool isFreeHandPaint = false;
    private List<ScriptableObject> _levelEditorScriptableObjs = new List<ScriptableObject>();
    private List<ScriptableObject> _currentlyDisplayedScriptableObjects = new List<ScriptableObject>();
    private string levelEditorScriptableObjPath = "Assets/Editor Default Resources/Level Editor Toolbox Objects";
    private int paletteIndex;

    private float iconSize = 100f;

    List<GUIContent> paletteIcons = new List<GUIContent>();

    ///////////

    [MenuItem("Tools/Croyangi's Level Editor Toolbox")]
    public static void ShowWindow() 
    {
        GetWindow(typeof(LevelEditorToolbox));
        GetWindow(typeof(LevelEditorToolbox)).titleContent = new GUIContent("Level Editor");
    }

    void OnFocus()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;

        GetLevelEditorScriptableObjs();
        //GetLevelEditorObjectManager();
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        Sprite toolboxLogo = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor Default Resources/ToolboxLogo.png");
        GUI.DrawTexture(new Rect(0, 0, toolboxLogo.texture.width, toolboxLogo.texture.height), toolboxLogo.texture);
        DisplayPaintButton();

        GUILayout.Space(50);

        CurrentGrid();
        SearchBar();
        IconGUIs();
        DisplayFreeHandPaintMode();
        DisplayCurrentName();


        GUILayout.Space(20);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        DisplayPrefabGrid();
        EditorGUILayout.EndScrollView();

    }

    private void DisplayCurrentName()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Prefab: " + currentPrefabName, GUILayout.Width(300));
        GUILayout.EndHorizontal();
    }

    private void DisplayPaintButton()
    {
        GUILayout.BeginHorizontal();

        int buttonWidth = 157;
        GUILayout.Space((Screen.width - buttonWidth) / 2);
        if (GUILayout.Button("If you can see this, tell me", GUILayout.Width(buttonWidth), GUILayout.Height(100)))
        {
            isPaintModeEnabled = !isPaintModeEnabled;
        }

        Sprite paintOn = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor Default Resources/ToolboxButtonOn.png");
        Sprite paintOff = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor Default Resources/ToolboxButtonOff.png");
        Texture2D paintButtonState = isPaintModeEnabled ? paintOff.texture : paintOn.texture;
        GUI.DrawTexture(new Rect((Screen.width - buttonWidth) / 2, 2, paintButtonState.width, paintButtonState.height), paintButtonState);
        GUILayout.EndHorizontal();
    }

    private void CurrentGrid()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("As Parent:", GUILayout.Width(70));
        currentParent = EditorGUILayout.ObjectField(currentParent, typeof(GameObject), true) as GameObject;

        GUILayout.EndHorizontal();
    }

    private void DisplayFreeHandPaintMode()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Free Hand:", GUILayout.Width(70));
        isFreeHandPaint = EditorGUILayout.Toggle(isFreeHandPaint);

        GUILayout.EndHorizontal();
    }

    private void SearchBar()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Search:", GUILayout.Width(70));
        string tempSearchString = searchString;
        searchString = EditorGUILayout.TextField(searchString);
        if (searchString != tempSearchString)
        {
            DeselectGrid();
            OrganizeSearchString();
        }

        GUILayout.EndHorizontal();
    }

    private void IconGUIs()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Icon Size:", GUILayout.Width(70));
        iconSize = EditorGUILayout.Slider(iconSize, 50f, 150f);
        GUILayout.EndHorizontal();
        //DisplayAttributeGrid();
    }

    private void OrganizeSearchString()
    {
        // Split the input string by comma and remove any leading/trailing white spaces
        string[] tagArray = searchString.Split(',').Select(tag => tag.Trim()).ToArray();
        searchTagList = new List<string>(tagArray);
        for (int i = 0; i < searchTagList.Count; i++)
        {
            searchTagList[i] = searchTagList[i].Replace(" ", "");
        }
    }

    private void DisplayPrefabGrid()
    {
        paletteIcons.Clear();
        _currentlyDisplayedScriptableObjects.Clear();

        foreach (ScriptableObject scriptableObject in _levelEditorScriptableObjs)
        {
            // Use reflection to get all public fields of the scriptable object
            FieldInfo[] fields = scriptableObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            // Gets name
            int nameIndex = GetIndexReflectionFields(fields, "levelEditorObjectName");

            // Gets tags
            int tagsIndex = GetIndexReflectionFields(fields, "tags");
            List<string> tags = (List<string>)fields[tagsIndex].GetValue(scriptableObject);

            // Create a new list and copy the elements from the original list to it
            List<string> tempTags = new List<string>(tags);

            // Auto adds the object's name to tags
            string objectName = (string)fields[nameIndex].GetValue(scriptableObject);
            tempTags.Add(objectName);

            // Matches tags, also works with multiple tag
            int tagAmountMatch = searchTagList.Count();
            int currentMatchedTags = 0;
            foreach (string tag in searchTagList)
            {
                bool isMatchingTags = SearchTags(tempTags, tag);
                if (isMatchingTags)
                {
                    currentMatchedTags++;
                }
            }

            // Finally displays the icons
            if (currentMatchedTags == tagAmountMatch || searchString.Length == 0)
            {
                int objectIndex = GetIndexReflectionFields(fields, "levelEditorObject");
                Texture2D texture = AssetPreview.GetAssetPreview((Object)fields[objectIndex].GetValue(scriptableObject));
                paletteIcons.Add(new GUIContent(texture));
                _currentlyDisplayedScriptableObjects.Add(scriptableObject);
            }
            
        }

        GUIStyle iconStyle = new GUIStyle(GUI.skin.button);
        iconStyle.fixedWidth = iconSize;
        iconStyle.fixedHeight = iconSize;

        int numColumns = Mathf.FloorToInt(position.width / iconStyle.fixedWidth);

        // Display the grid
        paletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), numColumns, iconStyle);
        //DetectChangeObject();
    }

    private void DeselectGrid()
    {
        paletteIndex = -1;
    }

    private bool SearchTags(List<string> tags, string name)
    {
        foreach (string tag in tags)
        {
            if (tag.ToLower() == name.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    private int GetIndexReflectionFields(FieldInfo[] fields, string searchedName)
    {
        int index = 0;

        if (fields.Length > 0)
        {
            foreach (FieldInfo field in fields)
            {
                if (field.Name == searchedName)
                {
                    //Debug.Log("Found " + searchedName);
                    return index;
                }
                index++;
            }
        }
        return -1;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        FieldInfo[] fields = _currentlyDisplayedScriptableObjects[paletteIndex].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        // Get current name
        int prefabIndex = GetIndexReflectionFields(fields, "levelEditorObject");
        GameObject prefab = (GameObject)fields[prefabIndex].GetValue(_currentlyDisplayedScriptableObjects[paletteIndex]);
        currentPrefabName = prefab.name;

        if (isPaintModeEnabled)
        {
            if (paletteIndex != -1)
            {
                int objectSizeIndex = GetIndexReflectionFields(fields, "levelEditorObjectSize");
                Vector2 objectBoundingSize = (Vector2) fields[objectSizeIndex].GetValue(_currentlyDisplayedScriptableObjects[paletteIndex]);

                int objectOffsetIndex = GetIndexReflectionFields(fields, "objectOffset");
                Vector3 objectOffset = (Vector3) fields[objectOffsetIndex].GetValue(_currentlyDisplayedScriptableObjects[paletteIndex]);

                DisplayVisualAid(objectBoundingSize, objectOffset);
                HandleSceneViewInputs(cellCenter);
            }
        }
    }

    private void DisplayVisualAid(Vector2 objectSize, Vector3 internalOffset)
    {
        Vector3 offset = new Vector3(0.5f, 0.5f, 0f);

        Vector2 cellSize = new Vector2(1f, 1f);
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);
        mousePosition.x += offset.x;
        mousePosition.y += offset.y;

        Vector2 cell = new Vector2(Mathf.Round(mousePosition.x / cellSize.x) - offset.x + internalOffset.x, Mathf.Round(mousePosition.y / cellSize.y) - offset.y + internalOffset.y);
        if (!isFreeHandPaint)
        {
            cellCenter = cell * cellSize;
        } else
        {
            cellCenter = mousePosition;
        }

        // Vertices of our square
        Vector3 topLeft = cellCenter + Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f * (objectSize.y * 2 - 1);
        Vector3 topRight = cellCenter + Vector2.right * cellSize * 0.5f * (objectSize.x * 2 - 1) + Vector2.up * cellSize * 0.5f * (objectSize.y * 2 - 1);

        Vector3 bottomLeft = cellCenter + Vector2.left * cellSize * 0.5f + Vector2.down * cellSize * 0.5f;
        Vector3 bottomRight = cellCenter + Vector2.right * cellSize * 0.5f * (objectSize.x * 2 - 1) + Vector2.down * cellSize * 0.5f;

        // Rendering
        Handles.color = Color.green;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    private void HandleSceneViewInputs(Vector2 cellCenter)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }

        if (paletteIndex < _currentlyDisplayedScriptableObjects.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            // Initialize fields
            FieldInfo[] fields = _currentlyDisplayedScriptableObjects[paletteIndex].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            //int objectOffsetIndex = GetIndexReflectionFields(fields, "objectOffset");
            //Vector3 objectOffset = (Vector3) fields[objectOffsetIndex].GetValue(_currentlyDisplayedScriptableObjects[paletteIndex]);

            // Get prefab
            int prefabIndex = GetIndexReflectionFields(fields, "levelEditorObject");
            GameObject prefab = (GameObject) fields[prefabIndex].GetValue(_currentlyDisplayedScriptableObjects[paletteIndex]);

            // Instantiate on grid or not
            GameObject instantiatedObject;
            if (currentParent != null)
            {
                instantiatedObject = PrefabUtility.InstantiatePrefab(prefab, currentParent.transform) as GameObject;
            } else
            {
                instantiatedObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            }

            // Get internal offset to align with visual aid
            Vector3 internalOffset = Vector3.zero;
            if (prefab.TryGetComponent<Renderer>(out var renderer)) 
            {
                internalOffset.x = cellCenter.x + renderer.bounds.extents.x - 0.5f;
                internalOffset.y = cellCenter.y + renderer.bounds.extents.y - 0.5f;
            }

            // Apply offset
            instantiatedObject.transform.position = (Vector3) internalOffset;

            // Register undo
            Undo.RegisterCreatedObjectUndo(instantiatedObject, "");
        }
    }

    private void GetLevelEditorScriptableObjs()
    {
        _levelEditorScriptableObjs.Clear();
        // Get all the ScriptableObject GUIDs in the specified path
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { levelEditorScriptableObjPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            if (scriptableObject != null)
            {
                _levelEditorScriptableObjs.Add(scriptableObject);
            }
        }
    }
}
