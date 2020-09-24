using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Vector3 worldLocationStart;
    private float timeElapsed = 0.0f;
    private GameObject mainCamera;
    private Canvas canvas;
    private Boolean isMovingLeft;
    private RectTransform rectTransform;

    public float aliveTime;
    public float canvasXMax;
    public float canvasYMax;
    public float minScale;
    public float maxScale;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        isMovingLeft = (UnityEngine.Random.value < 0.5);

        //Set max scale
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(maxScale, maxScale, maxScale);

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
        Vector2 canvasPos = mainCamera.GetComponent<Camera>().WorldToScreenPoint(worldLocationStart);

        //Place along sin trajectory
        canvasPos += new Vector2(
            isMovingLeft ? -(percentAlive * (float)Math.PI) * canvasXMax : percentAlive * (float)Math.PI * canvasXMax,
            (float)Math.Sin(percentAlive * Math.PI) * canvasYMax);

        //Add vertical offset
        canvasPos += new Vector2(0.0f, canvasYMax * 0.7f);

        //Set position to new position
        rectTransform.position = canvasPos;

        //Initial scale down 
        if(rectTransform.localScale.x >= minScale)
        {
            rectTransform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        }

        if (timeElapsed >= aliveTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamageTextProperties(float damage, Vector3 worldLocationStart, Color damageColor)
    {
        GetComponent<Text>().text = damage.ToString();
        GetComponent<Text>().color = damageColor;
        this.worldLocationStart = worldLocationStart;
    }
}
