using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : Singleton<DamageTextManager>
{
    public GameObject damageTextPrefab;

    public static void SpawnDamage(float finalDamage, Vector3 worldPositionText, Color damageColor)
    {
        var damageText = Instantiate(Instance.damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(finalDamage, worldPositionText, damageColor);
    }
}
