using System;

class FireSpellModifier : SpellModifier
{
    private float fireMultiplier = 1.2f;
    public override void ModifySpell(SpellBase spell)
    {
        //base.ModifySpell(spell);
        spell._speed *= fireMultiplier;
        spell._cooldown *= _cooldownMultiplier;
    }
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            action._objectForSpell.AddComponent<Fire>().SetInitializer();
        };
        action.behaviour = spell;
        return action;
    }

    public override void UseQuality()
    {
        fireMultiplier *=  quality;
    }
}