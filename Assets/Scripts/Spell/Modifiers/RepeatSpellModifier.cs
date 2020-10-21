using System;
using UnityEngine;

[CreateAssetMenu]
public class RepeatSpellModifier : SpellModifier
{
    public int iterations = 2;
    private SpellBase action;
    private TriggerEventHandler eventHandler;
    
    // public override SpellBase ModifyBehaviour(SpellBase action)
    // {
    //     //important to make sure it doesnt cast a recursive method
    //     Action oldBehavior = action.behaviour;
    //     
    //     Action temp = () =>
    //     {
    //         oldBehavior.Invoke();
    //         if (--iterations < 0) return;
    //         eventHandler = action.objectForSpell.GetComponent<TriggerEventHandler>();
    //         if (eventHandler != null)
    //             //existingHandler = action._objectForSpell.AddComponent<TriggerEventHandler>();
    //
    //             eventHandler.AddEvent(Invoke);
    //         
    //         this.action = action;
    //     };
    //
    //     Action spell = temp;
    //     action.behaviour = spell;
    //     return action;
    // }
    
    public override SpellContext ModifyBehaviour(SpellContext ctx)
    {
        var oldBehavior = ctx.action;
        ctx.action = ctx2 =>
        {
            oldBehavior(ctx2);
            if (--ctx2.iterations < 0) return;
            // todo: i dont know how to refactor this, probably broken
            eventHandler = ctx2.objectForSpell.GetComponent<TriggerEventHandler>();
            if (eventHandler != null)
                //existingHandler = action._objectForSpell.AddComponent<TriggerEventHandler>();
    
                eventHandler.AddEvent(Invoke);
            
            this.action = action;
        };
        
        ctx.cooldown *= _cooldownMultiplier;
        return ctx;
    }

    public override void UseValue()
    {
        iterations = Mathf.RoundToInt(iterations * value);
    }

    public void Invoke(Collider collider)
    {
        action.objectForSpell = collider.gameObject;
        eventHandler.RemoveEvent(Invoke);
        action.behaviour.Invoke(default);
    }
    
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip(DefaultModTitle(ctx), "Repeats the spell and changes the central object of the spell to the first object that had been interacted." + DefaultModBody(ctx));
    }
}