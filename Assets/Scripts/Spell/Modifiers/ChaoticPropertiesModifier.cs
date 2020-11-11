using UnityEngine;

[CreateAssetMenu]
public class ChaoticPropertiesModifier : SpellModifier
{
    public float damageBase = 1f;
    public float speedBase = 1f;
    public float _cooldownBase;
    public override void ModifySpell(SpellBase spell)
    {
        base.ModifySpell(spell);
        
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
            Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)
        };
        var allBad = true;
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
        speedBase *= value * Random.Range(-1, 1);
        damageBase *= value * Random.Range(-1, 1);
        _cooldownBase = _cooldownMultiplier * value * Random.Range(-1, 1);
        
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip("Pandemonium" + DefaultModTitle(), $"Increase or decrease the properties of the spell, modify base spell cooldown by {_cooldownBase:F1}, " +
                                                              $"modify base damage by {damageBase:F1}, modify base speed by {speedBase:F1}. {DefaultModBody()}");
    }
}