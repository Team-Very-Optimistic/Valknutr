using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    private Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = $"{GameManager.Instance.levelName}[{Mathf.RoundToInt(DifficultyScalingSystem.GetLevel())}]";
    }
}
