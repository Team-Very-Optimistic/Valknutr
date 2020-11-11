using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float elapsedTime;

    private Text textObj;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        textObj = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        textObj.text = $"{Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
    }
}
