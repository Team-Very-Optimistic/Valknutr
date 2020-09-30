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
        shakeAngle =  2 * shakeAmount / 1;
        StartCoroutine(ScreenShake(time, intensityCurve));
    }
    
    
    IEnumerator ScreenShake(float time, AnimationCurve intensity)
    {
        var transform1 = cam.transform;
        originalPos = transform1.localPosition;
        float t = 0;
        var random = Random.insideUnitSphere;
        var noise = new Vector3();
        while (t < time)
        {
            var intensityValue = intensity.Evaluate(t/time) * shakeAmount;

            //Mathf.PerlinNoise(t / intensity, 0f) * curve.Eval(t);
            t += Time.unscaledDeltaTime;
            
            noise.x =  Mathf.PerlinNoise( random.x + t, -10.0f) - 0.5f;
            noise.y =  Mathf.PerlinNoise( random.y + t, 0.0f) - 0.5f;
            noise.z = Mathf.PerlinNoise(random.z + t, 10.0f) - 0.5f;
            noise *= 2f;
            noise += Random.insideUnitSphere;
    
            
            transform1.localPosition = noise * intensityValue;
            intensityValue *= shakeAngle;
            transform1.localEulerAngles = new Vector3(
                noise.x * intensityValue , noise.y *  intensityValue, noise.z * intensityValue
            );
            yield return null;
        }

        transform1.transform.localPosition = new Vector3();
        transform1.transform.localEulerAngles = new Vector3();
    }
}
