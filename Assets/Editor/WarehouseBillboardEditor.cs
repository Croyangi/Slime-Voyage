using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WarehouseBillboard))]
public class WarehouseBillboardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WarehouseBillboard _warehouseBillboard = (WarehouseBillboard) target;

        if (GUILayout.Button("Regenerate Random Billboard Image"))
        {
            _warehouseBillboard.RandomizeBillboard();
        }
    }
}
