
using UnityEngine;

public class SpellManager : Singleton<SpellManager>
{
    /*
     * Used as a database to store prefabs
     */
    public GameObject projectileObject;
    public GameObject shieldObject;
    public GameObject fireObject;
    public GameObject explosionObject;

    public static Spell[] GetDefaultSpells()
    {
        var movementSpell = ScriptableObject.CreateInstance<Spell>();
        var projectileSpell = ScriptableObject.CreateInstance<Spell>();
        var shieldSpell = ScriptableObject.CreateInstance<Spell>();
        var bombSpell = ScriptableObject.CreateInstance<Spell>();

        var movementBehavior = ScriptableObject.CreateInstance<MovementBehavior>();
        movementBehavior.Init();
        movementSpell.AddBaseType(movementBehavior);
        
        var shieldBehavior = ScriptableObject.CreateInstance<ShieldBehavior>();
        shieldBehavior.Init();
        shieldSpell.AddBaseType(shieldBehavior);

        var projectileBehavior = ScriptableObject.CreateInstance<ProjectileBehavior>();
        projectileBehavior.Init();
        projectileSpell.AddBaseType(projectileBehavior);

        var novaBehavior = ScriptableObject.CreateInstance<NovaBehavior>();
        novaBehavior.Init();
        novaBehavior._objectForSpell = Instance.explosionObject;
        bombSpell.AddBaseType(novaBehavior);

        return new [] {projectileSpell,  shieldSpell, movementSpell, bombSpell};
    }
}
