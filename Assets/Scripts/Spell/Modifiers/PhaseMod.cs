using System;

class PhaseMod : SpellModifier
{
    
    public override SpellBaseType ModifyBehaviour(SpellBaseType action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        
        Action spell = () =>
        {
            oldBehavior.Invoke();
            action._objectForSpell.AddComponent<Phasing>()._damage = action._damage;
        };
        
        action.behaviour = spell;
        return action;
    }
}