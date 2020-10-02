using System;

class FireSpellModifier : SpellModifier
{
    public override void ModifySpell(SpellBase spell)
    {
        spell._speed *= 1.2f;
        spell._cooldown *= 1.2f;
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
    
}