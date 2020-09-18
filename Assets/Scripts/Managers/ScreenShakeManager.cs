using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class ScreenShakeManager : Singleton<ScreenShakeManager>
{
    private Vector3 originalPos;
    private Vector3 originalRot;
    private Camera cam;

    private float shakeAmount;
    private float shakeAngle;
    public AnimationCurve curve;
    
    public void Awake()
    {
        cam = Camera.main;
        originalRot = cam.transform.eulerAngles;
    }

    /// <summary>
    /// Screen shakes 
    /// </summary>
    /// <param name="time">The time it takes for the screen to shake</param>
    /// <param name="intensity">The intensity from 0 to 1 of the screen shake</param>
    public void ScreenShake(float time, float intensity)
    {
        shakeAmount = Math.Min(intensity, 1);
        shakeAngle = 45 * shakeAmount / 1;
        StartCoroutine(ScreenShake(time));
    }
    
    IEnumerator ScreenShake(float time)
    {
        Debug.Log("screenshake");
        var transform1 = cam.transform;
        originalPos = transform1.position;
        float t = 0;
        while (t < time)
        {
            //Mathf.PerlinNoise(t / intensity, 0f) * curve.Eval(t);
            t += Time.unscaledDeltaTime;
            yield return null;

            transform1.position += Random.insideUnitSphere * shakeAmount;

            shakeAngle = (shakeAngle + Mathf.PI * 0.7f) % (Mathf.PI * 2f);
            
            transform1.rotation *= Quaternion.Euler(
                Mathf.Cos(shakeAngle) * shakeAmount, Mathf.Sin(shakeAngle) * shakeAmount, 0f
            );
        }

        transform1.position = originalPos;
        transform1.eulerAngles = originalRot;
    }
}
