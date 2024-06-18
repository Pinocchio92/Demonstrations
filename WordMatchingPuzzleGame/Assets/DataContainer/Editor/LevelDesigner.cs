using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(LevelData),false)]
[CanEditMultipleObjects]
[System.Serializable]
public class LevelDesigner : Editor
{
    private LevelData gameDataInstance => target as LevelData;
    SerializedProperty serializedLevelProp;
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DrawAlphabetGrid();
    }

    private void DrawAlphabetGrid()
    {
        EditorGUILayout.Space(50);
        EditorGUILayout.TextArea("With more time we could have extended the Editor UI to populate data in better way but at the moment we have be care full filling the data in grid. Make sure that rows and coloumn matches the data.");
    }
}
