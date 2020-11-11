using System;
using System.Collections;
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
    public static void StartNextLevel()
    {

        Instance.StartCoroutine(_StartNextLevel(++Instance.levelIndex % Instance.levels.Length));
    }
    
    static IEnumerator _StartNextLevel(int levelIndex)
    {
        UiManager.FadeToBlack(1);
        yield return new WaitForSeconds(1);
        DifficultyScalingSystem.Instance.IncreaseDifficulty(1);
        StartLevel(levelIndex);
        AudioManager.PlayBackgroundSound(Instance.levels[levelIndex].config.ambientMusic);
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

    public static IEnumerator MoveLevel(GameObject level, Vector3 offset)
    {
        yield return new WaitForSeconds(1.0f);
        level.transform.position = offset;
        var ofst = GameManager.Instance.cameraRig.transform.position - GameManager.Instance._player.transform.position;
        GameManager.Instance._player.transform.position = offset;
        GameManager.Instance.cameraRig.transform.position += offset;
        Instance.transform.position = offset;
        UiManager.FadeFromBlack(1);
        yield return new WaitForSeconds(1.0f);
        UiManager.HideBlackOverlay();
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

            level.Generate();
            Instance.StartCoroutine(MoveLevel(level.gameObject, offset));
        }
        else
        {
            throw new Exception("Level index out of range");
        }
    }
}