using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Create SFXLibrary", fileName = "SFXLibrary", order = 0)]
[Serializable]
public class SFXLibrary : ScriptableObject
{
    public AudioManager.SoundEntry[] m_SfxLibrary;
}