using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Room))]
public class RoomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Room room = (Room) target;
        if (GUILayout.Button("Generate Minimap Sprite"))
        {
            room.GenerateMinimapSprite();
        }
    }
}
