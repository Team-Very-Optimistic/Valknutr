using System;
using UnityEngine;

[CreateAssetMenu]
public class RepeatSpellModifier : SpellModifier
{
    public int iterations = 2;
    private SpellBase action;
    private TriggerEventHandler eventHandler;
    
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action._behaviour;
        var i = iterations;
        Action temp = () =>
        {
            oldBehavior.Invoke();
            if (--i <= 0) return;
            eventHandler = action._objectForSpell.GetComponent<TriggerEventHandler>();
            if (eventHandler != null)
                //existingHandler = action._objectForSpell.AddComponent<TriggerEventHandler>();
                eventHandler.AddEvent(Invoke);
            
            this.action = action;
        };

        Action spell = temp;
        action._behaviour = spell;
        return action;
    }

    public override void UseValue()
    {
        iterations = Mathf.RoundToInt(iterations * value);
    }

    public void Invoke(Collider collider)
    {
        action._objectForSpell = collider.gameObject;
        eventHandler.RemoveEvent(Invoke);
        action._behaviour.Invoke();
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip(DefaultModTitle(), "Repeats the spell and changes the central object of the spell to the first collided object." + DefaultModBody());
    }
}