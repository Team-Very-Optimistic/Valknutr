using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoSmoothFollow : MonoBehaviour {

    [HideInInspector]
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;
    private float height;
    private float newHeight;
    private bool bossTriggered;
    
    private void Start()
    {
        target = GameManager.Instance._player.transform;
        height = transform.position.y;
    }

    private void Awake()
    {
        EnemyBehaviour_Boss.OnBossStart += BossCameraShift;
        EnemyBehaviour_Boss.OnBossDeath += BossCameraShift;  
        EnemyBehaviour_Boss_OakTree.OnBossStart += BossCameraShift;
        EnemyBehaviour_Boss_OakTree.OnBossDeath += BossCameraShift;    
    }

    private void BossCameraShift()
    {
        if (!bossTriggered)
        {
            bossTriggered = true;
            height +=  5f;
        }
        else
        {
            bossTriggered = false;
            height -=  5f;
        }
    }

    void Update() {
        Vector3 goalPos = target.position;
        newHeight = height * target.localScale.y;
        goalPos.y = Mathf.Lerp(transform.position.y, newHeight, Time.deltaTime * 2f);
        goalPos += offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }
}
