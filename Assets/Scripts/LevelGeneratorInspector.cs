using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelGenerator generator = (LevelGenerator) target;
        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }
        
        if (GUILayout.Button("Generate Single"))
        {
            generator.GenerateRooms();
        }
        
        if (GUILayout.Button("Clear"))
        {
            generator.Cleanup();
        }
    }
}