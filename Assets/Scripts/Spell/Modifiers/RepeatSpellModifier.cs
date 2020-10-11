using System;
using UnityEngine;

[CreateAssetMenu]
public class RepeatSpellModifier : SpellModifier
{
    private int n = 2;
    private SpellBase action;
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action temp = () =>
        {
            oldBehavior.Invoke();
            if (--n < 0) return;
            var existingHandler = action._objectForSpell.GetComponent<TriggerEventHandler>();
            if (existingHandler != null)
                //existingHandler = action._objectForSpell.AddComponent<TriggerEventHandler>();
    
                existingHandler.AddEvent(Invoke);
            
            this.action = action;
        };

        Action spell = temp;
        action.behaviour = spell;
        return action;
    }

    public void Invoke(Collider collider)
    {
        action._objectForSpell = collider.gameObject;
        action.behaviour.Invoke();
    }
}