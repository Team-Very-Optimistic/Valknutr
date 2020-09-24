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
        shakeAngle = 2 * shakeAmount / 1;
        StartCoroutine(ScreenShake(time, intensityCurve));
    }
    
    
    IEnumerator ScreenShake(float time, AnimationCurve intensity)
    {
        var transform1 = cam.transform;
        originalPos = transform1.position;
        float t = 0;
        while (t < time)
        {
            var intensityValue = intensity.Evaluate(t/time) * shakeAmount;

            //Mathf.PerlinNoise(t / intensity, 0f) * curve.Eval(t);
            t += Time.unscaledDeltaTime;
            var random = Random.insideUnitSphere;

            transform1.position += random * intensityValue;
            intensityValue *= shakeAngle;
            transform1.rotation *= Quaternion.Euler(
                random.x * intensityValue , random.y *  intensityValue, random.z * intensityValue
            );
            yield return null;
        }

        transform1.transform.position = originalPos;
        transform1.transform.localEulerAngles = new Vector3();
    }
}
