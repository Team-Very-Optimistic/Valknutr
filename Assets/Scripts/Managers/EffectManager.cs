using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectManager : Singleton<EffectManager>
{
    public PostProcessVolume m_postProcessVolume;
    private Vignette m_Vignette;
    private ColorGrading m_ColorGrading;
    [System.Serializable]
    public class EffectEntry
    {
        public string m_Identifier;
        public GameObject effect;
        public float duration;
    }
    
    [SerializeField]
    private EffectEntry[] m_VFXLibrary;

    public void Start()
    {
        m_Vignette = m_postProcessVolume.profile.GetSetting<Vignette>();
        m_ColorGrading = m_postProcessVolume.profile.GetSetting<ColorGrading>();

    }

    public static void PlayEffectAtPosition(string identifier, Vector3 position, Vector3 scale = new Vector3())
    {
        EffectEntry s = Array.Find(Instance.m_VFXLibrary, effect => effect.m_Identifier == identifier);

        if (s == null)
        {
            Debug.LogWarning("The requested effect \"" + identifier + "\" does not exist!");
            return;
        }

        GameObject temp = Instantiate(s.effect, position, Quaternion.identity);
        if (scale != Vector3.zero)
            temp.transform.localScale = scale;
        if(temp)
            Destroy(temp, s.duration); 
    }

    public void PlayerHurtEffect()
    {
        StartCoroutine(PlayerHurt());
    }

    IEnumerator PlayerHurt()
    {
        float ori = m_Vignette.intensity.value;
        m_Vignette.intensity.value =  0.45f;
        var mixerRedOutRedIn = m_ColorGrading.mixerRedOutRedIn.value;
        m_ColorGrading.mixerRedOutRedIn.value = 150f;
        Time.timeScale = 0.1f;
        ScreenShakeManager.Instance.ScreenShake(0.3f, 0.9f);
        yield return new WaitForSeconds(0.06f);
        Time.timeScale = 1;
        m_Vignette.intensity.value = ori;
        m_ColorGrading.mixerRedOutRedIn.value = mixerRedOutRedIn;
    }
}
