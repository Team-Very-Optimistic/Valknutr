using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Vector3 worldLocationStart;
    private float timeElapsed = 0.0f;
    private Camera mainCamera;
    private Canvas canvas;
    private Boolean isMovingLeft;
    private RectTransform rectTransform;

    public float aliveTime;
    public float canvasXMax;
    public float canvasYMax;
    public float minScale;
    public float maxScale;
    public float horizontalScaleMultiplier;
    public float scaleReductionRate;

    void Start()
    {
        mainCamera = Camera.main;
        isMovingLeft = (UnityEngine.Random.value < 0.5);

        //Set max scale
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(maxScale * horizontalScaleMultiplier, maxScale, maxScale);

        //Find and parent to canvas
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        transform.SetParent(canvas.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        //Set time elapsed
        timeElapsed += Time.deltaTime;

        //Percent of time alive
        float percentAlive = timeElapsed / aliveTime;

        //World -> Canvas location
        Vector2 canvasPos = mainCamera.WorldToScreenPoint(worldLocationStart);

        //Place along sin trajectory
        canvasPos += new Vector2(
            isMovingLeft ? -(percentAlive * (float)Math.PI) * canvasXMax : percentAlive * (float)Math.PI * canvasXMax,
            (float)Math.Sin(percentAlive * Math.PI) * canvasYMax);

        //Add vertical offset
        canvasPos += new Vector2(0.0f, canvasYMax * 0.3f);

        //Set position to new position
        rectTransform.position = canvasPos;

        //Initial scale down 
        if(rectTransform.localScale.y >= minScale)
        {
            rectTransform.localScale -= new Vector3(scaleReductionRate * horizontalScaleMultiplier, scaleReductionRate, scaleReductionRate);
        }

        if (timeElapsed >= aliveTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamageTextProperties(float damage, Vector3 worldLocationStart, Color damageColor)
    {
        GetComponent<Text>().text = $"{damage:F}";
        GetComponent<Text>().color = damageColor;
        this.worldLocationStart = worldLocationStart;
    }
    public void SetWordTextProperties(string word, Vector3 worldLocationStart, Color wordColor)
    {
        aliveTime *= 3;
        minScale *= 1.3f;
        GetComponent<Text>().text = word;
        GetComponent<Text>().color = wordColor;
        this.worldLocationStart = worldLocationStart;
    }
}
