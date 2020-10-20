using UnityEngine;

public class SpellManager : Singleton<SpellManager>
{
    /*
     * Used as a database to store prefabs
     */
    public GameObject fireObject;
    public Spell[] _defaultSpells;
    public int maxShields = 10;
    private int currShields = 0;
    public GameObject skeleton;

    public Spell[] GetDefaultSpells()
    {
        // var movementSpell = ScriptableObject.CreateInstance<Spell>();
        // var projectileSpell = ScriptableObject.CreateInstance<Spell>();
        // var meleeSpell = ScriptableObject.CreateInstance<Spell>();
        //
        // var shieldSpell = ScriptableObject.CreateInstance<Spell>();
        // var bombSpell = ScriptableObject.CreateInstance<Spell>();
        //
        //
        // var meleeBehavior = ScriptableObject.CreateInstance<GroundStrikeBehaviour>();
        // meleeSpell.AddBaseType(meleeBehavior);
        //
        // var movementBehavior = ScriptableObject.CreateInstance<MovementBehavior>();
        //
        // movementSpell.AddBaseType(movementBehavior);
        //
        // var shieldBehavior = ScriptableObject.CreateInstance<ShieldBehavior>();
        // shieldSpell.AddBaseType(shieldBehavior);
        //
        // var projectileBehavior = ScriptableObject.CreateInstance<ProjectileBehavior>();
        // projectileSpell.AddBaseType(projectileBehavior);
        //
        // var novaBehavior = ScriptableObject.CreateInstance<ExplosiveBehaviour>();
        // novaBehavior._objectForSpell = Instance.explosionObject;
        // bombSpell.AddBaseType(novaBehavior);
        //
        // return new[] {meleeSpell, movementSpell, shieldSpell, projectileSpell};
        return _defaultSpells;
    }

    public bool ShieldFull()
    {
        return currShields >= maxShields;
    }

    public void AddShield()
    {
        currShields++;
    }
    public void RemoveShield()
    {
        currShields--;
    }
}