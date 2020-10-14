using System;
using UnityEngine;

[CreateAssetMenu]
public class RepeatSpellModifier : SpellModifier
{
    private int n = 2;
    private SpellBase action;
    private TriggerEventHandler eventHandler;
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action temp = () =>
        {
            oldBehavior.Invoke();
            Debug.Log(n);
            if (--n < 0) return;
            eventHandler = action._objectForSpell.GetComponent<TriggerEventHandler>();
            if (eventHandler != null)
                //existingHandler = action._objectForSpell.AddComponent<TriggerEventHandler>();
    
                eventHandler.AddEvent(Invoke);
            
            this.action = action;
        };

        Action spell = temp;
        action.behaviour = spell;
        return action;
    }

    public void Invoke(Collider collider)
    {
        action._objectForSpell = collider.gameObject;
        eventHandler.RemoveEvent(Invoke);
        action.behaviour.Invoke();
    }
}