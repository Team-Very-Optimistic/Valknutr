using UnityEngine;

[CreateAssetMenu]
public class ChaoticPropertiesModifier : SpellModifier
{
    public float damageBase = 1f;
    public float speedBase = 1f;
    public float _cooldownBase;
    public GameObject randObj;
    public float randChance = 1.0f;
    public override void ModifySpell(SpellBase spell)
    {
        base.ModifySpell(spell);
        if (Random.value <= randChance)
            spell._objectForSpell = randObj;
        spell._damage += damageBase;
        if (spell._damage <= 0) spell._damage = 0;
        
        spell._speed += speedBase;
        if (spell._speed <= 0) spell._speed = 0;

        spell._cooldown += _cooldownBase;
        if (spell._cooldown <= 0) spell._cooldown = 0;
    }

    public override void UseValue()
    {
        var rand = new float[3]
        {
            Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)
        };
        var allBad = true;
        randObj = new GameObject[]
        {
            SpellManager.Instance.chaosPrefabs[0],
            SpellManager.Instance.chaosPrefabs[1],
            SpellManager.Instance.chaosPrefabs[2],
            SpellManager.Instance.chaosPrefabs[3],

            SpellManager.Instance.skeleton

        }[Random.Range(0, 5)];
        for (int i = 0; i < 3; i++)
        {
            if (rand[i] >= 0)
            {
                allBad = false;
                break;
            }
        }

        if (allBad)
        {
            int v = Random.Range(0, 3);
            rand[v] *= -1;
            rand[v] -= 0.1f;
        }
        speedBase *= value * Random.Range(-1f, 1f);
        damageBase *= value * Random.Range(-1f, 1f);
        _cooldownBase = _cooldownMultiplier * value * Random.Range(-1f, 1f);
        
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip("Pandemonium" + DefaultModTitle(), $"Increase or decrease the properties of the spell, modify base spell cooldown by {_cooldownBase:F1}, " +
                                                              $"modify base damage by {damageBase:F1}, modify base speed by {speedBase:F1}. {DefaultModBody()}");
    }
}