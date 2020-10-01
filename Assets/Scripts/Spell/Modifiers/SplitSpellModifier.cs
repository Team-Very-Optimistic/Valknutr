using System;
using UnityEngine;
using Random = UnityEngine.Random;

class SplitSpellModifier : SpellModifier
{
    private int n = 2;
    private float randomMax = 0.2f;
    public override SpellBehavior ModifyBehaviour(SpellBehavior action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        
        Action spell = () =>
        {
            for (int i = 0; i < n; i++)
            {
                Vector3 originalPosDiff = action._posDiff;
                action._posDiff += new Vector3(Random.Range(-randomMax, randomMax), 0, Random.Range(-randomMax, randomMax));
                action._posDiff.Normalize();
                oldBehavior.Invoke();
                action._posDiff = originalPosDiff; //reset
            }
        };
        action.behaviour = spell;
        return action;
    }
    
}
