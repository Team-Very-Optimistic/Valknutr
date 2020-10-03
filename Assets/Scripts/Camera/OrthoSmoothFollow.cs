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
    private void Start()
    {
        target = GameManager.Instance._player.transform;
        height = transform.position.y;
    }

    void Update() {
        Vector3 goalPos = target.position;
        newHeight = height * target.localScale.y;
        goalPos.y = newHeight;
        goalPos += offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }
}
