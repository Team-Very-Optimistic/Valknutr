using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectManager : Singleton<EffectManager>
{
    [HideInInspector]
    public PostProcessVolume m_postProcessVolume;

    public float _enemyHurtTimeStopIntensity = 2f;
    private Vignette m_Vignette;
    private ColorGrading m_ColorGrading;
    public float playerHurtIntensityMultiplier;
    private float _playerHurtIntensity;
    private float ori; 
    private float mixerRedOutRedIn;
    private Light _staffLight;
    private float oriIntensity;

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
        _staffLight = GameManager.Instance._weapon.GetComponent<Light>();
        m_postProcessVolume = GameObject.Find("PostProcess").GetComponent<PostProcessVolume>();
        m_Vignette = m_postProcessVolume.profile.GetSetting<Vignette>();
        m_ColorGrading = m_postProcessVolume.profile.GetSetting<ColorGrading>();
        ori = m_Vignette.intensity.value;
        mixerRedOutRedIn = m_ColorGrading.mixerRedOutRedIn.value;
        oriIntensity = _staffLight.intensity;

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

    public void PlayerHurtEffect(Vector3 pos, float damageRatio)
    {
        _playerHurtIntensity = Mathf.Min(1, damageRatio) * playerHurtIntensityMultiplier;
        PlayEffectAtPosition("bloodExplosion", pos).transform.localScale *= 0.1f + 2 * _playerHurtIntensity;
        StartCoroutine(PlayerHurt());
    }

    IEnumerator PlayerHurt()
    {
        m_Vignette.intensity.value =  ori +  0.5f * _playerHurtIntensity;
        m_ColorGrading.mixerRedOutRedIn.value = mixerRedOutRedIn + 50f * _playerHurtIntensity ;
        Time.timeScale = 0.1f;
        ScreenShakeManager.Instance.ScreenShake(0.3f * _playerHurtIntensity, 0.9f * _playerHurtIntensity);
        yield return new WaitForSeconds(0.06f * _playerHurtIntensity);
        Time.timeScale = 1;
        m_Vignette.intensity.value = ori;
        m_ColorGrading.mixerRedOutRedIn.value = mixerRedOutRedIn;
    }

    public void UseStaffEffect(float time = 0.15f)
    {
        if (_staffLight)
        {
            _staffLight.intensity *= 8f;
            DOTween.To(() => _staffLight.intensity, 
                x => _staffLight.intensity =  x, 
                oriIntensity, time).SetEase(Ease.InQuad);
        }
    }

    public void EnemyHurtEffect()
    {
        StartCoroutine(EnemyHurt());
    }

    IEnumerator EnemyHurt()
    {
        Time.timeScale = 0.1f;
        // ScreenShakeManager.Instance.ScreenShake(0.05f * _enemyHurtTimeStopIntensity, 0.5f);
        yield return new WaitForSeconds(0.01f * _enemyHurtTimeStopIntensity);
        Time.timeScale = 1;
    }
}
