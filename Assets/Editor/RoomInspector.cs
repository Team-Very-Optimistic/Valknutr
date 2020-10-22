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
            int children = room.transform.childCount;
            for (int i = 0; i < children; ++i)
            {
                var child = room.transform.GetChild(i).gameObject;
                if (child.name == "Minimap_Room(Clone)")
                {
                    DestroyImmediate(child);
                }
            }

            room.GenerateMinimapSprite();
        }
    }
}
