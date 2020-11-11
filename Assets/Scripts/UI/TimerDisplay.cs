using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    private Text textObj;
    // Start is called before the first frame update
    void Start()
    {
        textObj = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var elapsedTime = GameManager.GetPlayTime();
        textObj.text = $"{Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
    }
}
