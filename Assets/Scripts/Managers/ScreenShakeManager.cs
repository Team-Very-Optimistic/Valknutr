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
    public void ScreenShake(float time, float intensity, AnimationCurve intensityCurve = null)
    {
        if (intensityCurve == null)
        {
            intensityCurve = curve;
        }
        shakeAmount = Math.Min(intensity, 1);
        shakeAngle = 45 * shakeAmount / 1;
        StartCoroutine(ScreenShake(time, intensityCurve));
    }
    
    
    IEnumerator ScreenShake(float time, AnimationCurve intensity)
    {
        Debug.Log("screenshake");
        var transform1 = cam.transform;
        originalPos = transform1.position;
        shakeAngle = (shakeAngle + Mathf.PI * 0.7f) % (Mathf.PI * 2f);
        float t = 0;
        while (t < time)
        {
            var intensityValue = intensity.Evaluate(t/time);

            //Mathf.PerlinNoise(t / intensity, 0f) * curve.Eval(t);
            t += Time.unscaledDeltaTime;
            
            transform1.position += Random.insideUnitSphere * (shakeAmount * intensityValue);
            
            transform1.rotation *= Quaternion.Euler(
                Mathf.Cos(shakeAngle * intensityValue) * shakeAmount, Mathf.Sin(shakeAngle * intensityValue) * shakeAmount, 0f
            );
            yield return null;
        }

        transform1.position = originalPos;
        transform1.eulerAngles = originalRot;
    }
}
