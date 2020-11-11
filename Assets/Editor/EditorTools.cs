using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class EditorTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [MenuItem("Tools/Next Level")]
    public static void GoNextLevel()
    {
        LevelManager.StartNextLevel();
    }

    [MenuItem("Tools/Trigger Lose")]
    public static void TriggerLose()
    {
        GameManager.Instance._player.GetComponent<PlayerHealth>().ApplyDamage(float.MaxValue);
    }
    
    [MenuItem("Tools/Trigger Win")]
    public static void TriggerWin()
    {
        GameManager.SetGameWin();
    }
    
    [MenuItem("Tools/Generate Minimap Icon %#m")]
    private static void GenerateMinimapIcons()
    {
        Undo.RecordObjects(Selection.gameObjects, "generate minimap icon");
        foreach (var (collider1, transform1) in Selection.gameObjects.Select(go => (go.GetComponent<Collider>(), go.transform)))
        {
            if (collider1 != null)
            {
                var icon = generateMinimapIcon(collider1);
                icon.transform.parent = transform1;
            }
        }
    }
    
    private static GameObject generateMinimapIcon(Collider c)
    {
        var bounds = c.bounds;
        var iconPrefab = Resources.Load<GameObject>("MinimapIcon_Enemy");

        var minimapIcon = Instantiate(iconPrefab);
        var spriteRenderer = minimapIcon.GetComponent<SpriteRenderer>();
        var spriteWidth = spriteRenderer.bounds.size.x;
        var spriteHeight = spriteRenderer.bounds.size.z;

        var cWidth = bounds.size.x;
        var cHeight = bounds.size.z;

        minimapIcon.transform.localScale = new Vector3(cWidth / spriteWidth, cHeight / spriteHeight, 1);
        minimapIcon.transform.position = bounds.center + Vector3.up * 15;
        return minimapIcon;
    }
}
