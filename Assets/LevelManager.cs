using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelGenerator[] levels;
    public int levelIndex;
    public GameObject optionalLevel;
    public void Start()
    {
        levelIndex = 0;
        AudioManager.PlayBackgroundSound("level1");
    }

    [ContextMenu("Next Level")]
    public void StartNextLevel()
    {
        StartCoroutine(DifficultyScalingSystem.Instance.IncreaseDifficulty(1, 0.0f));
        StartLevel(++levelIndex % levels.Length);

        AudioManager.PlayBackgroundSound("level2");
    }

    public void StartOptionalLevel()
    {
        foreach (var level in Instance.levels)
        {
            level.gameObject.SetActive(false);
        }

       
        optionalLevel.gameObject.SetActive(true);
        var offset = new Vector3(-1000, 0, -1000);

        optionalLevel.transform.position = offset;

        GameManager.Instance._player.transform.position = offset;
    }
    
    public static void StartLevel(int levelIndex)
    {
        for (var index = 0; index < Instance.levels.Length; index++)
        {
            if (index == levelIndex) continue;
            var level = Instance.levels[index];
            level.gameObject.SetActive(false);
        }

        if (levelIndex < Instance.levels.Length)
        {
            var level = Instance.levels[levelIndex];
            level.gameObject.SetActive(true);
            var offset = levelIndex * new Vector3(1000, 0, 1000);

            level.transform.position = Vector3.zero;
            level.Generate();
            
            level.transform.position = offset;
            GameManager.Instance._player.transform.position = offset;
            GameManager.Instance.cameraRig.transform.position = offset;
            
        }
        else
        {
            throw new Exception("Level index out of range");
        }
    }
}