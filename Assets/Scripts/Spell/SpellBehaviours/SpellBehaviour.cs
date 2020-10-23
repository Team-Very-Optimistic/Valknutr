
public abstract class SpellBehaviour : TriggerEventHandler
{
    public void Set(SpellBase baseSpell, params float[] additionalProperties)
    {
        SetProperties(baseSpell._damage, baseSpell._scale, baseSpell._speed, baseSpell._cooldown, additionalProperties);
    }
    
    public abstract void SetProperties(float damage, float scale, float speed,  float cooldown, params float[] additionalProperties);
}
