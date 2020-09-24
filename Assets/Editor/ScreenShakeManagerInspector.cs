using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ScreenShakeManager))]
public class ScreenShakeManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ScreenShakeManager screenShakeManager = (ScreenShakeManager) target;
        if (GUILayout.Button("ScreenShake"))
        {
            screenShakeManager.Awake();
            screenShakeManager.ScreenShake(1f, 1f);
        }
        
    }
}