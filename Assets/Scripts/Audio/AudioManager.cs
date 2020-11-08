using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [System.Serializable]
    public class SoundEntry
    {
        public string m_Identifier;
        public AudioClip m_Clip;

        [Range(0.0f, 3.0f)]
        public float m_Volume = 1;
        [Range(0.1f, 3.0f)]
        public float m_Pitch = 1;

        [HideInInspector]
        public AudioSource m_Source;
    }

    [SerializeField]
    private AudioMixer m_MasterMixer;

    private SoundEntry[] m_SfxLibrary;
    private SoundEntry[] m_bgLibrary;

    [SerializeField]
    private SFXLibrary _library;
    [SerializeField]
    private SFXLibrary _backgroundMusic;

    private AudioSource backgroundAudio;
   
    void Awake()
    {
        m_SfxLibrary = _library.m_SfxLibrary;
        foreach(SoundEntry s in m_SfxLibrary)
        {
            GameObject soundObject = new GameObject(s.m_Identifier + " source");
            soundObject.transform.parent = transform;
            s.m_Source = soundObject.AddComponent<AudioSource>();
            s.m_Source.outputAudioMixerGroup = m_MasterMixer.FindMatchingGroups("SFX")[0];
            s.m_Source.clip = s.m_Clip;
            s.m_Source.volume = s.m_Volume;
            s.m_Source.pitch = s.m_Pitch;
        }
        m_bgLibrary = _backgroundMusic.m_SfxLibrary;
        foreach(SoundEntry s in m_bgLibrary)
        {
            GameObject soundObject = new GameObject(s.m_Identifier + " source");
            soundObject.transform.parent = transform;
            s.m_Source = soundObject.AddComponent<AudioSource>();
            s.m_Source.outputAudioMixerGroup = m_MasterMixer.FindMatchingGroups("SFX")[0];
            s.m_Source.clip = s.m_Clip;
            s.m_Source.volume = s.m_Volume;
            s.m_Source.pitch = s.m_Pitch;
        }
    }

    public static GameObject PlaySoundAtPosition(string identifier, Vector3 position, float volume = 0, float pitch = 0)
    {
        SoundEntry s = Array.Find(Instance.m_SfxLibrary, sound => sound.m_Identifier == identifier);

        if (s == null)
        {
            Debug.LogWarning("The requested sound \"" + identifier + "\" does not exist!");
            return null;
        }

        GameObject tempSoundPlayer = Instantiate(s.m_Source.gameObject);
        tempSoundPlayer.transform.position = position;
        AudioSource audioSource = tempSoundPlayer.GetComponent<AudioSource>();
        if(volume > 0)
            audioSource.volume *= volume;
        if(pitch > 0)
            audioSource.pitch *= pitch;

        audioSource.Play();
        Destroy(tempSoundPlayer, s.m_Clip.length);
        return tempSoundPlayer;
    }

    public static void PlaySound(string identifier, float volume = 1, float pitch = 1)
    {
        SoundEntry s = Array.Find(Instance.m_SfxLibrary, sound => sound.m_Identifier == identifier);

        if (s == null)
        {
            Debug.LogWarning("The requested sound \"" + identifier + "\" does not exist!");
            return;
        }

        GameObject tempSoundPlayer = Instantiate(s.m_Source.gameObject);
        AudioSource audioSource = tempSoundPlayer.GetComponent<AudioSource>();
        audioSource.volume *= volume;
        audioSource.pitch *= pitch;
        audioSource.spatialBlend = 0.0f; // Important

        audioSource.Play();
        Destroy(tempSoundPlayer, s.m_Clip.length);
    }
    
    public static void PlayBackgroundSound(string identifier, float volume = 1, float pitch = 1)
    {
        if(Instance.backgroundAudio)
            DOTween.To(() => Instance.backgroundAudio.volume, 
            x => Instance.backgroundAudio.volume =  x, 
            0, 1f).SetEase(Ease.InQuad);
        
        SoundEntry s = Array.Find(Instance.m_bgLibrary, sound => sound.m_Identifier == identifier);

        if (s == null)
        {
            Debug.LogWarning("The requested sound \"" + identifier + "\" does not exist!");
            return;
        }

        GameObject tempSoundPlayer = Instantiate(s.m_Source.gameObject);
        AudioSource audioSource = tempSoundPlayer.GetComponent<AudioSource>();
        audioSource.volume *= volume;
        audioSource.pitch *= pitch;
        audioSource.spatialBlend = 0.0f; // Important
        Instance.backgroundAudio = audioSource;

        audioSource.Play();
        Destroy(tempSoundPlayer, s.m_Clip.length);
    }
}