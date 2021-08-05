using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapGenerator mg = (MapGenerator)target;
        if (GUILayout.Button("���ɵ�ͼ"))
        {
            mg.gennerator();
            EditorUtility.SetDirty(mg);
        }
        if (GUILayout.Button("��յ�ͼ"))
        {
            mg.clear();
            EditorUtility.SetDirty(mg);
        }
    }
}
