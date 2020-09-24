using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [System.Serializable]
    public class EffectEntry
    {
        public string m_Identifier;
        public GameObject effect;
    }
    
    [SerializeField]
    private EffectEntry[] m_VFXLibrary;
    
    public static void PlayEffectAtPosition(string identifier, Vector3 position)
    {
        EffectEntry s = Array.Find(Instance.m_VFXLibrary, effect => effect.m_Identifier == identifier);

        if (s == null)
        {
            Debug.LogWarning("The requested effect \"" + identifier + "\" does not exist!");
            return;
        }

        GameObject temp = Instantiate(s.effect, position, Quaternion.identity);
      
    }
}
