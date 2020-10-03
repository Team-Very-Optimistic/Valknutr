using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectManager : Singleton<EffectManager>
{
    [HideInInspector]
    public PostProcessVolume m_postProcessVolume;
    private Vignette m_Vignette;
    private ColorGrading m_ColorGrading;
    private float playerHurtIntensity;
    private float ori; 
    private float mixerRedOutRedIn;
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
        m_postProcessVolume = GameObject.Find("PostProcess").GetComponent<PostProcessVolume>();
        m_Vignette = m_postProcessVolume.profile.GetSetting<Vignette>();
        m_ColorGrading = m_postProcessVolume.profile.GetSetting<ColorGrading>();
        ori = m_Vignette.intensity.value;
        mixerRedOutRedIn = m_ColorGrading.mixerRedOutRedIn.value;
    }

    public static GameObject PlayEffectAtPosition(string identifier, Vector3 position, Vector3 scale = new Vector3())
    {
        EffectEntry s = Array.Find(Instance.m_VFXLibrary, effect => effect.m_Identifier == identifier);

        if (s == null)
        {
            Debug.LogWarning("The requested effect \"" + identifier + "\" does not exist!");
            return null;
        }

        GameObject temp = Instantiate(s.effect, position, Quaternion.identity);
        if (scale != Vector3.zero)
            temp.transform.localScale = scale;
        if(temp)
            Destroy(temp, s.duration);
        return temp;
    }

    public void PlayerHurtEffect(Vector3 pos, float damage = 1f)
    {
        playerHurtIntensity = Mathf.Min(1, 0.05f * damage);
        PlayEffectAtPosition("bloodExplosion", pos).transform.localScale *= 4 * playerHurtIntensity;
        StartCoroutine(PlayerHurt());
    }

    IEnumerator PlayerHurt()
    {
        
        m_Vignette.intensity.value =  ori +  0.45f * playerHurtIntensity;
        m_ColorGrading.mixerRedOutRedIn.value = mixerRedOutRedIn + 50f * playerHurtIntensity ;
        Time.timeScale = 0.1f;
        ScreenShakeManager.Instance.ScreenShake(0.3f * playerHurtIntensity, 0.9f * playerHurtIntensity);
        yield return new WaitForSeconds(0.06f * playerHurtIntensity);
        Time.timeScale = 1;
        m_Vignette.intensity.value = ori;
        m_ColorGrading.mixerRedOutRedIn.value = mixerRedOutRedIn;
    }
}
