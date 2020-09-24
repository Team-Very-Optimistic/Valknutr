using System;
using UnityEngine;
using Random = UnityEngine.Random;

class SplitSpellModifier : SpellModifier
{
    private int n = 2;
    public override SpellBehavior ModifyBehaviour(SpellBehavior action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        
        Action spell = () =>
        {
            for (int i = 0; i < n; i++)
            {
                Vector3 originalPosDiff = action._posDiff;
                action._posDiff += new Vector3(Random.Range(-0.1f,0.1f), 0, Random.Range(-0.1f, 0.1f));
                action._posDiff.Normalize();
                oldBehavior.Invoke();
                action._posDiff = originalPosDiff; //reset
            }
        };
        action.behaviour = spell;
        return action;
    }
    
}
