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
        
        if (GUILayout.Button("Remove Connected"))
        {
            generator.RemoveConnectedExits();
        }
        
        if (GUILayout.Button("Remove Colliders"))
        {
            generator.RemoveRoomColliders();
        }
        
        if (GUILayout.Button("Rebuild Navmesh"))
        {
            generator.RebuildNavMesh();
        }
    }
}